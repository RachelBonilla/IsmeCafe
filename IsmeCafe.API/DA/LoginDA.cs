using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos.Auth;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class LoginDA : ILoginDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public LoginDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<UsuarioAutenticacion> ObtenerUsuarioPorCorreo(string correo)
        {
            string query = @"ObtenerUsuarioPorCorreo";

            var resultadoConsulta = await _sqlConnection.QueryFirstOrDefaultAsync<UsuarioAutenticacion>(query,new 
            { 
                Correo = correo 
            });

            return resultadoConsulta;
        }

        public async Task ActualizarContrasena(Guid idUsuario,string contrasenaHash)
        {
            string query = @"ActualizarContrasena";

            await _sqlConnection.ExecuteAsync(query,new
            {
                Id = idUsuario,
                Contrasena = contrasenaHash
            });
        }
    }
}