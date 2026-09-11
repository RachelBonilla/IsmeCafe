using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Servicios
{
    public interface IReporteServicio
    {
        Task<IEnumerable<ReporteInventarioLinea>> Inventario(Guid? idCategoria);
        Task<IEnumerable<ReporteCatalogoLinea>> Catalogo(bool? activo);
        Task<IEnumerable<ReporteVentasLinea>> VentasPorEmpleado(ReporteVentasFiltro filtro);
        Task<IEnumerable<CategoriaResponse>> Categorias();
    }
}
