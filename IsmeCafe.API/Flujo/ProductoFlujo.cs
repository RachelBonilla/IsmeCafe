using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ProductoFlujo : IProductoFlujo
    {
        private IProductoDA _productoDA;

        public ProductoFlujo(IProductoDA productoDA)
        {
            _productoDA = productoDA;
        }

        public Task<Guid> Agregar(ProductoRequest producto)
        {
            return _productoDA.Agregar(producto);
        }

        public Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            return _productoDA.Editar(Id, producto);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return _productoDA.Eliminar(Id);
        }

        public Task<IEnumerable<ProductoResponse>> Obtener()
        {
            return _productoDA.Obtener();
        }

        public Task<ProductoDetalle> Obtener(Guid Id)
        {
            return _productoDA.Obtener(Id);
        }

        public Task<IEnumerable<ProductoResponse>> ObtenerActivos()
        {
            return _productoDA.ObtenerActivos();
        }
    }
}
