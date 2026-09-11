CREATE PROCEDURE [dbo].[EliminarProductoCarrito]
    @IdUsuario  UNIQUEIDENTIFIER,
    @IdProducto UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[Carritos]
     WHERE [IdUsuario] = @IdUsuario AND [IdProducto] = @IdProducto;

    -- HU-22 CA4: informar si el carrito quedó vacío.
    DECLARE @Restantes INT;
    SELECT @Restantes = COUNT(1) FROM [dbo].[Carritos] WHERE [IdUsuario] = @IdUsuario;

    SELECT @Restantes AS ItemsRestantes;
END
