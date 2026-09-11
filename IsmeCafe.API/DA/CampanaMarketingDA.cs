using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DA
{
    public class CampanaMarketingDA : ICampanaMarketingDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public CampanaMarketingDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Registrar(string tipo, Guid? idOferta, Guid? idDescuento,
            string asunto, string contenido, int cantidadDestinatarios, bool exitosa)
        {
            string query = "RegistrarCampana";
            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                Tipo = tipo,
                IdOferta = idOferta,
                IdDescuento = idDescuento,
                Asunto = asunto,
                Contenido = contenido,
                FechaEnvio = DateTime.Now,
                CantidadDestinatarios = cantidadDestinatarios,
                Exitosa = exitosa
            });
            return resultado;
        }

        public async Task<IEnumerable<CampanaResponse>> Obtener()
        {
            string query = "ObtenerCampanas";
            return await _sqlConnection.QueryAsync<CampanaResponse>(query);
        }
    }
}
