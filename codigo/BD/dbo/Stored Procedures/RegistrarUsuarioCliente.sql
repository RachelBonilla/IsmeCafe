CREATE PROCEDURE RegistrarUsuarioCliente
    @Id UNIQUEIDENTIFIER,
    @Nombre VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20),
    @Contrasena VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdRolCliente UNIQUEIDENTIFIER;

    SELECT TOP 1 @IdRolCliente = Id 
    FROM Roles 
    WHERE Nombre = 'Cliente';

    INSERT INTO Usuarios (Id, Nombre, Apellidos, Correo, Telefono, Contrasena, IdRol, Activo, FechaCreacion)
    VALUES (@Id, @Nombre, @Apellidos, @Correo, @Telefono, @Contrasena, @IdRolCliente, 1, GETDATE());

    SELECT @Id;
END;