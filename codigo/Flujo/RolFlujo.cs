using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class RolFlujo : IRolFlujo
    {
        private readonly IRolDA _rolDA;

        public RolFlujo(IRolDA rolDA)
        {
            _rolDA = rolDA;
        }

        public async Task<IEnumerable<Rol>> Obtener()
        {
            return await _rolDA.Obtener();
        }
    }
}
