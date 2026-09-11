using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IPedidoController
    {
        Task<ActionResult> Confirmar(PedidoRequest request);
        Task<ActionResult> Obtener(Guid id);
        Task<ActionResult> ObtenerPorUsuario(Guid idUsuario);
    }
}
