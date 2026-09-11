CREATE PROCEDURE [dbo].[VaciarCarrito]
    @IdUsuario UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[Carritos] WHERE [IdUsuario] = @IdUsuario;
END
