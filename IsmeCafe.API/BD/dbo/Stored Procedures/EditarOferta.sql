
--Editar ofertas--
CREATE PROCEDURE EditarOferta
	@Id UNIQUEIDENTIFIER,
	@Nombre VARCHAR(150),
	@Descripcion VARCHAR(500),
	@TipoOferta VARCHAR(20),
	@PrecioCombo DECIMAL(18,2),
	@FechaInicio DATETIME,
	@FechaFin DATETIME,
	@Activo BIT,
	@FechaActualizacion DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	UPDATE [dbo].[Ofertas]
	SET
		[Nombre] = @Nombre,
		[Descripcion] = @Descripcion,
		[TipoOferta] = @TipoOferta,
		[PrecioCombo] = @PrecioCombo,
		[FechaInicio] = @FechaInicio,
		[FechaFin] = @FechaFin,
		[Activo] = @Activo,
		[FechaActualizacion] = @FechaActualizacion
	WHERE (Id = @Id)

	SELECT @Id
	COMMIT TRANSACTION
END