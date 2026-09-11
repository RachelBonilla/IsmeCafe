-- Actualizar Servicio
CREATE PROCEDURE spActualizarServicio
    @Id UNIQUEIDENTIFIER,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(500),
    @Duracion INT,
    @Precio DECIMAL(18,2),
    @CupoMaximo INT,
    @Activo BIT
AS
BEGIN
    UPDATE Servicios
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Duracion = @Duracion,
        Precio = @Precio,
        CupoMaximo = @CupoMaximo,
        Activo = @Activo
    WHERE Id = @Id;
END;