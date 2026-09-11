using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Servicios
{
    public interface IDescuentoServicio
    {
        Task<IEnumerable<DescuentoResponse>> Obtener();
        Task<IEnumerable<DescuentoResponse>> ObtenerActivos();
        Task<DescuentoDetalle?> Obtener(Guid Id);
        Task<Guid> Agregar(DescuentoRequest descuento);
        Task<Guid> Editar(Guid Id, DescuentoRequest descuento);
        Task<Guid> Eliminar(Guid Id);
    }
}