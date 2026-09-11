
--Campañas de marketing/Envio de correos electronicos--
--Registrar campaña--
CREATE PROCEDURE RegistrarCampana
	@Id UNIQUEIDENTIFIER,
	@Tipo VARCHAR(20),
	@IdOferta UNIQUEIDENTIFIER,
	@IdDescuento UNIQUEIDENTIFIER,
	@Asunto VARCHAR(200),
	@Contenido VARCHAR(MAX),
	@FechaEnvio DATETIME,
	@CantidadDestinatarios INT,
	@Exitosa BIT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	INSERT INTO [dbo].[CampanasMarketing]
		   ([Id], [Tipo], [IdOferta], [IdDescuento], [Asunto], [Contenido], [FechaEnvio], [CantidadDestinatarios], [Exitosa])
	VALUES (@Id, @Tipo, @IdOferta, @IdDescuento, @Asunto, @Contenido, @FechaEnvio, @CantidadDestinatarios, @Exitosa)

	SELECT @Id
	COMMIT TRANSACTION
END