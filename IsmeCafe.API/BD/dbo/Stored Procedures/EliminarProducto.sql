CREATE   PROCEDURE EliminarProducto
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION

	UPDATE [dbo].[Productos]
	SET [Eliminado] = 1,
		[Activo] = 0,
		[FechaActualizacion] = GETDATE()
	WHERE (Id = @Id)

	SELECT @Id
	COMMIT TRANSACTION
END
