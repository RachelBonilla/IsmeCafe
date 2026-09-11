-- Crear Servicio
CREATE PROCEDURE spCrearServicio
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(500),
    @Duracion INT,
    @Precio DECIMAL(18,2),
    @CupoMaximo INT,
    @Activo BIT
AS
BEGIN
    INSERT INTO Servicios (Id, Nombre, Descripcion, Duracion, Precio, CupoMaximo, Activo, FechaCreacion)
    VALUES (NEWID(), @Nombre, @Descripcion, @Duracion, @Precio, @CupoMaximo, @Activo, GETDATE());
END;