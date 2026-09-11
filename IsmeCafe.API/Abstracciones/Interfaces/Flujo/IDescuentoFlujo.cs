using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IDescuentoFlujo
    {
        Task<IEnumerable<DescuentoResponse>> Obtener();
        Task<DescuentoDetalle> Obtener(Guid Id);
        Task<IEnumerable<DescuentoResponse>> ObtenerActivos();
        Task<Guid> Agregar(DescuentoRequest descuento);
        Task<Guid> Editar(Guid Id, DescuentoRequest descuento);
        Task<Guid> Eliminar(Guid Id);
    }
}
