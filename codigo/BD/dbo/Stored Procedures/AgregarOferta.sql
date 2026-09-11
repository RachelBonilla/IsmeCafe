
--Ofertas--
--Agregar ofertas--
CREATE PROCEDURE AgregarOferta
	@Id UNIQUEIDENTIFIER,
	@Nombre VARCHAR(150),
	@Descripcion VARCHAR(500),
	@TipoOferta VARCHAR(20),
	@PrecioCombo DECIMAL(18,2),
	@FechaInicio DATETIME,
	@FechaFin DATETIME,
	@Activo BIT,
	@FechaCreacion DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	INSERT INTO [dbo].[Ofertas]
           ([Id], [Nombre], [Descripcion], [TipoOferta], [PrecioCombo], [FechaInicio], [FechaFin], [Activo], [FechaCreacion])
     VALUES
           (@Id, @Nombre, @Descripcion, @TipoOferta, @PrecioCombo, @FechaInicio, @FechaFin, @Activo, @FechaCreacion)

	SELECT @Id
	COMMIT TRANSACTION
END