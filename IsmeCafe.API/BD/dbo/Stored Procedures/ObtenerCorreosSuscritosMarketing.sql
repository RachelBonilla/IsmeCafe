
-- 3) Nuevo SP: correos suscritos, para las campañas
CREATE   PROCEDURE ObtenerCorreosSuscritosMarketing
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Correo
    FROM Usuarios
    WHERE Activo = 1 AND SuscritoMarketing = 1;
END;