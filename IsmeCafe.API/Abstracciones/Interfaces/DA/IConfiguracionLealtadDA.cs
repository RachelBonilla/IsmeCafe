using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IConfiguracionLealtadDA
    {
        Task<ConfiguracionLealtadResponse> Obtener();
    }
}