using Abstracciones.Modelos.Auth;

namespace Abstracciones.Interfaces.Servicios
{
    public interface ILoginServicio
    {
        Task<LoginResponse> IniciarSesion(LoginRequest credenciales);
    }
}
