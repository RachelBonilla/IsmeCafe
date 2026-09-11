
CREATE PROCEDURE [dbo].[DesactivarUsuario]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET Activo = CASE 
                    WHEN Activo = 0 THEN 1
                    ELSE 0
                 END
    WHERE Id = @Id;

    SELECT @Id;
END;