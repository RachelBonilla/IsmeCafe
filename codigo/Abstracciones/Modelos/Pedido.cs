using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    /// <summary>
    /// M5 - Pedidos. Confirmación y consulta de pedidos (HU-23).
    /// </summary>
    public class PedidoRequest
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Description("Identificador del cliente que confirma el pedido")]
        public Guid IdUsuario { get; set; }
    }

    public class PedidoDetalleResponse
    {
        public Guid IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class PedidoResponse
    {
        public Guid Id { get; set; }
        public int NumeroPedido { get; set; }
        public Guid IdUsuario { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public decimal Total { get; set; }
        public IEnumerable<PedidoDetalleResponse> Detalle { get; set; } = new List<PedidoDetalleResponse>();
    }
}
