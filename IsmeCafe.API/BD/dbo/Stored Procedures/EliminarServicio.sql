-- Eliminar Servicio
CREATE PROCEDURE EliminarServicio
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM Servicios
    WHERE Id = @Id;
END;
GO