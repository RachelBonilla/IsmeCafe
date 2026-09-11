CREATE PROCEDURE [dbo].[ReporteInventario]
    @IdCategoria UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pr.[Id],
        pr.[Nombre],
        ISNULL(ca.[Nombre], 'Sin categoria')            AS [Categoria],
        pr.[IdCategoria],
        pr.[Cantidad]                                   AS [StockActual],
        pr.[StockMinimo],
        pr.[Precio],
        pr.[Activo],
        CAST(CASE WHEN pr.[Cantidad] <= pr.[StockMinimo]
                  THEN 1 ELSE 0 END AS BIT)             AS [StockBajo],
        CASE
            WHEN pr.[Cantidad] = 0                      THEN 'Agotado'
            WHEN pr.[Cantidad] <= pr.[StockMinimo]      THEN 'Bajo'
            ELSE 'Disponible'
        END                                             AS [EstadoExistencias]
      FROM [dbo].[Productos] pr
      LEFT JOIN [dbo].[Categorias] ca ON ca.[Id] = pr.[IdCategoria]
     WHERE pr.[Eliminado] = 0
       AND (@IdCategoria IS NULL OR pr.[IdCategoria] = @IdCategoria)
     ORDER BY
        CASE
            WHEN pr.[Cantidad] = 0                 THEN 0
            WHEN pr.[Cantidad] <= pr.[StockMinimo] THEN 1
            ELSE 2
        END,
        pr.[Nombre];
END
