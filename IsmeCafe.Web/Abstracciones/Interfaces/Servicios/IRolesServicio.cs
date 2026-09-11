using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Servicios
{
    public interface IRolServicio
    {
        Task<IEnumerable<Rol>> Obtener();
    }
}