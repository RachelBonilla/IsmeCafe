using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class OfertaBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de oferta es obligatorio")]
        public string TipoOferta { get; set; } = string.Empty;

     
        public decimal? PrecioCombo { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [DefaultValue(true)]
        public bool Activo { get; set; }
    }

    public class OfertaProductoRequest
    {
        [Required(ErrorMessage = "El producto es obligatorio")]
        public Guid IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 100, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }
    }

    public class OfertaProductoResponse
    {
        public Guid Id { get; set; }
        public Guid IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal PrecioProducto { get; set; }
        public int Cantidad { get; set; }
    }

    public class OfertaRequest : OfertaBase
    {
        [Required(ErrorMessage = "Debe incluir al menos un producto en la oferta")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto en la oferta")]
        [Description("Productos incluidos en la oferta o combo")]
        public List<OfertaProductoRequest> Productos { get; set; } = new();
    }

    public class OfertaResponse : OfertaBase
    {
        [Required]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaActualizacion { get; set; }
    }

    public class OfertaDetalle : OfertaResponse
    {
        public List<OfertaProductoResponse> Productos { get; set; } = new();
    }
}


