
CREATE PROCEDURE [dbo].[ReporteVentasPorEmpleado]
    @FechaInicio DATE,
    @FechaFin    DATE,
    @IdEmpleado  UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaInicio > @FechaFin
    BEGIN
        THROW 50030, 'El rango de fechas no es valido: la fecha de inicio es posterior a la fecha de fin.', 1;
    END

    DECLARE @Desde DATETIME = CAST(@FechaInicio AS DATETIME);
    DECLARE @Hasta DATETIME = DATEADD(DAY, 1, CAST(@FechaFin AS DATETIME));

    SELECT
        pe.[IdEmpleado],
        ISNULL(LTRIM(RTRIM(us.[Nombre] + ' ' + us.[Apellidos])), 'Sin asignar') AS [Empleado],
        ISNULL(us.[Correo], '')                    AS [Correo],
        COUNT(DISTINCT pe.[Id])                    AS [CantidadOrdenes],
        SUM(pe.[Total])                            AS [MontoTotal],
        CAST(SUM(pe.[Total]) / NULLIF(COUNT(DISTINCT pe.[Id]), 0)
             AS DECIMAL(18, 2))                    AS [TicketPromedio],
        ISNULL((
            SELECT SUM(dp.[Cantidad])
              FROM [dbo].[DetallePedidos] dp
              JOIN [dbo].[Pedidos] p2 ON p2.[Id] = dp.[IdPedido]
              JOIN [dbo].[PedidosEstados] estado2 ON estado2.[IdEstado] = p2.[IdEstado]
             WHERE p2.[Fecha] >= @Desde
               AND p2.[Fecha] <  @Hasta
               AND estado2.[Nombre] <> 'Cancelado'
               AND ((p2.[IdEmpleado] IS NULL AND pe.[IdEmpleado] IS NULL)
                    OR p2.[IdEmpleado] = pe.[IdEmpleado])
        ), 0)                                      AS [UnidadesVendidas],
        MIN(pe.[Fecha])                            AS [PrimeraVenta],
        MAX(pe.[Fecha])                            AS [UltimaVenta]
      FROM [dbo].[Pedidos] pe
      LEFT JOIN [dbo].[Usuarios] us ON us.[Id] = pe.[IdEmpleado]
      JOIN [dbo].[PedidosEstados] estado ON estado.[IdEstado] = pe.[IdEstado]
     WHERE pe.[Fecha] >= @Desde
       AND pe.[Fecha] <  @Hasta
       AND estado.[Nombre] <> 'Cancelado'
       AND (@IdEmpleado IS NULL OR pe.[IdEmpleado] = @IdEmpleado)
     GROUP BY pe.[IdEmpleado], us.[Nombre], us.[Apellidos], us.[Correo]
     ORDER BY [MontoTotal] DESC;
END
