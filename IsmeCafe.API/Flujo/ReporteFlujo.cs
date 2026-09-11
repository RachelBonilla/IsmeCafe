using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{

    public class ReporteFlujo : IReporteFlujo
    {
        private readonly IReporteDA _reporteDA;

        public ReporteFlujo(IReporteDA reporteDA)
        {
            _reporteDA = reporteDA;
        }

        public Task<IEnumerable<ReporteInventarioLinea>> Inventario(Guid? idCategoria) =>
            _reporteDA.Inventario(idCategoria);

        public Task<IEnumerable<ReporteCatalogoLinea>> Catalogo(bool? activo) =>
            _reporteDA.Catalogo(activo);

       public Task<IEnumerable<ReporteVentasLinea>> VentasPorEmpleado(ReporteVentasFiltro filtro)
        {
            if (filtro.FechaInicio.Date > filtro.FechaFin.Date)
                throw new Exception("El rango de fechas no es válido: la fecha de inicio es posterior a la fecha de fin.");

            return _reporteDA.VentasPorEmpleado(filtro.FechaInicio, filtro.FechaFin, filtro.IdEmpleado);
        }

        public Task<IEnumerable<CategoriaResponse>> Categorias() => _reporteDA.Categorias();
    }
}
