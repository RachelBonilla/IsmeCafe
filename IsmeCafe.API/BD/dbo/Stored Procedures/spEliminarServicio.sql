-- Eliminar Servicio
CREATE PROCEDURE spEliminarServicio
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM Servicios
    WHERE Id = @Id;
END;