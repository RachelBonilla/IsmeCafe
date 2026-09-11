using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IDescuentoDA
    {

        Task<IEnumerable<DescuentoResponse>> Obtener();
        Task<DescuentoDetalle> Obtener(Guid Id);
        Task<IEnumerable<DescuentoResponse>> ObtenerActivos();
        Task<Guid> Agregar(DescuentoRequest descuento);
        Task<Guid> Editar(Guid Id, DescuentoRequest descuento);
        Task<Guid> Eliminar(Guid Id);
    }
}
