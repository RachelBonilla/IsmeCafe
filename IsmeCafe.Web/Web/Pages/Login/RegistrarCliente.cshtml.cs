using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Login
{
    public class RegistrarClienteModel : PageModel
    {
        private readonly IUsuarioReglas _usuarioReglas;

        public RegistrarClienteModel(IUsuarioReglas usuarioReglas)
        {
            _usuarioReglas = usuarioReglas;
        }

        [BindProperty]
        public UsuarioClienteRequest Input { get; set; } = new();

        public string? Mensaje { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var exito = await _usuarioReglas.AgregarCliente(Input);

            if (!exito)
            {
                Mensaje = "No fue posible completar el registro. Verificá que el correo no esté ya registrado.";
                return Page();
            }

            return RedirectToPage("/Login/Index");
        }
    }
}