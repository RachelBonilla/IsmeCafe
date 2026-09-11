using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Reglas
{
    public class UsuarioReglas : IUsuarioReglas
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public UsuarioReglas(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public Task<bool> Agregar(UsuarioRequest usuario) => _usuarioServicio.Agregar(usuario);
        public Task<bool> AgregarCliente(UsuarioClienteRequest usuario) => _usuarioServicio.AgregarCliente(usuario);

        public Task<bool> Desactivar(Guid id) => _usuarioServicio.Desactivar(id);

        public Task<bool> Editar(Guid id, UsuarioEditarRequest usuario) => _usuarioServicio.Editar(id, usuario);

        public Task<IEnumerable<UsuarioDetalle>> Obtener() => _usuarioServicio.Obtener();

        public Task<UsuarioDetalle> Obtener(Guid id) => _usuarioServicio.Obtener(id);
    }
}