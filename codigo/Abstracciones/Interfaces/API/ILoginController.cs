using Abstracciones.Modelos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface ILoginController
    {
        Task<ActionResult> IniciarSesion(LoginRequest loginRequest);
    }
}