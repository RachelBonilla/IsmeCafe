using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IRolDA
    {
        Task<IEnumerable<Rol>> Obtener();
    }
}
