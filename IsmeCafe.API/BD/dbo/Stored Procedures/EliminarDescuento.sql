
--Eliminar/Inactivar descuento--

CREATE PROCEDURE EliminarDescuento
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	UPDATE [dbo].[Descuentos]
	SET [Activo] = 0,
		[FechaActualizacion] = GETDATE()
	WHERE (Id = @Id)

	SELECT @Id
	COMMIT TRANSACTION
END