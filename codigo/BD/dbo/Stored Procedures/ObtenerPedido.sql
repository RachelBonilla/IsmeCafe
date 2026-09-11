CREATE PROCEDURE [dbo].[ObtenerPedido]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pe.[Id],
        pe.[NumeroPedido],
        pe.[IdUsuario],
        pe.[Fecha],
        pe.[Estado],
        pe.[Total],
        (u.[Nombre] + ' ' + u.[Apellidos]) AS Cliente
      FROM [dbo].[Pedidos] pe
      JOIN [dbo].[Usuarios] u ON u.[Id] = pe.[IdUsuario]
     WHERE pe.[Id] = @Id;

    SELECT
        dp.[IdProducto],
        p.[Nombre],
        dp.[Cantidad],
        dp.[PrecioUnitario],
        dp.[Subtotal]
      FROM [dbo].[DetallePedidos] dp
      JOIN [dbo].[Productos] p ON p.[Id] = dp.[IdProducto]
     WHERE dp.[IdPedido] = @Id;
END
