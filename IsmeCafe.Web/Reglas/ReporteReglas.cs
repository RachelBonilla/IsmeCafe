using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Reglas
{
    public class ReporteReglas : IReporteReglas
    {
        private readonly IReporteServicio _reporteServicio;

        public ReporteReglas(IReporteServicio reporteServicio)
        {
            _reporteServicio = reporteServicio;
        }

        public Task<IEnumerable<ReporteInventarioLinea>> Inventario(Guid? idCategoria) =>
            _reporteServicio.Inventario(idCategoria);

        public Task<IEnumerable<ReporteCatalogoLinea>> Catalogo(bool? activo) =>
            _reporteServicio.Catalogo(activo);

        // HU-33 CA3: se valida antes de llamar al API para dar el mensaje de inmediato.
        public Task<IEnumerable<ReporteVentasLinea>> VentasPorEmpleado(ReporteVentasFiltro filtro)
        {
            if (filtro.FechaInicio.Date > filtro.FechaFin.Date)
                throw new Exception("El rango de fechas no es válido: la fecha de inicio es posterior a la fecha de fin.");

            return _reporteServicio.VentasPorEmpleado(filtro);
        }

        public Task<IEnumerable<CategoriaResponse>> Categorias() => _reporteServicio.Categorias();
    }
}
