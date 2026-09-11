CREATE PROCEDURE RegistrarUsuario
    @Id UNIQUEIDENTIFIER,
    @Nombre VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20),
    @Contrasena VARCHAR(100),
    @IdRol UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Usuarios (
        Id, 
        Nombre, 
        Apellidos, 
        Correo, 
        Telefono, 
        Contrasena, 
        IdRol, 
        Activo, 
        FechaCreacion
    )
    VALUES (
        @Id, 
        @Nombre, 
        @Apellidos, 
        @Correo, 
        @Telefono, 
        @Contrasena, 
        @IdRol, 
        1,
        GETDATE()
    );

    SELECT @Id;
END;