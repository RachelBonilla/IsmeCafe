CREATE   PROCEDURE ObtenerProducto
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		pr.[Id],
		pr.[IdCategoria],
		pr.[Nombre],
		pr.[Descripcion],
		pr.[Precio],
		pr.[Imagen],
		pr.[Cantidad],
		pr.[Activo],
		pr.[FechaCreacion],
		pr.[FechaActualizacion],
		ca.[Nombre] AS [Categoria]
	FROM [dbo].[Productos] pr
	LEFT JOIN [dbo].[Categorias] ca ON ca.[Id] = pr.[IdCategoria]
	WHERE pr.[Id] = @Id
	  AND pr.[Eliminado] = 0
END
