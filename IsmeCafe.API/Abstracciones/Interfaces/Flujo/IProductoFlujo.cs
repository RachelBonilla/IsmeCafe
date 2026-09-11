using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IProductoFlujo
    {
        Task<IEnumerable<ProductoResponse>> Obtener();
        Task<ProductoDetalle> Obtener(Guid Id);
        Task<IEnumerable<ProductoResponse>> ObtenerActivos();
        Task<Guid> Agregar(ProductoRequest producto);
        Task<Guid> Editar(Guid Id, ProductoRequest producto);
        Task<Guid> Eliminar(Guid Id);
    }
}
