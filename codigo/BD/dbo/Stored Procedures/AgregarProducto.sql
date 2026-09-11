CREATE   PROCEDURE AgregarProducto
	@Id UNIQUEIDENTIFIER,
	@IdCategoria UNIQUEIDENTIFIER,
	@Nombre VARCHAR(100),
	@Descripcion VARCHAR(500),
	@Precio DECIMAL(18,2),
	@Imagen VARCHAR(300),
	@Cantidad INT,
	@Activo BIT,
	@FechaCreacion DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION

	INSERT INTO [dbo].[Productos]
           ([Id], [IdCategoria], [Nombre], [Descripcion], [Precio], [Imagen], [Cantidad], [Activo], [FechaCreacion])
     VALUES
           (@Id, @IdCategoria, @Nombre, @Descripcion, @Precio, @Imagen, @Cantidad, @Activo, @FechaCreacion)

	SELECT @Id
	COMMIT TRANSACTION
END
