
CREATE   PROCEDURE ObtenerUsuarioPorCorreo
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.Id,
        u.Nombre,
        u.Apellidos,
        u.Correo,
        u.Telefono,
        u.Contrasena,
        u.Activo,
        u.IdRol,
        r.Nombre AS NombreRol
    FROM Usuarios u
    INNER JOIN Roles r ON u.IdRol = r.Id
    WHERE u.Correo = @Correo;
END;