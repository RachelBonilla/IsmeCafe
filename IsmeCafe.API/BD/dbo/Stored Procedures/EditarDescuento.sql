
--Editar descuentos--

CREATE PROCEDURE EditarDescuento
	@Id UNIQUEIDENTIFIER,
	@IdProducto UNIQUEIDENTIFIER,
	@PorcentajeDescuento DECIMAL(5,2),
	@FechaInicio DATETIME,
	@FechaFin DATETIME,
	@Activo BIT,
	@FechaActualizacion DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	UPDATE [dbo].[Descuentos]
	SET
		[IdProducto] = @IdProducto,
		[PorcentajeDescuento] = @PorcentajeDescuento,
		[FechaInicio] = @FechaInicio,
		[FechaFin] = @FechaFin,
		[Activo] = @Activo,
		[FechaActualizacion] = @FechaActualizacion
	WHERE (Id = @Id)

	SELECT @Id
	COMMIT TRANSACTION
END