using Abstracciones.Modelos.Auth;

namespace Abstracciones.Interfaces.DA
{
    public interface ILoginDA
    {
        Task<UsuarioAutenticacion> ObtenerUsuarioPorCorreo(string correo);
        Task ActualizarContrasena(Guid idUsuario, string contrasenaHash);
    }
}
