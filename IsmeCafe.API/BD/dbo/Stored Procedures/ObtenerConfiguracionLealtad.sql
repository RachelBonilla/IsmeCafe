
-- 3) La configuración ahora devuelve también el valor de canje
CREATE   PROCEDURE ObtenerConfiguracionLealtad
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 Id, PuntosPorMonto, ValorPunto, FechaActualizacion
    FROM ConfiguracionLealtad;
END;