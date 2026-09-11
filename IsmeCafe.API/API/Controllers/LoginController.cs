using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase, ILoginController
    {
        private readonly ILoginFlujo _loginFlujo;

        public LoginController(ILoginFlujo loginFlujo)
        {
            _loginFlujo = loginFlujo;
        }

        [HttpPost("IniciarSesion")]
        public async Task<ActionResult> IniciarSesion([FromBody] LoginRequest loginRequest)
        {
            var resultado = await _loginFlujo.IniciarSesion(loginRequest);
            return Ok(resultado);
        }

        [HttpPost("SolicitarRecuperacion")]
        public async Task<ActionResult> SolicitarRecuperacion([FromBody] SolicitarRecuperacionRequest request)
        {
            await _loginFlujo.SolicitarRecuperacion(request);

            return Ok(new{mensaje = "Si el correo está registrado, recibirá un código de recuperación."});
        }

        [HttpPost("RestablecerContrasena")]
        public async Task<ActionResult> RestablecerContrasena([FromBody] RestablecerContrasenaRequest request)
        {
            await _loginFlujo.RestablecerContrasena(request);

            return Ok(new{mensaje = "La contraseña fue restablecida correctamente."});
        }
    }
}