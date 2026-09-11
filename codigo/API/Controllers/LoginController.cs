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
    }
}