using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IRolFlujo
    {
        Task<IEnumerable<Rol>> Obtener();
    }
}
