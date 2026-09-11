using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IReservaReglas
    {
        Task<IEnumerable<ReservaResponse>> ObtenerTodos();
        Task<ReservaResponse?> Obtener(Guid id);
        Task<bool> Agregar(ReservaRequest reserva);
        Task<bool> Editar(Guid id, ReservaRequest reserva);
        Task<bool> Eliminar(Guid id);
    }
}