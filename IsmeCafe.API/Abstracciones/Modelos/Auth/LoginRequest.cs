using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un formato de correo válido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; } = string.Empty;

        public bool EsEmpleado { get; set; }
    }
}
