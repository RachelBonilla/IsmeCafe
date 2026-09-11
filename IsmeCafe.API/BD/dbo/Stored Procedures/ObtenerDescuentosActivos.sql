
--ObtenerDescuentos Activos--
CREATE PROCEDURE ObtenerDescuentosActivos
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		de.[Id], de.[IdProducto], de.[PorcentajeDescuento], de.[FechaInicio], de.[FechaFin],
		de.[Activo], de.[FechaCreacion], de.[FechaActualizacion],
		pr.[Nombre] AS [NombreProducto], pr.[Precio] AS [PrecioProducto]
	FROM [dbo].[Descuentos] de
	INNER JOIN [dbo].[Productos] pr ON pr.[Id] = de.[IdProducto]
	WHERE de.[Activo] = 1 AND GETDATE() BETWEEN de.[FechaInicio] AND de.[FechaFin]
END