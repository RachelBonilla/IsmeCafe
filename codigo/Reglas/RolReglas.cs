using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Reglas
{
    public class RolReglas : IRolReglas
    {
        private readonly IRolServicio _rolServicio;

        public RolReglas(IRolServicio rolServicio)
        {
            _rolServicio = rolServicio;
        }

        public Task<IEnumerable<Rol>> Obtener() => _rolServicio.Obtener();
    }
}