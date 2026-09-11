CREATE PROCEDURE ObtenerOferta
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		ofe.[Id], ofe.[Nombre], ofe.[Descripcion], ofe.[TipoOferta], ofe.[PrecioCombo],
		ofe.[FechaInicio], ofe.[FechaFin], ofe.[Activo], ofe.[FechaCreacion], ofe.[FechaActualizacion]
	FROM [dbo].[Ofertas] ofe
	WHERE ofe.[Id] = @Id

	SELECT
		op.[Id], op.[IdOferta], op.[IdProducto], op.[Cantidad],
		pr.[Nombre] AS [NombreProducto], pr.[Precio] AS [PrecioProducto]
	FROM [dbo].[OfertaProductos] op
	INNER JOIN [dbo].[Productos] pr ON pr.[Id] = op.[IdProducto]
	WHERE op.[IdOferta] = @Id
END