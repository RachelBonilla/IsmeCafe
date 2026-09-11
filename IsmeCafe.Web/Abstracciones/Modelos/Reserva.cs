using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ReservaBase
    {
        [Required(ErrorMessage = "El servicio es obligatorio")]
        public Guid IdServicio { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaReserva { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        public TimeSpan HoraReserva { get; set; }

        [Required(ErrorMessage = "La cantidad de personas es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe reservar al menos para una persona")]
        public int CantidadPersonas { get; set; }
    }

    public class ReservaRequest : ReservaBase
    {
    }

    public class ReservaResponse : ReservaBase
    {
        public Guid Id { get; set; }

        public string Servicio { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }
    }
}