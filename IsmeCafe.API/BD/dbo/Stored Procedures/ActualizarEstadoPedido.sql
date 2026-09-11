CREATE PROCEDURE [dbo].[ActualizarEstadoPedido]
    @IdPedido UNIQUEIDENTIFIER,
    @IdEstado INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM [dbo].[Pedidos]
        WHERE [Id] = @IdPedido
    )
    BEGIN
        RAISERROR('El pedido indicado no existe.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (
        SELECT 1
        FROM [dbo].[PedidosEstados]
        WHERE [IdEstado] = @IdEstado
          AND [Activo] = 1
    )
    BEGIN
        RAISERROR('El estado indicado no existe o se encuentra inactivo.', 16, 1);
        RETURN;
    END

    UPDATE [dbo].[Pedidos]
       SET [IdEstado] = @IdEstado
     WHERE [Id] = @IdPedido;

    SELECT
        p.[Id],
        p.[NumeroPedido],
        p.[IdUsuario],
        p.[Fecha],
        p.[IdEstado],
        pe.[Nombre] AS [Estado],
        p.[Total]
    FROM [dbo].[Pedidos] p
    INNER JOIN [dbo].[PedidosEstados] pe
        ON pe.[IdEstado] = p.[IdEstado]
    WHERE p.[Id] = @IdPedido;
END