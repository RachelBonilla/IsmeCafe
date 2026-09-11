using Abstracciones.Modelos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Web.Documentos
{

    public static class FacturaPdfService
    {
        public static byte[] Generar(PedidoResponse pedido)
        {
            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(c => ComponerEncabezado(c, pedido));
                    page.Content().Element(c => ComponerContenido(c, pedido));
                    page.Footer().AlignCenter().Text("Isme Café — Gracias por su compra").FontSize(9);
                });
            });

            return documento.GeneratePdf();
        }

        private static void ComponerEncabezado(IContainer container, PedidoResponse pedido)
        {
            container.PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Isme Café").FontSize(18).Bold();
                    column.Item().Text("Factura de compra").FontSize(11);
                });

                row.ConstantItem(200).Column(column =>
                {
                    column.Item().AlignRight().Text($"Pedido #{pedido.NumeroPedido}").Bold();
                    column.Item().AlignRight().Text($"Fecha: {pedido.Fecha:dd/MM/yyyy HH:mm}");
                    column.Item().AlignRight().Text($"Estado: {pedido.Estado}");
                });
            });
        }

        private static void ComponerContenido(IContainer container, PedidoResponse pedido)
        {
            container.PaddingVertical(15).Column(column =>
            {
                column.Spacing(8);

                column.Item().Text($"Cliente: {pedido.Cliente}");

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Producto").Bold();
                        header.Cell().AlignCenter().Text("Cant.").Bold();
                        header.Cell().AlignRight().Text("Precio unit.").Bold();
                        header.Cell().AlignRight().Text("Subtotal").Bold();

                        header.Cell().ColumnSpan(4).PaddingTop(4).BorderBottom(1)
                            .BorderColor(Colors.Grey.Medium);
                    });

                    foreach (var item in pedido.Detalle)
                    {
                        table.Cell().Text(item.Nombre);
                        table.Cell().AlignCenter().Text(item.Cantidad.ToString());
                        table.Cell().AlignRight().Text(item.PrecioUnitario.ToString("C"));
                        table.Cell().AlignRight().Text(item.Subtotal.ToString("C"));
                    }
                });

                var subtotal = pedido.Total + pedido.DescuentoPuntos;

                column.Item().AlignRight().Column(totales =>
                {
                    totales.Item().Text($"Subtotal: {subtotal:C}");

                    if (pedido.DescuentoPuntos > 0)
                        totales.Item().Text($"Descuento por {pedido.PuntosUsados} puntos canjeados: -{pedido.DescuentoPuntos:C}");

                    totales.Item().PaddingTop(4).Text($"Total pagado: {pedido.Total:C}").FontSize(13).Bold();

                    if (pedido.PuntosGanados > 0)
                        totales.Item().Text($"Puntos ganados con esta compra: +{pedido.PuntosGanados}");
                });
            });
        }
    }
}