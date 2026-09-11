CREATE PROCEDURE DesactivarUsuario
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET Activo = 0
    WHERE Id = @Id;

    SELECT @Id;
END;