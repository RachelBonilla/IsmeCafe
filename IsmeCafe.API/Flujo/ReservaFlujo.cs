using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ReservaFlujo : IReservaFlujo
    {
        private readonly IReservaDA _reservaDA;

        public ReservaFlujo(IReservaDA reservaDA)
        {
            _reservaDA = reservaDA;
        }

        public async Task<Guid> Agregar(ReservaRequest reserva)
        {
            return await _reservaDA.Agregar(reserva);
        }

        public async Task<Guid> Editar(Guid Id, ReservaRequest reserva)
        {
            return await _reservaDA.Editar(Id, reserva);
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            return await _reservaDA.Eliminar(Id);
        }

        public async Task<IEnumerable<ReservaResponse>> Obtener()
        {
            return await _reservaDA.Obtener();
        }

        public async Task<ReservaResponse> Obtener(Guid Id)
        {
            return await _reservaDA.Obtener(Id);
        }
    }
}