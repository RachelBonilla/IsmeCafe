using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    // ---------------------------------------------------------------------
    // Reporte de inventario
    // ---------------------------------------------------------------------
    public class ReporteInventarioLinea
    {
        [Description("Identificador del producto")]
        public Guid Id { get; set; }

        [Description("Nombre del producto")]
        public string Nombre { get; set; } = string.Empty;

        [Description("Categoría a la que pertenece el producto")]
        public string Categoria { get; set; } = string.Empty;

        [Description("Identificador de la categoría")]
        public Guid? IdCategoria { get; set; }

        [Description("Existencias actuales en inventario")]
        public int StockActual { get; set; }

        [Description("Umbral a partir del cual se considera existencia baja")]
        public int StockMinimo { get; set; }

        [Description("Precio de venta")]
        public decimal Precio { get; set; }

        [Description("1 = activo en el catálogo, 0 = inactivo")]
        public bool Activo { get; set; }

        [Description("Indica si el producto está en o por debajo del mínimo")]
        public bool StockBajo { get; set; }

        [Description("Agotado, Bajo o Disponible")]
        public string EstadoExistencias { get; set; } = string.Empty;
    }

    // ---------------------------------------------------------------------
    // Reporte de catálogo
    // ---------------------------------------------------------------------
    public class ReporteCatalogoLinea
    {
        [Description("Identificador del producto o servicio")]
        public Guid Id { get; set; }

        [Description("Producto o Servicio")]
        public string Tipo { get; set; } = string.Empty;

        [Description("Nombre del elemento del catálogo")]
        public string Nombre { get; set; } = string.Empty;

        [Description("Categoría del producto. Los servicios se reportan como 'Servicio'.")]
        public string Categoria { get; set; } = string.Empty;

        [Description("Precio de venta")]
        public decimal Precio { get; set; }

        [Description("1 = activo, 0 = inactivo")]
        public bool Activo { get; set; }

        [Description("Existencias del producto o cupo máximo del servicio")]
        public int Existencias { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Fecha de creación del registro")]
        public DateTime? FechaCreacion { get; set; }
    }

    // ---------------------------------------------------------------------
    // Reporte de ventas por empleado
    // ---------------------------------------------------------------------
    public class ReporteVentasFiltro
    {
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        [Description("Inicio del rango de fechas (inclusive)")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.Date)]
        [Description("Fin del rango de fechas (inclusive)")]
        public DateTime FechaFin { get; set; }

        [Description("Identificador del empleado. Si se omite, incluye a todos.")]
        public Guid? IdEmpleado { get; set; }
    }

    public class ReporteVentasLinea
    {
        [Description("Identificador del empleado. NULL si el pedido fue de autoservicio.")]
        public Guid? IdEmpleado { get; set; }

        [Description("Nombre completo del empleado")]
        public string Empleado { get; set; } = string.Empty;

        [Description("Correo del empleado")]
        public string Correo { get; set; } = string.Empty;

        [Description("Cantidad de órdenes atendidas en el periodo")]
        public int CantidadOrdenes { get; set; }

        [Description("Monto acumulado de las ventas del periodo")]
        public decimal MontoTotal { get; set; }

        [Description("Monto promedio por orden")]
        public decimal TicketPromedio { get; set; }

        [Description("Unidades de producto vendidas en el periodo")]
        public int UnidadesVendidas { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? PrimeraVenta { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UltimaVenta { get; set; }
    }

    // ---------------------------------------------------------------------
    // Catálogo auxiliar
    // ---------------------------------------------------------------------
    public class CategoriaResponse
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
