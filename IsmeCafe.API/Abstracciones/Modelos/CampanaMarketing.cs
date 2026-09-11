using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class CampanaOfertaRequest
    {
        [Required(ErrorMessage = "La oferta es obligatoria")]
        public Guid IdOferta { get; set; }

        [Required(ErrorMessage = "El asunto es obligatorio")]
        [DefaultValue("¡Nueva oferta en Isme Café!")]
        public string Asunto { get; set; } = string.Empty;

        [StringLength(4000)]
        public string? Contenido { get; set; }
    }

    public class CampanaDescuentoRequest
    {
        [Required(ErrorMessage = "El descuento es obligatorio")]
        public Guid IdDescuento { get; set; }

        [Required(ErrorMessage = "El asunto es obligatorio")]
        public string Asunto { get; set; } = string.Empty;

        [StringLength(4000)]
        public string? Contenido { get; set; }
    }

    public class CampanaResponse
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public Guid? IdOferta { get; set; }
        public Guid? IdDescuento { get; set; }
        public string Asunto { get; set; } = string.Empty;
        public DateTime? FechaEnvio { get; set; }
        public int CantidadDestinatarios { get; set; }
        public bool Exitosa { get; set; }
    }
}