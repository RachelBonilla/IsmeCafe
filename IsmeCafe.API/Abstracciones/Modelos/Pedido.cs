using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class PedidoRequest
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public Guid IdUsuario { get; set; }

        public bool UsarPuntos { get; set; }
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
        public int IdEstado { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int PuntosGanados { get; set; }
        public int PuntosUsados { get; set; }
        public decimal DescuentoPuntos { get; set; }
        public IEnumerable<PedidoDetalleResponse> Detalle { get; set; } = new List<PedidoDetalleResponse>();
    }

    public class PedidoEstadoResponse
    {
        public int IdEstado { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class ActualizarPedidoEstadoRequest
    {
        [Required(ErrorMessage = "El estado es obligatorio")]
        public int IdEstado { get; set; }
    }
}
