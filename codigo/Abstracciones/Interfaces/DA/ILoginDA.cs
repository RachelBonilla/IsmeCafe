using Abstracciones.Modelos.Auth;

namespace Abstracciones.Interfaces.DA
{
    public interface ILoginDA
    {
        Task<UsuarioAutenticacion> ObtenerUsuarioPorCorreo(string correo);
    }
}
