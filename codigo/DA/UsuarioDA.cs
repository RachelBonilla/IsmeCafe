using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class UsuarioDA : IUsuarioDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public UsuarioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(UsuarioRequest usuario)
        {
            string query = @"RegistrarUsuario";

            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Contrasena = usuario.Contrasena,
                IdRol = usuario.IdRol
            });

            return resultadoConsulta;
        }

        public async Task<Guid> AgregarCliente(UsuarioClienteRequest usuario)
        {
            string query = @"RegistrarUsuarioCliente";

            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Contrasena = usuario.Contrasena
            });

            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid Id, UsuarioEditarRequest usuario)
        {
            await VerificarUsuarioExiste(Id);
            string query = @"EditarUsuario";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Id,
                Nombre = usuario.Nombre,
                Apellidos = usuario.Apellidos,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                IdRol = usuario.IdRol,
                Activo = usuario.Activo
            });

            return resultado;
        }

        public async Task<Guid> Desactivar(Guid Id)
        {
            await VerificarUsuarioExiste(Id);
            string query = @"DesactivarUsuario";
            await _sqlConnection.ExecuteScalarAsync<Guid>(query, new { Id = Id });

            return Id;
        }

        public async Task<IEnumerable<UsuarioDetalle>> Obtener()
        {
            string query = @"ObtenerUsuarios";
            var resultadoConsulta = await _sqlConnection.QueryAsync<UsuarioDetalle>(query);

            return resultadoConsulta;
        }

        public async Task<UsuarioDetalle> Obtener(Guid Id)
        {
            string query = @"ObtenerUsuarioPorId";
            var resultadoConsulta = await _sqlConnection.QueryFirstOrDefaultAsync<UsuarioDetalle>(query, new
            {
                Id = Id
            });

            return resultadoConsulta;
        }

        private async Task VerificarUsuarioExiste(Guid Id)
        {
            UsuarioDetalle? resultadoConsultaUsuario = await Obtener(Id);

            if (resultadoConsultaUsuario == null)
            {
                throw new Exception("No se encontró el usuario");
            }
        }

    }
}
