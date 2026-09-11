
-- 4) ConfirmarPedido ahora acepta canjear puntos como descuento (mantiene la lógica de IdEstado tal cual está)
CREATE   PROCEDURE [dbo].[ConfirmarPedido]
    @IdUsuario  UNIQUEIDENTIFIER,
    @IdPedido   UNIQUEIDENTIFIER,
    @UsarPuntos BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (SELECT 1 FROM [dbo].[Carritos] WHERE [IdUsuario] = @IdUsuario)
    BEGIN
        RAISERROR('Debe agregar productos antes de confirmar el pedido.', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

    IF EXISTS (
        SELECT 1
          FROM [dbo].[Carritos] c
          JOIN [dbo].[Productos] p ON p.[Id] = c.[IdProducto]
         WHERE c.[IdUsuario] = @IdUsuario
           AND (p.[Activo] = 0 OR p.[Cantidad] < c.[Cantidad])
    )
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('Uno o más productos no tienen stock suficiente.', 16, 1);
        RETURN;
    END

    DECLARE @Total DECIMAL(18,2);
    SELECT @Total = SUM(p.[Precio] * c.[Cantidad])
      FROM [dbo].[Carritos] c
      JOIN [dbo].[Productos] p ON p.[Id] = c.[IdProducto]
     WHERE c.[IdUsuario] = @IdUsuario;

    DECLARE @IdEstadoPendiente INT;
    SELECT @IdEstadoPendiente = [IdEstado]
    FROM [dbo].[PedidosEstados]
    WHERE [Nombre] = 'Pendiente';

    IF @IdEstadoPendiente IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('No se encontró configurado el estado Pendiente.', 16, 1);
        RETURN;
    END

    -- Canje de puntos (si se solicitó)
    DECLARE @PuntosAUsar INT = 0;
    DECLARE @DescuentoPuntos DECIMAL(18,2) = 0;

    IF @UsarPuntos = 1
    BEGIN
        DECLARE @PuntosDisponibles INT;
        DECLARE @ValorPunto DECIMAL(18,2);

        SELECT @PuntosDisponibles = Puntos FROM [dbo].[Usuarios] WHERE [Id] = @IdUsuario;
        SELECT TOP 1 @ValorPunto = ValorPunto FROM [dbo].[ConfiguracionLealtad];

        IF @PuntosDisponibles > 0 AND @ValorPunto IS NOT NULL AND @ValorPunto > 0
        BEGIN
            DECLARE @ValorTotalPuntos DECIMAL(18,2) = @PuntosDisponibles * @ValorPunto;

            IF @ValorTotalPuntos >= @Total
                SET @PuntosAUsar = FLOOR(@Total / @ValorPunto);
            ELSE
                SET @PuntosAUsar = @PuntosDisponibles;

            SET @DescuentoPuntos = @PuntosAUsar * @ValorPunto;
        END
    END

    DECLARE @TotalFinal DECIMAL(18,2) = @Total - @DescuentoPuntos;

    -- Puntos ganados: sobre lo realmente pagado, no sobre el total original.
    DECLARE @PuntosPorMonto DECIMAL(18,2);
    SELECT TOP 1 @PuntosPorMonto = PuntosPorMonto FROM [dbo].[ConfiguracionLealtad];

    DECLARE @PuntosGanados INT = 0;
    IF @PuntosPorMonto IS NOT NULL AND @PuntosPorMonto > 0
        SET @PuntosGanados = FLOOR(@TotalFinal / @PuntosPorMonto);

    INSERT INTO [dbo].[Pedidos] ([Id], [IdUsuario], [Fecha], [IdEstado], [Total], [PuntosGanados], [PuntosUsados], [DescuentoPuntos])
    VALUES (@IdPedido, @IdUsuario, GETDATE(), @IdEstadoPendiente, @TotalFinal, @PuntosGanados, @PuntosAUsar, @DescuentoPuntos);

    INSERT INTO [dbo].[DetallePedidos] ([Id], [IdPedido], [IdProducto], [Cantidad], [PrecioUnitario], [Subtotal])
    SELECT NEWID(), @IdPedido, c.[IdProducto], c.[Cantidad], p.[Precio], (p.[Precio] * c.[Cantidad])
      FROM [dbo].[Carritos] c
      JOIN [dbo].[Productos] p ON p.[Id] = c.[IdProducto]
     WHERE c.[IdUsuario] = @IdUsuario;

    UPDATE p
       SET [Cantidad] = p.[Cantidad] - c.[Cantidad],
           [FechaActualizacion] = GETDATE()
      FROM [dbo].[Productos] p
      JOIN [dbo].[Carritos] c ON c.[IdProducto] = p.[Id]
     WHERE c.[IdUsuario] = @IdUsuario;

    -- Neto de puntos: se descuentan los usados y se suman los ganados en una sola operación.
    UPDATE [dbo].[Usuarios]
       SET [Puntos] = [Puntos] - @PuntosAUsar + @PuntosGanados
     WHERE [Id] = @IdUsuario;

    DELETE FROM [dbo].[Carritos] WHERE [IdUsuario] = @IdUsuario;

    COMMIT TRANSACTION;

    SELECT
        p.[NumeroPedido], p.[Total], p.[Fecha], p.[IdEstado], pe.[Nombre] AS [Estado],
        p.[PuntosGanados], p.[PuntosUsados], p.[DescuentoPuntos]
    FROM [dbo].[Pedidos] p
    INNER JOIN [dbo].[PedidosEstados] pe ON pe.[IdEstado] = p.[IdEstado]
    WHERE p.[Id] = @IdPedido;
END