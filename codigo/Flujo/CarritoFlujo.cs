using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class CarritoFlujo : ICarritoFlujo
    {
        private readonly ICarritoDA _carritoDA;
        private readonly IProductoDA _productoDA;

        public CarritoFlujo(ICarritoDA carritoDA, IProductoDA productoDA)
        {
            _carritoDA = carritoDA;
            _productoDA = productoDA;
        }

        public async Task<CarritoResponse> Obtener(Guid idUsuario)
        {
            var items = await _carritoDA.Obtener(idUsuario);
            return Construir(idUsuario, items);
        }

        public async Task<CarritoResponse> Agregar(CarritoItemRequest item)
        {
            if (item.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");

            // HU-21 CA1/CA3: validar existencia, estado activo y stock del producto.
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
            // HU-22 CA3: la cantidad debe ser un número positivo.
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

        private static CarritoResponse Construir(Guid idUsuario, IEnumerable<CarritoItemResponse> items)
        {
            var lista = items?.ToList() ?? new List<CarritoItemResponse>();
            return new CarritoResponse
            {
                IdUsuario = idUsuario,
                Items = lista,
                CantidadItems = lista.Sum(i => i.Cantidad),
                Total = lista.Sum(i => i.Subtotal)
            };
        }
    }
}
