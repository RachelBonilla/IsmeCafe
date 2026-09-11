using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    /// <summary>
    /// M5 - Pedidos. Ítems del carrito de compras (HU-21, HU-22).
    /// </summary>
    public class CarritoItemRequest
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Description("Identificador del cliente dueño del carrito")]
        public Guid IdUsuario { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Description("Identificador del producto que se agrega o modifica")]
        public Guid IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un entero positivo")]
        [DefaultValue(1)]
        [Description("Cantidad de unidades del producto")]
        public int Cantidad { get; set; }
    }

    public class CarritoItemResponse
    {
        public Guid IdItem { get; set; }
        public Guid IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Imagen { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public int StockDisponible { get; set; }
        public bool Activo { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class CarritoResponse
    {
        public Guid IdUsuario { get; set; }
        public IEnumerable<CarritoItemResponse> Items { get; set; } = new List<CarritoItemResponse>();
        public decimal Total { get; set; }
        public int CantidadItems { get; set; }
    }
}
