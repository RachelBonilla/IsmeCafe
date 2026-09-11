CREATE PROCEDURE [dbo].[ObtenerPedidosPorUsuario]
    @IdUsuario UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Id],
        [NumeroPedido],
        [IdUsuario],
        [Fecha],
        [Estado],
        [Total]
      FROM [dbo].[Pedidos]
     WHERE [IdUsuario] = @IdUsuario
     ORDER BY [Fecha] DESC;
END
