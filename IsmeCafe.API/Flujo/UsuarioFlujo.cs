using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using BCrypt.Net;

namespace Flujo
{
    public class UsuarioFlujo : IUsuarioFlujo
    {
        private readonly IUsuarioDA _usuarioDA;

        public UsuarioFlujo(IUsuarioDA usuarioDA)
        {
            _usuarioDA = usuarioDA;
        }

        public async Task<Guid> Agregar(UsuarioRequest usuario)
        {
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

            var nuevoId = await _usuarioDA.Agregar(usuario);

            return nuevoId;
        }

        public async Task<Guid> AgregarCliente(UsuarioClienteRequest usuario)
        {
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasena);

            var nuevoId = await _usuarioDA.AgregarCliente(usuario);

            return nuevoId;
        }

        public async Task<Guid> Editar(Guid Id, UsuarioEditarRequest usuario)
        {
            return await _usuarioDA.Editar(Id, usuario);
        }

        public async Task<Guid> Desactivar(Guid Id)
        {
            return await _usuarioDA.Desactivar(Id);
        }

        public async Task<IEnumerable<UsuarioDetalle>> Obtener()
        {
            return await _usuarioDA.Obtener();
        }

        public async Task<UsuarioDetalle> Obtener(Guid Id)
        {
            return await _usuarioDA.Obtener(Id);
        }

    }
}