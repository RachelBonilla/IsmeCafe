using Abstracciones.Modelos.Auth;

namespace Abstracciones.Interfaces.Reglas
{
    public interface ILoginReglas
    {
        Task<LoginResponse> IniciarSesion(LoginRequest credenciales);
        Task<bool> SolicitarRecuperacion(SolicitarRecuperacionRequest request);
        Task<bool> RestablecerContrasena(RestablecerContrasenaRequest request);
    }
}
