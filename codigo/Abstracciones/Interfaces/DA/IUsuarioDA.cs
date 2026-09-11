using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IUsuarioDA
    {
        Task<IEnumerable<UsuarioDetalle>> Obtener();
        Task<UsuarioDetalle> Obtener(Guid Id);
        Task<Guid> Agregar(UsuarioRequest usuario);
        Task<Guid> AgregarCliente(UsuarioClienteRequest usuario);
        Task<Guid> Editar(Guid Id, UsuarioEditarRequest usuario);
        Task<Guid> Desactivar(Guid Id);
    }
}