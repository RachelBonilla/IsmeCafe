
--Agregar oferta a un producto--
CREATE PROCEDURE AgregarOfertaProducto
	@Id UNIQUEIDENTIFIER,
	@IdOferta UNIQUEIDENTIFIER,
	@IdProducto UNIQUEIDENTIFIER,
	@Cantidad INT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	INSERT INTO [dbo].[OfertaProductos] ([Id], [IdOferta], [IdProducto], [Cantidad])
	VALUES (@Id, @IdOferta, @IdProducto, @Cantidad)

	SELECT @Id
	COMMIT TRANSACTION
END