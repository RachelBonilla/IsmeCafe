using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    /// <summary>
    /// Campos comunes de un producto del catálogo del café (M1 - Productos).
    /// Historias de usuario HU-01 a HU-06.
    /// </summary>
    public class ProductoBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [DefaultValue("Café de altura 250g")]
        [Description("Nombre del producto")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        [DefaultValue("Café molido de tueste medio, notas a chocolate y cítricos.")]
        [Description("Descripción del producto")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 9999999, ErrorMessage = "El precio debe ser un valor numérico mayor que cero")]
        [DefaultValue(4500)]
        [Description("Precio de venta del producto")]
        public decimal Precio { get; set; }

        [StringLength(300, ErrorMessage = "La URL de la imagen no puede exceder 300 caracteres")]
        [DefaultValue("/img/productos/default.png")]
        [Description("Ruta o URL de la imagen. Si se omite, se asigna una imagen por defecto.")]
        public string Imagen { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad disponible es obligatoria")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible debe ser un valor numérico positivo")]
        [DefaultValue(20)]
        [Description("Cantidad disponible en inventario (stock)")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [DefaultValue(true)]
        [Description("1 = activo (visible en el catálogo), 0 = inactivo")]
        public bool Activo { get; set; }
    }

    public class ProductoRequest : ProductoBase
    {
        [Required(ErrorMessage = "La categoría es obligatoria")]
        [RegularExpression(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$", ErrorMessage = "IdCategoria debe ser un GUID válido")]
        [DefaultValue("B0000000-0000-0000-0000-000000000001")]
        [Description("Identificador de la categoría (ver tabla Categorias)")]
        public Guid IdCategoria { get; set; }
    }

    public class ProductoResponse : ProductoBase
    {
        [Required]
        [DefaultValue("C0000001-0000-0000-0000-000000000001")]
        [Description("Identificador del producto")]
        public Guid Id { get; set; }

        [StringLength(50)]
        [DefaultValue("Café en grano")]
        [Description("Nombre de la categoría")]
        public string Categoria { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Description("Fecha de creación del producto")]
        public DateTime FechaCreacion { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Fecha de la última actualización del producto")]
        public DateTime? FechaActualizacion { get; set; }

        [Description("Porcentaje de descuento vigente (null si no tiene descuento activo)")]
        public decimal? PorcentajeDescuento { get; set; }

        [Description("Precio con descuento aplicado (null si no tiene descuento activo)")]
        public decimal? PrecioConDescuento { get; set; }
    }
    public class ProductoDetalle : ProductoResponse
    {
        public Guid IdCategoria { get; set; }
    }
}
