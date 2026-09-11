using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Reglas
{
    public class ReservaReglas : IReservaReglas
    {
        private readonly IReservaServicios _reservaServicios;

        public ReservaReglas(
            IReservaServicios reservaServicios)
        {
            _reservaServicios = reservaServicios;
        }

        public Task<IEnumerable<ReservaResponse>> ObtenerTodos()
            => _reservaServicios.ObtenerTodos();

        public Task<ReservaResponse?> Obtener(Guid id)
            => _reservaServicios.Obtener(id);

        public Task<bool> Agregar(ReservaRequest reserva)
            => _reservaServicios.Agregar(reserva);

        public Task<bool> Editar(
            Guid id,
            ReservaRequest reserva)
            => _reservaServicios.Editar(id, reserva);

        public Task<bool> Eliminar(Guid id)
            => _reservaServicios.Eliminar(id);
    }
}