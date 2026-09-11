CREATE PROCEDURE [dbo].[ObtenerCarrito]
    @IdUsuario UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.[Id]              AS IdItem,
        c.[IdProducto],
        p.[Nombre],
        p.[Imagen],
        p.[Precio],
        c.[Cantidad],
        p.[Cantidad]        AS StockDisponible,
        p.[Activo],
        (p.[Precio] * c.[Cantidad]) AS Subtotal
      FROM [dbo].[Carritos] c
      JOIN [dbo].[Productos] p ON p.[Id] = c.[IdProducto]
     WHERE c.[IdUsuario] = @IdUsuario
     ORDER BY c.[FechaCreacion] ASC;
END
