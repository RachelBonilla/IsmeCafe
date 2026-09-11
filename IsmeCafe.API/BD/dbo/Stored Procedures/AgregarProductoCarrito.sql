CREATE PROCEDURE [dbo].[AgregarProductoCarrito]
    @IdUsuario  UNIQUEIDENTIFIER,
    @IdProducto UNIQUEIDENTIFIER,
    @Cantidad   INT
AS
BEGIN
    SET NOCOUNT ON;

    -- HU-21 CA1/CA3/CA4: validar que el producto esté activo y con stock suficiente.
    DECLARE @Stock INT, @Activo BIT;
    SELECT @Stock = [Cantidad], @Activo = [Activo]
      FROM [dbo].[Productos]
     WHERE [Id] = @IdProducto;

    IF @Stock IS NULL
    BEGIN
        THROW 50001, 'El producto no existe.', 1;
    END

    IF @Activo = 0
    BEGIN
        THROW 50002, 'El producto no está activo.', 1;
    END

    IF @Stock <= 0
    BEGIN
        THROW 50003, 'El producto está agotado.', 1;
    END

    BEGIN TRANSACTION;

    -- Cantidad ya presente del mismo producto en el carrito del usuario.
    DECLARE @CantidadActual INT = 0;
    SELECT @CantidadActual = ISNULL([Cantidad], 0)
      FROM [dbo].[Carritos]
     WHERE [IdUsuario] = @IdUsuario AND [IdProducto] = @IdProducto;

    DECLARE @NuevaCantidad INT = @CantidadActual + @Cantidad;

    -- HU-21 CA4: si la suma supera el stock, se limita al stock disponible.
    IF @NuevaCantidad > @Stock
        SET @NuevaCantidad = @Stock;

    IF @CantidadActual = 0
    BEGIN
        INSERT INTO [dbo].[Carritos] ([Id], [IdUsuario], [IdProducto], [Cantidad], [FechaCreacion])
        VALUES (NEWID(), @IdUsuario, @IdProducto, @NuevaCantidad, GETDATE());
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Carritos]
           SET [Cantidad]           = @NuevaCantidad,
               [FechaActualizacion] = GETDATE()
         WHERE [IdUsuario] = @IdUsuario AND [IdProducto] = @IdProducto;
    END

    COMMIT TRANSACTION;

    SELECT @NuevaCantidad AS CantidadFinal, @Stock AS StockDisponible;
END
