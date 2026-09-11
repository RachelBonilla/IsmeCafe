-- Leer todos los Servicios
CREATE PROCEDURE ObtenerServicios
AS
BEGIN
    SELECT Id, Nombre, Descripcion, Duracion, Precio, CupoMaximo, Activo, FechaCreacion
    FROM Servicios;
END;
GO