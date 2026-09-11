using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DA
{
    public class ConfiguracionLealtadDA : IConfiguracionLealtadDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public ConfiguracionLealtadDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<ConfiguracionLealtadResponse> Obtener()
        {
            var resultado = await _sqlConnection.QueryFirstOrDefaultAsync<ConfiguracionLealtadResponse>(
                "ObtenerConfiguracionLealtad",
                commandType: CommandType.StoredProcedure);

            return resultado ?? new ConfiguracionLealtadResponse { PuntosPorMonto = 0 };
        }
    }
}