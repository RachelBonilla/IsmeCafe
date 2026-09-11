CREATE   PROCEDURE EditarProducto
	@Id UNIQUEIDENTIFIER,
	@IdCategoria UNIQUEIDENTIFIER,
	@Nombre VARCHAR(100),
	@Descripcion VARCHAR(500),
	@Precio DECIMAL(18,2),
	@Imagen VARCHAR(300),
	@Cantidad INT,
	@Activo BIT,
	@FechaActualizacion DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION

	UPDATE [dbo].[Productos]
	SET
		[IdCategoria] = @IdCategoria,
		[Nombre] = @Nombre,
		[Descripcion] = @Descripcion,
		[Precio] = @Precio,
		[Imagen] = @Imagen,
		[Cantidad] = @Cantidad,
		[Activo] = @Activo,
		[FechaActualizacion] = @FechaActualizacion
	WHERE (Id = @Id)

	SELECT @Id
	COMMIT TRANSACTION
END
