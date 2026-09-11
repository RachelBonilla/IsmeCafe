
-- 2) RegistrarUsuarioCliente actualizado con el nuevo parámetro
CREATE   PROCEDURE RegistrarUsuarioCliente
    @Id UNIQUEIDENTIFIER,
    @Nombre VARCHAR(100),
    @Apellidos VARCHAR(100),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20),
    @Contrasena VARCHAR(100),
    @SuscritoMarketing BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdRolCliente UNIQUEIDENTIFIER;

    SELECT TOP 1 @IdRolCliente = Id
    FROM Roles
    WHERE Nombre = 'Cliente';

    IF @IdRolCliente IS NULL
    BEGIN
        RAISERROR('No existe el rol "Cliente" en la tabla Roles.', 16, 1);
        RETURN;
    END

    INSERT INTO Usuarios (Id, Nombre, Apellidos, Correo, Telefono, Contrasena, IdRol, Activo, FechaCreacion, SuscritoMarketing)
    VALUES (@Id, @Nombre, @Apellidos, @Correo, @Telefono, @Contrasena, @IdRolCliente, 1, GETDATE(), @SuscritoMarketing);

    SELECT @Id;
END;