CREATE PROCEDURE AgregarDescuento
	@Id UNIQUEIDENTIFIER,
	@IdProducto UNIQUEIDENTIFIER,
	@PorcentajeDescuento DECIMAL(5,2),
	@FechaInicio DATETIME,
	@FechaFin DATETIME,
	@Activo BIT,
	@FechaCreacion DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	INSERT INTO [dbo].[Descuentos]
           ([Id], [IdProducto], [PorcentajeDescuento], [FechaInicio], [FechaFin], [Activo], [FechaCreacion])
     VALUES
           (@Id, @IdProducto, @PorcentajeDescuento, @FechaInicio, @FechaFin, @Activo, @FechaCreacion)

	SELECT @Id
	COMMIT TRANSACTION
END