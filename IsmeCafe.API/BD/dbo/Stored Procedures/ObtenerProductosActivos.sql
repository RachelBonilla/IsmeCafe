CREATE   PROCEDURE [dbo].[ObtenerProductosActivos]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		pr.[Id], pr.[Nombre], pr.[Descripcion], pr.[Precio], pr.[Imagen],
		pr.[Cantidad], pr.[Activo], pr.[FechaCreacion], pr.[FechaActualizacion],
		ca.[Nombre] AS [Categoria],
	    de.[PorcentajeDescuento],
        CASE
            WHEN de.[Id] IS NOT NULL
            THEN pr.[Precio] * (1 - de.[PorcentajeDescuento] / 100.0)
            ELSE NULL
        END AS [PrecioConDescuento]
    FROM [dbo].[Productos] pr
    LEFT JOIN [dbo].[Categorias] ca ON ca.[Id] = pr.[IdCategoria]
    LEFT JOIN [dbo].[Descuentos] de
        ON de.[IdProducto] = pr.[Id]
        AND de.[Activo] = 1
        AND GETDATE() BETWEEN de.[FechaInicio] AND de.[FechaFin]
    WHERE pr.[Activo] = 1
      AND pr.[Eliminado] = 0
END