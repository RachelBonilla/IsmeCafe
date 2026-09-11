CREATE PROCEDURE [dbo].[ConfirmarPedido]
    @IdUsuario UNIQUEIDENTIFIER,
    @IdPedido  UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- HU-23 CA2: no permitir si el carrito está vacío.
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Carritos] WHERE [IdUsuario] = @IdUsuario)
    BEGIN
        THROW 50020, 'Debe agregar productos antes de confirmar el pedido.', 1;
    END

    BEGIN TRANSACTION;

    -- HU-23 CA3: verificar stock disponible para cada item del carrito.
    IF EXISTS (
        SELECT 1
          FROM [dbo].[Carritos] c
          JOIN [dbo].[Productos] p ON p.[Id] = c.[IdProducto]
         WHERE c.[IdUsuario] = @IdUsuario
           AND (p.[Activo] = 0 OR p.[Cantidad] < c.[Cantidad])
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50021, 'Uno o más productos no tienen stock suficiente.', 1;
    END

    DECLARE @Total DECIMAL(18,2);
    SELECT @Total = SUM(p.[Precio] * c.[Cantidad])
      FROM [dbo].[Carritos] c
      JOIN [dbo].[Productos] p ON p.[Id] = c.[IdProducto]
     WHERE c.[IdUsuario] = @IdUsuario;

    -- HU-23 CA4: registrar el pedido con estado inicial 'Pendiente'.
    INSERT INTO [dbo].[Pedidos] ([Id], [IdUsuario], [Fecha], [Estado], [Total])
    VALUES (@IdPedido, @IdUsuario, GETDATE(), 'Pendiente', @Total);

    -- Registrar el detalle del pedido a partir del carrito.
    INSERT INTO [dbo].[DetallePedidos] ([Id], [IdPedido], [IdProducto], [Cantidad], [PrecioUnitario], [Subtotal])
    SELECT NEWID(), @IdPedido, c.[IdProducto], c.[Cantidad], p.[Precio], (p.[Precio] * c.[Cantidad])
      FROM [dbo].[Carritos] c
      JOIN [dbo].[Productos] p ON p.[Id] = c.[IdProducto]
     WHERE c.[IdUsuario] = @IdUsuario;

    -- HU-23 CA1: descontar stock.
    UPDATE p
       SET [Cantidad] = p.[Cantidad] - c.[Cantidad],
           [FechaActualizacion] = GETDATE()
      FROM [dbo].[Productos] p
      JOIN [dbo].[Carritos] c ON c.[IdProducto] = p.[Id]
     WHERE c.[IdUsuario] = @IdUsuario;

    -- HU-23 CA1: vaciar el carrito.
    DELETE FROM [dbo].[Carritos] WHERE [IdUsuario] = @IdUsuario;

    COMMIT TRANSACTION;

    -- Devolver el número de pedido asignado.
    SELECT [NumeroPedido], [Total], [Fecha], [Estado]
      FROM [dbo].[Pedidos]
     WHERE [Id] = @IdPedido;
END
