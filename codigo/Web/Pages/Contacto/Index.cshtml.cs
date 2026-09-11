using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Contacto
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ContactoInput Input { get; set; } = new();

        public bool EnviadoConExito { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Aquí se integraría el envío real (correo, API o base de datos).
            // Por ahora confirmamos la recepción del mensaje al usuario.
            EnviadoConExito = true;

            ModelState.Clear();
            Input = new ContactoInput();

            return Page();
        }

        public class ContactoInput
        {
            [Required(ErrorMessage = "El nombre es obligatorio.")]
            [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
            [Display(Name = "Nombre completo")]
            public string Nombre { get; set; } = string.Empty;

            [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
            [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
            [Display(Name = "Correo electrónico")]
            public string Correo { get; set; } = string.Empty;

            [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
            [Display(Name = "Teléfono")]
            public string? Telefono { get; set; }

            [Required(ErrorMessage = "El asunto es obligatorio.")]
            [StringLength(150, ErrorMessage = "El asunto no puede superar los 150 caracteres.")]
            [Display(Name = "Asunto")]
            public string Asunto { get; set; } = string.Empty;

            [Required(ErrorMessage = "El mensaje es obligatorio.")]
            [StringLength(1000, MinimumLength = 10, ErrorMessage = "El mensaje debe tener entre 10 y 1000 caracteres.")]
            [Display(Name = "Mensaje")]
            public string Mensaje { get; set; } = string.Empty;
        }
    }
}
