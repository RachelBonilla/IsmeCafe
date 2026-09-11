-- Crear Servicio
CREATE PROCEDURE AgregarServicio
    @Id UNIQUEIDENTIFIER,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(500),
    @Duracion INT,
    @Precio DECIMAL(18,2),
    @CupoMaximo INT,
    @Activo BIT,
    @FechaCreacion DATETIME
AS
BEGIN
    INSERT INTO Servicios (Id, Nombre, Descripcion, Duracion, Precio, CupoMaximo, Activo, FechaCreacion)
    VALUES (@Id, @Nombre, @Descripcion, @Duracion, @Precio, @CupoMaximo, @Activo, @FechaCreacion);

    SELECT @Id;
END;
GO
