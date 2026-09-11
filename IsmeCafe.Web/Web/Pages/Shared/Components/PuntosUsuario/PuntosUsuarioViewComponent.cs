using Abstracciones.Interfaces.Reglas;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Pages.Shared.Components.PuntosUsuario
{
    public class PuntosUsuarioViewComponent : ViewComponent
    {
        private readonly IUsuarioReglas _usuarioReglas;

        public PuntosUsuarioViewComponent(IUsuarioReglas usuarioReglas)
        {
            _usuarioReglas = usuarioReglas;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var idTexto = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idTexto) || !Guid.TryParse(idTexto, out var idUsuario))
            {
                return Content(string.Empty);
            }

            try
            {
                var usuario = await _usuarioReglas.Obtener(idUsuario);
                return View(usuario);
            }
            catch
            {
                return Content(string.Empty);
            }
        }
    }
}