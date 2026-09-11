using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampanaMarketingController : ControllerBase, ICampanaMarketingController
    {
        private ICampanaMarketingFlujo _campanaFlujo;
        private ILogger<CampanaMarketingController> _logger;

        public CampanaMarketingController(ICampanaMarketingFlujo campanaFlujo, ILogger<CampanaMarketingController> logger)
        {
            _campanaFlujo = campanaFlujo;
            _logger = logger;
        }

        //Parte de orfertas
        [HttpPost("enviar-oferta")]
        public async Task<ActionResult> EnviarOferta([FromBody] CampanaOfertaRequest campana)
        {
            try
            {
                var resultado = await _campanaFlujo.EnviarOferta(campana);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar campaña de oferta");
                return BadRequest(ex.Message);
            }
        }

       //Parte de descuento
        [HttpPost("enviar-descuento")]
        public async Task<ActionResult> EnviarDescuento([FromBody] CampanaDescuentoRequest campana)
        {
            try
            {
                var resultado = await _campanaFlujo.EnviarDescuento(campana);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar campaña de descuento");
                return BadRequest(ex.Message);
            }
        }

        //Historial de campañas/correos enviados
        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var resultado = await _campanaFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();

            return Ok(resultado);
        }
    }
}