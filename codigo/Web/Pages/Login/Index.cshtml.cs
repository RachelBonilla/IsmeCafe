using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Abstracciones.Constantes;

namespace Web.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly ILoginReglas _loginReglas;

        public IndexModel(ILoginReglas loginReglas)
        {
            _loginReglas = loginReglas;
        }

        [BindProperty]
        public LoginRequest Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var resultado = await _loginReglas.IniciarSesion(Input);

            if (resultado != null)
            {
                // Claims para la sesión (Seguridad).
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, resultado.Id.ToString()),
                    new Claim(ClaimTypes.Name, Input.Correo),
                    new Claim(ClaimTypes.Role, resultado.NombreRol ?? Roles.Cliente)
                };

                var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identidad);

                // Enviamos la cookie de autenticación al navegador.
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Al ser exitosa (Respuesta de la API) se completa la autenticación.
                if (resultado.NombreRol == Roles.Empleado || resultado.NombreRol == Roles.Administrador)
                {
                    return RedirectToPage("/Admin/Index");
                }

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
            return Page();
        }
    }
}