using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase, IReporteController
    {
        private readonly IReporteFlujo _reporteFlujo;
        private readonly ILogger<ReporteController> _logger;

        public ReporteController(IReporteFlujo reporteFlujo, ILogger<ReporteController> logger)
        {
            _reporteFlujo = reporteFlujo;
            _logger = logger;
        }

        [HttpGet("inventario")]
        public async Task<ActionResult> Inventario([FromQuery] Guid? idCategoria)
        {
            try
            {
                var lineas = await _reporteFlujo.Inventario(idCategoria);
                return Ok(lineas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de inventario");
                return BadRequest(new { mensaje = $"No fue posible generar el reporte de inventario: {ex.Message}" });
            }
        }

        [HttpGet("catalogo")]
        public async Task<ActionResult> Catalogo([FromQuery] bool? activo)
        {
            try
            {
                var lineas = await _reporteFlujo.Catalogo(activo);
                return Ok(lineas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de catálogo");
                return BadRequest(new { mensaje = $"No fue posible generar el reporte de catálogo: {ex.Message}" });
            }
        }

        [HttpGet("ventas-empleado")]
        public async Task<ActionResult> VentasPorEmpleado(
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            [FromQuery] Guid? idEmpleado)
        {
            try
            {
                var filtro = new ReporteVentasFiltro
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    IdEmpleado = idEmpleado
                };

                var lineas = await _reporteFlujo.VentasPorEmpleado(filtro);
                return Ok(lineas);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al generar el reporte de ventas por empleado");
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("categorias")]
        public async Task<ActionResult> Categorias()
        {
            try
            {
                var categorias = await _reporteFlujo.Categorias();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorías");
                return BadRequest(new { mensaje = $"No fue posible obtener las categorías: {ex.Message}" });
            }
        }
    }
}
