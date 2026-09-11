CREATE PROCEDURE ObtenerUsuarioPorId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.Id, 
        u.Nombre, 
        u.Apellidos, 
        u.Correo, 
        u.Telefono, 
        u.Activo, 
        u.FechaCreacion,
        u.IdRol,
        r.Nombre AS NombreRol
    FROM Usuarios u
    INNER JOIN Roles r ON u.IdRol = r.Id
    WHERE u.Id = @Id;
END;