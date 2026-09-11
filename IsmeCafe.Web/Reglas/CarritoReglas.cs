using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Reglas
{
    public class CarritoReglas : ICarritoReglas
    {
        private readonly ICarritoServicio _carritoServicio;

        public CarritoReglas(ICarritoServicio carritoServicio)
        {
            _carritoServicio = carritoServicio;
        }

        public Task<CarritoResponse> Obtener(Guid idUsuario, bool usarPuntos = false) =>
        _carritoServicio.Obtener(idUsuario, usarPuntos);

        public Task<CarritoResponse> Agregar(CarritoItemRequest item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (item.Cantidad <= 0) throw new Exception("La cantidad debe ser mayor a cero.");
            return _carritoServicio.Agregar(item);
        }

        public Task<CarritoResponse> Actualizar(CarritoItemRequest item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (item.Cantidad <= 0) throw new Exception("La cantidad debe ser mayor a cero.");
            return _carritoServicio.Actualizar(item);
        }

        public Task<CarritoResponse> Eliminar(Guid idUsuario, Guid idProducto) =>
            _carritoServicio.Eliminar(idUsuario, idProducto);
    }
}
