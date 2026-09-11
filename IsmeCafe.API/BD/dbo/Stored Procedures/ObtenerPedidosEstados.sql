CREATE PROCEDURE [dbo].[ObtenerPedidosEstados]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [IdEstado],
        [Nombre]
    FROM [dbo].[PedidosEstados]
    WHERE [Activo] = 1
    ORDER BY [IdEstado];
END