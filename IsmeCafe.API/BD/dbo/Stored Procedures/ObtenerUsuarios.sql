
CREATE   PROCEDURE ObtenerUsuarios
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.Id, u.Nombre, u.Apellidos, u.Correo, u.Telefono, u.Activo, 
        u.FechaCreacion, u.IdRol, u.Puntos,
        r.Nombre AS NombreRol
    FROM Usuarios u
    INNER JOIN Roles r ON u.IdRol = r.Id
    ORDER BY u.FechaCreacion DESC;
END;