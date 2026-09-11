CREATE PROCEDURE [dbo].[EliminarReserva]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[Reservas]
    WHERE Id = @Id;

    SELECT @Id;
END;