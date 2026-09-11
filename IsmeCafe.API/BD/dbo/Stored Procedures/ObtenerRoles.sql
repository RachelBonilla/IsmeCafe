CREATE PROCEDURE ObtenerRoles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Nombre
    FROM Roles
    ORDER BY Nombre;
END;