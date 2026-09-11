using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class DescuentoBase
    {
        [Required(ErrorMessage = "El porcentaje de descuento es obligatorio")]
        [Range(0.01, 100, ErrorMessage = "El porcentaje debe ser mayor a 0 y menor o igual a 100")]
        public decimal PorcentajeDescuento { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.DateTime)]

        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public bool Activo { get; set; }
    }

    public class DescuentoRequest : DescuentoBase
    {
        [Required(ErrorMessage = "El producto es obligatorio")]
        public Guid IdProducto { get; set; }
    }

    public class DescuentoResponse : DescuentoBase
    {
        [Required]
        public Guid Id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        public string NombreProducto { get; set; } = string.Empty;
        [Required(ErrorMessage = "El precio del producto es obligatorio")]
        public decimal PrecioProducto { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaActualizacion { get; set; }
    }

    public class DescuentoDetalle : DescuentoResponse
    {
        public Guid IdProducto { get; set; }
    }
}
