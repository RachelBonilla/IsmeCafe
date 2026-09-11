using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class CarritoFlujo : ICarritoFlujo
    {
        private readonly ICarritoDA _carritoDA;
        private readonly IProductoDA _productoDA;
        private readonly IConfiguracionLealtadDA _configuracionLealtadDA;
        private readonly IUsuarioDA _usuarioDA;

        public CarritoFlujo(
            ICarritoDA carritoDA,
            IProductoDA productoDA,
            IConfiguracionLealtadDA configuracionLealtadDA,
            IUsuarioDA usuarioDA)
        {
            _carritoDA = carritoDA;
            _productoDA = productoDA;
            _configuracionLealtadDA = configuracionLealtadDA;
            _usuarioDA = usuarioDA;
        }

        public async Task<CarritoResponse> Obtener(Guid idUsuario, bool usarPuntos = false)
        {
            var items = await _carritoDA.Obtener(idUsuario);
            return await Construir(idUsuario, items, usarPuntos);
        }

        public async Task<CarritoResponse> Agregar(CarritoItemRequest item)
        {
            if (item.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");

            var producto = await _productoDA.Obtener(item.IdProducto)
                ?? throw new Exception("El producto no existe.");

            if (!producto.Activo)
                throw new Exception("El producto no está activo.");

            if (producto.Cantidad <= 0)
                throw new Exception("El producto está agotado.");

            await _carritoDA.Agregar(item.IdUsuario, item.IdProducto, item.Cantidad);
            return await Obtener(item.IdUsuario);
        }

        public async Task<CarritoResponse> Actualizar(CarritoItemRequest item)
        {
            if (item.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");

            await _carritoDA.Actualizar(item.IdUsuario, item.IdProducto, item.Cantidad);
            return await Obtener(item.IdUsuario);
        }

        public async Task<CarritoResponse> Eliminar(Guid idUsuario, Guid idProducto)
        {
            await _carritoDA.Eliminar(idUsuario, idProducto);
            return await Obtener(idUsuario);
        }

        private async Task<CarritoResponse> Construir(Guid idUsuario, IEnumerable<CarritoItemResponse> items, bool usarPuntos)
        {
            var lista = items?.ToList() ?? new List<CarritoItemResponse>();
            var total = lista.Sum(i => i.Subtotal);

            var configuracion = await _configuracionLealtadDA.Obtener();
            var usuario = await _usuarioDA.Obtener(idUsuario);
            int puntosDisponibles = usuario?.Puntos ?? 0;

            int puntosAUsar = 0;
            decimal descuentoPuntos = 0;

            if (usarPuntos && puntosDisponibles > 0 && configuracion.ValorPunto > 0)
            {
                decimal valorTotalPuntos = puntosDisponibles * configuracion.ValorPunto;

                puntosAUsar = valorTotalPuntos >= total
                    ? (int)Math.Floor(total / configuracion.ValorPunto)
                    : puntosDisponibles;

                descuentoPuntos = puntosAUsar * configuracion.ValorPunto;
            }

            var totalConDescuento = total - descuentoPuntos;

            int puntosAGanar = configuracion.PuntosPorMonto > 0
                ? (int)Math.Floor(totalConDescuento / configuracion.PuntosPorMonto)
                : 0;

            return new CarritoResponse
            {
                IdUsuario = idUsuario,
                Items = lista,
                CantidadItems = lista.Sum(i => i.Cantidad),
                Total = total,
                PuntosAGanar = puntosAGanar,
                PuntosDisponibles = puntosDisponibles,
                UsarPuntos = usarPuntos,
                PuntosAUsar = puntosAUsar,
                DescuentoPuntos = descuentoPuntos,
                TotalConDescuento = totalConDescuento
            };
        }
    }
}