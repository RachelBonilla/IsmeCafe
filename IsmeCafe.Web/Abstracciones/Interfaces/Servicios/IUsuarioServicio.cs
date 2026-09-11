using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Servicios
{
    public interface IUsuarioServicio
    {
        Task<IEnumerable<UsuarioDetalle>> Obtener();
        Task<UsuarioDetalle> Obtener(Guid id);
        Task<bool> Agregar(UsuarioRequest usuario);
        Task<bool> AgregarCliente(UsuarioClienteRequest usuario);
        Task<bool> Editar(Guid id, UsuarioEditarRequest usuario);
        Task<bool> Desactivar(Guid id);
    }
}