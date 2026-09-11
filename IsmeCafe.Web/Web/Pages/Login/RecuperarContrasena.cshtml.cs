using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Login
{
    public class RecuperarContrasenaModel : PageModel
    {
        private readonly ILoginReglas _loginReglas;

        public RecuperarContrasenaModel(ILoginReglas loginReglas)
        {
            _loginReglas = loginReglas;
        }

        [BindProperty]
        public RecuperarContrasenaViewModel Formulario { get; set; } = new();

        [BindProperty]
        public bool CodigoEnviado { get; set; }
        public string? Mensaje { get; set; }
        public string? Error { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostSolicitarCodigoAsync()
        {
            if (string.IsNullOrWhiteSpace(Formulario.Correo))
            {
                Error = "Debe ingresar un correo electrónico.";
                return Page();
            }

            var request = new SolicitarRecuperacionRequest
            {
                Correo = Formulario.Correo.Trim()
            };

            var resultado = await _loginReglas.SolicitarRecuperacion(request);

            if (!resultado)
            {
                Error = "No fue posible enviar el código de recuperación.";
                return Page();
            }

            CodigoEnviado = true;
            Error = null;

            Mensaje = "Si el correo está registrado, recibirá un código de recuperación.";

            return Page();
        }

        public async Task<IActionResult> OnPostRestablecerAsync()
        {
            // Mantener visible el formulario del OTP
            // si ocurre algún error.
            CodigoEnviado = true;

            if (string.IsNullOrWhiteSpace(Formulario.Correo))
            {
                Error = "Debe ingresar el correo electrónico.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Formulario.Codigo))
            {
                Error = "Debe ingresar el código de recuperación.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Formulario.NuevaContrasena))
            {
                Error = "Debe ingresar la nueva contraseña.";
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Formulario.ConfirmarContrasena))
            {
                Error = "Debe confirmar la nueva contraseña.";
                return Page();
            }

            if (Formulario.NuevaContrasena != Formulario.ConfirmarContrasena)
            {
                Error = "Las contraseñas no coinciden.";
                return Page();
            }

            var request = new RestablecerContrasenaRequest
            {
                Correo = Formulario.Correo.Trim(),
                Codigo = Formulario.Codigo.Trim(),
                NuevaContrasena = Formulario.NuevaContrasena
            };

            var resultado = await _loginReglas.RestablecerContrasena(request);

            if (!resultado)
            {
                Error = "No fue posible restablecer la contraseña.";
                return Page();
            }

            TempData["MensajeExito"] = "La contraseña fue restablecida correctamente.";

            return RedirectToPage("/Login/Index");
        }
    }
}