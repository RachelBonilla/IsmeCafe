using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IReservaFlujo
    {
        Task<IEnumerable<ReservaResponse>> Obtener();
        Task<ReservaResponse> Obtener(Guid Id);
        Task<Guid> Agregar(ReservaRequest reserva);
        Task<Guid> Editar(Guid Id, ReservaRequest reserva);
        Task<Guid> Eliminar(Guid Id);
    }
}