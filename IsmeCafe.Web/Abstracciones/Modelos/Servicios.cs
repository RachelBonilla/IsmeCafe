using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos;

public class ServicioBase
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    [DefaultValue("Cata de café tradicional")]
    [Description("Nombre del servicio")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    [DefaultValue("Experiencia guiada de cata de café con notas aromáticas y sensoriales.")]
    [Description("Descripción del servicio")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La duración es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor que cero")]
    [DefaultValue(60)]
    [Description("Duración del servicio en minutos")]
    public int Duracion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, 9999999, ErrorMessage = "El precio debe ser un valor numérico mayor que cero")]
    [DefaultValue(15000)]
    [Description("Precio del servicio")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El cupo máximo es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser un valor numérico positivo")]
    [DefaultValue(20)]
    [Description("Número máximo de personas permitidas en el servicio")]
    public int CupoMaximo { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio")]
    [DefaultValue(true)]
    [Description("1 = activo (visible en el catálogo), 0 = inactivo")]
    public bool Activo { get; set; }
}

public class ServicioRequest : ServicioBase
{
    [Required(ErrorMessage = "El grupo es obligatorio")]
    [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$",
        ErrorMessage = "IdGrupo debe ser un GUID válido")]
    [DefaultValue("B0000000-0000-0000-0000-000000000001")]
    [Description("Identificador del grupo de servicios (ver tabla Grupos)")]
    public Guid IdGrupo { get; set; }
}

public class ServicioResponse : ServicioBase
{
    [Required]
    [DefaultValue("S0000001-0000-0000-0000-000000000001")]
    [Description("Identificador del servicio")]
    public Guid Id { get; set; }

    [StringLength(100)]
    [DefaultValue("Tours de café")]
    [Description("Nombre del grupo de servicios")]
    public string Grupo { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    [Description("Fecha de creación del servicio")]
    public DateTime FechaCreacion { get; set; }

    [DataType(DataType.DateTime)]
    [Description("Fecha de la última actualización del servicio")]
    public DateTime? FechaActualizacion { get; set; }
}

public class ServicioDetalle : ServicioResponse
{
    public Guid IdGrupo { get; set; }
}