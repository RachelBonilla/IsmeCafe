using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IReporteReglas
    {
        Task<IEnumerable<ReporteInventarioLinea>> Inventario(Guid? idCategoria);
        Task<IEnumerable<ReporteCatalogoLinea>> Catalogo(bool? activo);
        Task<IEnumerable<ReporteVentasLinea>> VentasPorEmpleado(ReporteVentasFiltro filtro);
        Task<IEnumerable<CategoriaResponse>> Categorias();
    }
}
