
-- 5) ObtenerPedido / ObtenerPedidosPorUsuario también traen el detalle de puntos
CREATE   PROCEDURE [dbo].[ObtenerPedido]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pe.[Id], pe.[NumeroPedido], pe.[IdUsuario], pe.[Fecha], pe.[IdEstado],
        pedEstado.[Nombre] AS [Estado], pe.[Total],
        pe.[PuntosGanados], pe.[PuntosUsados], pe.[DescuentoPuntos],
        (u.[Nombre] + ' ' + u.[Apellidos]) AS Cliente
      FROM [dbo].[Pedidos] pe
    INNER JOIN [dbo].[Usuarios] u ON u.[Id] = pe.[IdUsuario]
    INNER JOIN [dbo].[PedidosEstados] pedEstado ON pedEstado.[IdEstado] = pe.[IdEstado]
     WHERE pe.[Id] = @Id;

    SELECT
        dp.[IdProducto], p.[Nombre], dp.[Cantidad], dp.[PrecioUnitario], dp.[Subtotal]
      FROM [dbo].[DetallePedidos] dp
    INNER JOIN [dbo].[Productos] p ON p.[Id] = dp.[IdProducto]
     WHERE dp.[IdPedido] = @Id;
END