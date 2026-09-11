CREATE PROCEDURE [dbo].[ActualizarCantidadCarrito]
    @IdUsuario  UNIQUEIDENTIFIER,
    @IdProducto UNIQUEIDENTIFIER,
    @Cantidad   INT
AS
BEGIN
    SET NOCOUNT ON;

    -- HU-22 CA3: cantidad inválida.
    IF @Cantidad <= 0
    BEGIN
        THROW 50010, 'La cantidad debe ser mayor a cero.', 1;
    END

    -- Verificar existencia del item en el carrito.
    IF NOT EXISTS (
        SELECT 1 FROM [dbo].[Carritos]
         WHERE [IdUsuario] = @IdUsuario AND [IdProducto] = @IdProducto
    )
    BEGIN
        THROW 50011, 'El producto no está en el carrito.', 1;
    END

    -- HU-22 CA1: verificar stock suficiente.
    DECLARE @Stock INT;
    SELECT @Stock = [Cantidad] FROM [dbo].[Productos] WHERE [Id] = @IdProducto;

    IF @Cantidad > @Stock
    BEGIN
        THROW 50012, 'No hay stock suficiente para la cantidad solicitada.', 1;
    END

    UPDATE [dbo].[Carritos]
       SET [Cantidad]           = @Cantidad,
           [FechaActualizacion] = GETDATE()
     WHERE [IdUsuario] = @IdUsuario AND [IdProducto] = @IdProducto;

    SELECT @Cantidad AS CantidadFinal;
END
