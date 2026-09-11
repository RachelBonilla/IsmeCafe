using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class UsuarioBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [DefaultValue("Carlos")]
        [Description("Nombre del usuario")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres")]
        [DefaultValue("Sainz Jr")]
        [Description("Apellidos del usuario")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(100, ErrorMessage = "El correo no puede exceder los 100 caracteres")]
        [DefaultValue("csainsjr@gmail.com")]
        [Description("Correo electrónico único para el ingreso al sistema")]
        public string Correo { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        [DefaultValue("8888-8888")]
        [Description("Número de teléfono de contacto")]
        public string Telefono { get; set; } = string.Empty;

        [DefaultValue(true)]
        public bool Activo { get; set; } = true;

        public string NombreRol { get; set; } = string.Empty;
    }

    public class UsuarioRequest : UsuarioBase
    {
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio")]
        public Guid IdRol { get; set; }
    }

    public class UsuarioResponse : UsuarioBase
    {
        [Required]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaActualizacion { get; set; }
    }

    public class UsuarioDetalle : UsuarioResponse
    {
        [Required]
        public Guid IdRol { get; set; }

        [Required]
        public string NombreRol { get; set; } = string.Empty;
        public int Puntos { get; set; }
    }

    public class UsuarioEditarRequest : UsuarioBase
    {

        [Required(ErrorMessage = "El rol es obligatorio")]
        public Guid IdRol { get; set; }
    }
    public class UsuarioClienteRequest : UsuarioBase
    {
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;
        public bool SuscritoMarketing { get; set; }

    }
}
