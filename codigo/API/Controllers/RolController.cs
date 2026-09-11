using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase, IRolController
    {
        private readonly IRolFlujo _rolFlujo;

        public RolController(IRolFlujo rolFlujo)
        {
            _rolFlujo = rolFlujo;
        }

        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var resultado = await _rolFlujo.Obtener();

            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }
    }
}
