-- Leer todos los Servicios
CREATE PROCEDURE spObtenerServicios
AS
BEGIN
    SELECT Id, Nombre, Descripcion, Duracion, Precio, CupoMaximo, Activo, FechaCreacion
    FROM Servicios;
END;