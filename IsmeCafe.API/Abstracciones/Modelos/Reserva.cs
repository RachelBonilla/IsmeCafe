using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos;

public class ReservaBase
{
    [Required(ErrorMessage = "El servicio es obligatorio")]
    [DefaultValue("S0000001-0000-0000-0000-000000000001")]
    [Description("Identificador del servicio que se desea reservar")]
    public Guid IdServicio { get; set; }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    [DefaultValue("Juan Pérez")]
    [Description("Nombre completo del cliente")]
    public string NombreCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
    [StringLength(150, ErrorMessage = "El correo no puede exceder 150 caracteres")]
    [DefaultValue("cliente@email.com")]
    [Description("Correo electrónico del cliente")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    [DefaultValue("8888-8888")]
    [Description("Número de teléfono del cliente")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de reserva es obligatoria")]
    [DataType(DataType.Date)]
    [Description("Fecha en que se realizará la reserva")]
    public DateTime FechaReserva { get; set; }

    [Required(ErrorMessage = "La hora de reserva es obligatoria")]
    [Description("Hora en que se realizará la reserva")]
    public TimeSpan HoraReserva { get; set; }

    [Required(ErrorMessage = "La cantidad de personas es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de personas debe ser mayor que cero")]
    [DefaultValue(1)]
    [Description("Cantidad de personas para la reserva")]
    public int CantidadPersonas { get; set; }
}

public class ReservaRequest : ReservaBase
{
}

public class ReservaResponse : ReservaBase
{
    [Required]
    [DefaultValue("C0000001-0000-0000-0000-000000000001")]
    [Description("Identificador de la reserva")]
    public Guid Id { get; set; }

    [Description("Nombre del servicio reservado")]
    public string Servicio { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    [Description("Fecha de creación de la reserva")]
    public DateTime FechaCreacion { get; set; }
}