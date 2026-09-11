-- Seed de servicios según el diseño "Nuestros Servicios"
-- Duracion en minutos

INSERT INTO [dbo].[Servicios] ([Nombre], [Descripcion], [Duracion], [Precio], [CupoMaximo], [Imagen])
SELECT v.Nombre, v.Descripcion, v.Duracion, v.Precio, v.CupoMaximo, v.Imagen
FROM (VALUES
    ('Coffee Tour Tradicional',
     'Un recorrido inmersivo por nuestra finca. Aprenda sobre el proceso del café desde la semilla hasta la taza, caminando por los cafetales, visitando el beneficio húmedo y terminando con una degustación. Mínimo 2 pax. Nivel: Fácil.',
     120, 15000.00, 15,
     'https://images.unsplash.com/photo-1611080626919-7cf5a9dbab5b?w=800&q=80'),
    ('Cata de Café Premium',
     'Descubra los perfiles de sabor de nuestros mejores microlotes. Guiado por nuestro barista experto, aprenderá a identificar notas de cata, fragancias y aromas utilizando protocolos profesionales. Mínimo 1 pax. Incluye certificado.',
     90, 22000.00, 10,
     'https://images.unsplash.com/photo-1447933601403-0c6688de566e?w=800&q=80')
) AS v (Nombre, Descripcion, Duracion, Precio, CupoMaximo, Imagen)
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Servicios] s WHERE s.Nombre = v.Nombre
);
