CREATE PROCEDURE [dbo].[ActualizarContrasena]
    @Id UNIQUEIDENTIFIER,
    @Contrasena VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Usuarios]
    SET [Contrasena] = @Contrasena
    WHERE [Id] = @Id;
END