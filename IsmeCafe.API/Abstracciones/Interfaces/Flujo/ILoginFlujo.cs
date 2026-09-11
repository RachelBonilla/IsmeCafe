using Abstracciones.Modelos.Auth;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ILoginFlujo
    {
        Task<LoginResponse> IniciarSesion(LoginRequest request);
        Task SolicitarRecuperacion(SolicitarRecuperacionRequest request);
        Task RestablecerContrasena(RestablecerContrasenaRequest request);
    }
}