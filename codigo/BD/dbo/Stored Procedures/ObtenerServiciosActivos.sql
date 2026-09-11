-- Leer los Servicios activos (catálogo público)
CREATE PROCEDURE ObtenerServiciosActivos
AS
BEGIN
    SELECT Id, Nombre, Descripcion, Duracion, Precio, CupoMaximo, Activo, FechaCreacion
    FROM Servicios
    WHERE Activo = 1;
END;
GO
