using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IRolReglas
    {
        Task<IEnumerable<Rol>> Obtener();
    }
}
