using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DA
{

    public class ReporteDA : IReporteDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public ReporteDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<IEnumerable<ReporteInventarioLinea>> Inventario(Guid? idCategoria)
        {
            return await _sqlConnection.QueryAsync<ReporteInventarioLinea>(
                "ReporteInventario",
                new { IdCategoria = idCategoria },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteCatalogoLinea>> Catalogo(bool? activo)
        {
            return await _sqlConnection.QueryAsync<ReporteCatalogoLinea>(
                "ReporteCatalogo",
                new { Activo = activo },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ReporteVentasLinea>> VentasPorEmpleado(
            DateTime fechaInicio, DateTime fechaFin, Guid? idEmpleado)
        {
            return await _sqlConnection.QueryAsync<ReporteVentasLinea>(
                "ReporteVentasPorEmpleado",
                new
                {
                    FechaInicio = fechaInicio.Date,
                    FechaFin = fechaFin.Date,
                    IdEmpleado = idEmpleado
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CategoriaResponse>> Categorias()
        {
            return await _sqlConnection.QueryAsync<CategoriaResponse>(
                "ObtenerCategorias",
                commandType: CommandType.StoredProcedure);
        }
    }
}
