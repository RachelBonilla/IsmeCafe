using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IUsuarioController
    {
        Task<ActionResult> Obtener();
        Task<ActionResult> Obtener(Guid Id);
        Task<ActionResult> Agregar(UsuarioRequest usuario);
        Task<ActionResult> AgregarCliente(UsuarioClienteRequest usuario);
        Task<ActionResult> Editar(Guid Id, UsuarioEditarRequest usuario);
        Task<ActionResult> Desactivar(Guid Id);
    }
}