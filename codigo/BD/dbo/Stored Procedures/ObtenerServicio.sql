-- Leer un Servicio por Id
CREATE PROCEDURE ObtenerServicio
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SELECT Id, Nombre, Descripcion, Duracion, Precio, CupoMaximo, Activo, FechaCreacion
    FROM Servicios
    WHERE Id = @Id;
END;
GO