CREATE PROCEDURE [dbo].[ObtenerPedidos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.[Id],
        p.[NumeroPedido],
        p.[IdUsuario],
        p.[Fecha],
        p.[IdEstado],
        pe.[Nombre] AS [Estado],
        p.[Total],
        (u.[Nombre] + ' ' + u.[Apellidos]) AS [Cliente]
    FROM [dbo].[Pedidos] p
    INNER JOIN [dbo].[Usuarios] u
        ON u.[Id] = p.[IdUsuario]
    INNER JOIN [dbo].[PedidosEstados] pe
        ON pe.[IdEstado] = p.[IdEstado]
    ORDER BY p.[Fecha] DESC;
END