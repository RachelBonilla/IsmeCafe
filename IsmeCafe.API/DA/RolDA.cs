using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class RolDA : IRolDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public RolDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<IEnumerable<Rol>> Obtener()
        {
            string query = @"ObtenerRoles";

            var resultadoConsulta = await _sqlConnection.QueryAsync<Rol>(query);

            return resultadoConsulta;
        }
    }
}
