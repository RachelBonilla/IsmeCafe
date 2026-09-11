CREATE PROCEDURE EditarUsuario
    @Id UNIQUEIDENTIFIER,
    @Nombre VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20),
    @activo INT,
    @IdRol UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET 
        Nombre = @Nombre,
        Apellidos = @Apellidos,
        Correo = @Correo,
        Telefono = @Telefono,
        IdRol = @IdRol,
        Activo = @activo
    WHERE Id = @Id;

    SELECT @Id;
END;