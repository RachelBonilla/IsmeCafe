
CREATE   PROCEDURE [dbo].[ObtenerPedidosPorUsuario]
    @IdUsuario UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.[Id], p.[NumeroPedido], p.[IdUsuario], p.[Fecha], p.[IdEstado],
        pe.[Nombre] AS [Estado], p.[Total],
        p.[PuntosGanados], p.[PuntosUsados], p.[DescuentoPuntos]
    FROM [dbo].[Pedidos] p
    INNER JOIN [dbo].[PedidosEstados] pe ON pe.[IdEstado] = p.[IdEstado]
    WHERE p.[IdUsuario] = @IdUsuario
    ORDER BY p.[Fecha] DESC;
END