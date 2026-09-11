
USE IsmeCafe
GO

-- Limpieza en orden inverso a las llaves foraneas.
-- Roles y Usuarios NO se borran: pueden existir cuentas reales.
DELETE FROM Carritos;
DELETE FROM DetallePedidos;
DELETE FROM Pedidos;
DELETE FROM OfertaProductos;
DELETE FROM Descuentos;
DELETE FROM Productos;
DELETE FROM Categorias;
DELETE FROM Servicios;
GO

-- Categorias con identificadores fijos para referenciarlas desde Swagger / la API.
INSERT INTO Categorias (Id, Nombre, Descripcion) VALUES
('B0000000-0000-0000-0000-000000000001', 'Café en grano', 'Café tostado en grano y molido'),
('B0000000-0000-0000-0000-000000000002', 'Bebidas', 'Bebidas calientes y frías'),
('B0000000-0000-0000-0000-000000000003', 'Repostería', 'Pasteles, galletas y postres'),
('B0000000-0000-0000-0000-000000000004', 'Merchandising', 'Tazas, termos y accesorios');
GO

-- Productos con Id fijo (el Nº1 se usa como ejemplo por defecto en Swagger).
-- StockMinimo alimenta las alertas de existencias bajas del reporte
INSERT INTO Productos (Id, IdCategoria, Nombre, Descripcion, Precio, Imagen, Cantidad, StockMinimo, Activo, FechaCreacion) VALUES
('C0000001-0000-0000-0000-000000000001', 'B0000000-0000-0000-0000-000000000001', 'Café de altura 250g', 'Café molido de tueste medio, notas a chocolate y cítricos.', 4500.00, 'https://images.unsplash.com/photo-1642437271884-fe1d3393eef9?auto=format&fit=crop&w=800&q=80', 25, 10, 1, GETDATE()),
('C0000001-0000-0000-0000-000000000002', 'B0000000-0000-0000-0000-000000000001', 'Espresso premium 500g', 'Mezcla intensa ideal para espresso.', 8200.00, 'https://images.unsplash.com/photo-1544486864-3087e2e20d91?auto=format&fit=crop&w=800&q=80', 15, 8, 1, GETDATE()),
('C0000001-0000-0000-0000-000000000003', 'B0000000-0000-0000-0000-000000000002', 'Latte de la casa', 'Latte caliente con leche vaporizada.', 2200.00, 'https://images.unsplash.com/photo-1559001724-fbad036dbc9e?auto=format&fit=crop&w=800&q=80', 50, 20, 1, GETDATE()),
-- Queda por debajo del minimo: sirve para comprobar la alerta del reporte de inventario.
('C0000001-0000-0000-0000-000000000004', 'B0000000-0000-0000-0000-000000000003', 'Cheesecake de frutos rojos', 'Porción individual de cheesecake.', 3100.00, 'https://images.unsplash.com/photo-1533134242443-d4fd215305ad?auto=format&fit=crop&w=800&q=80', 12, 15, 1, GETDATE()),
-- Agotado e inactivo.
('C0000001-0000-0000-0000-000000000005', 'B0000000-0000-0000-0000-000000000004', 'Taza Isme Café', 'Taza de cerámica con logo del café.', 5000.00, 'https://images.unsplash.com/photo-1630595478342-5b257b95f783?auto=format&fit=crop&w=800&q=80', 0, 5, 0, GETDATE());
GO

-- Servicios según el diseño "Nuestros Servicios". Duracion en minutos.
INSERT INTO Servicios (Id, Nombre, Descripcion, Duracion, Precio, CupoMaximo, Imagen, Activo, FechaCreacion) VALUES
('D0000001-0000-0000-0000-000000000001', 'Coffee Tour Tradicional',
 'Un recorrido inmersivo por nuestra finca. Aprenda sobre el proceso del café desde la semilla hasta la taza, caminando por los cafetales, visitando el beneficio húmedo y terminando con una degustación. Mínimo 2 pax. Nivel: Fácil.',
 120, 15000.00, 15,
 'https://images.unsplash.com/photo-1611080626919-7cf5a9dbab5b?w=800&q=80', 1, GETDATE()),
('D0000001-0000-0000-0000-000000000002', 'Cata de Café Premium',
 'Descubra los perfiles de sabor de nuestros mejores microlotes. Guiado por nuestro barista experto, aprenderá a identificar notas de cata, fragancias y aromas utilizando protocolos profesionales. Mínimo 1 pax. Incluye certificado.',
 90, 22000.00, 10,
 'https://images.unsplash.com/photo-1447933601403-0c6688de566e?w=800&q=80', 1, GETDATE());
GO

INSERT INTO Roles (Id, Nombre, Descripcion)
SELECT v.Id, v.Nombre, v.Descripcion
FROM (VALUES
    ('A0000000-0000-0000-0000-000000000001', 'Administrador', 'Acceso total al panel de administracion'),
    ('A0000000-0000-0000-0000-000000000002', 'Empleado',      'Atiende pedidos y gestiona el catalogo'),
    ('A0000000-0000-0000-0000-000000000003', 'Cliente',       'Compra en la tienda en linea')
) AS v (Id, Nombre, Descripcion)
WHERE NOT EXISTS (SELECT 1 FROM Roles r WHERE r.Nombre = v.Nombre);
GO

-- Contrasena de todas las cuentas de prueba: Isme2026*
INSERT INTO Usuarios (Id, Nombre, Apellidos, Correo, Telefono, Contrasena, IdRol, Activo, FechaCreacion)
SELECT v.Id, v.Nombre, v.Apellidos, v.Correo, v.Telefono, v.Contrasena,
       (SELECT r.Id FROM Roles r WHERE r.Nombre = v.NombreRol),
       1, GETDATE()
FROM (VALUES
    ('E0000000-0000-0000-0000-000000000001', 'Ana',    'Rojas Vargas',   'ana.rojas@ismecafe.cr',    '8888-0001', '$2a$11$yDF60H15fGzRoi.uyNTgl.jwO2GSWo2usUKEAQ71DBs1/WDd0Pa0.', 'Administrador'),
    ('E0000000-0000-0000-0000-000000000002', 'Bruno',  'Mora Jimenez',   'bruno.mora@ismecafe.cr',   '8888-0002', '$2a$11$yDF60H15fGzRoi.uyNTgl.jwO2GSWo2usUKEAQ71DBs1/WDd0Pa0.', 'Empleado'),
    ('E0000000-0000-0000-0000-000000000003', 'Carla',  'Solis Ureña',    'carla.solis@ismecafe.cr',  '8888-0003', '$2a$11$yDF60H15fGzRoi.uyNTgl.jwO2GSWo2usUKEAQ71DBs1/WDd0Pa0.', 'Empleado'),
    ('F0000000-0000-0000-0000-000000000001', 'Diego',  'Castro Leon',    'diego.castro@correo.cr',   '7777-0001', '$2a$11$yDF60H15fGzRoi.uyNTgl.jwO2GSWo2usUKEAQ71DBs1/WDd0Pa0.', 'Cliente'),
    ('F0000000-0000-0000-0000-000000000002', 'Elena',  'Nunez Pacheco',  'elena.nunez@correo.cr',    '7777-0002', '$2a$11$yDF60H15fGzRoi.uyNTgl.jwO2GSWo2usUKEAQ71DBs1/WDd0Pa0.', 'Cliente')
) AS v (Id, Nombre, Apellidos, Correo, Telefono, Contrasena, NombreRol)
WHERE NOT EXISTS (SELECT 1 FROM Usuarios u WHERE u.Correo = v.Correo)
  AND EXISTS     (SELECT 1 FROM Roles    r WHERE r.Nombre = v.NombreRol);
GO


DECLARE @Ana    UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000001';
DECLARE @Bruno  UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000002';
DECLARE @Carla  UNIQUEIDENTIFIER = 'E0000000-0000-0000-0000-000000000003';
DECLARE @Diego  UNIQUEIDENTIFIER = 'F0000000-0000-0000-0000-000000000001';
DECLARE @Elena  UNIQUEIDENTIFIER = 'F0000000-0000-0000-0000-000000000002';

-- Cabeceras. El Total se recalcula mas abajo a partir del detalle.
INSERT INTO Pedidos (Id, IdUsuario, IdEmpleado, Fecha, Estado, Total) VALUES
('AA000000-0000-0000-0000-000000000001', @Diego, @Bruno, DATEADD(DAY,  -1, GETDATE()), 'Entregado',  0),
('AA000000-0000-0000-0000-000000000002', @Elena, @Bruno, DATEADD(DAY,  -3, GETDATE()), 'Entregado',  0),
('AA000000-0000-0000-0000-000000000003', @Diego, @Bruno, DATEADD(DAY,  -8, GETDATE()), 'Entregado',  0),
('AA000000-0000-0000-0000-000000000004', @Elena, @Carla, DATEADD(DAY,  -2, GETDATE()), 'Entregado',  0),
('AA000000-0000-0000-0000-000000000005', @Diego, @Carla, DATEADD(DAY, -10, GETDATE()), 'Entregado',  0),
('AA000000-0000-0000-0000-000000000006', @Elena, @Ana,   DATEADD(DAY,  -5, GETDATE()), 'Entregado',  0),
-- Cancelado: NO debe contarse como venta en el reporte.
('AA000000-0000-0000-0000-000000000007', @Diego, @Bruno, DATEADD(DAY,  -4, GETDATE()), 'Cancelado',  0),
-- Autoservicio (sin empleado): aparece agrupado como "Sin asignar".
('AA000000-0000-0000-0000-000000000008', @Elena, NULL,   DATEADD(DAY,  -6, GETDATE()), 'Pendiente',  0);
GO

DECLARE @Cafe       UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000001';
DECLARE @Espresso   UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000002';
DECLARE @Latte      UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000003';
DECLARE @Cheesecake UNIQUEIDENTIFIER = 'C0000001-0000-0000-0000-000000000004';

INSERT INTO DetallePedidos (Id, IdPedido, IdProducto, Cantidad, PrecioUnitario, Subtotal)
SELECT NEWID(), v.IdPedido, v.IdProducto, v.Cantidad, p.Precio, p.Precio * v.Cantidad
FROM (VALUES
    ('AA000000-0000-0000-0000-000000000001', @Cafe,        2),
    ('AA000000-0000-0000-0000-000000000001', @Latte,       1),
    ('AA000000-0000-0000-0000-000000000002', @Espresso,    1),
    ('AA000000-0000-0000-0000-000000000003', @Cafe,        3),
    ('AA000000-0000-0000-0000-000000000003', @Cheesecake,  2),
    ('AA000000-0000-0000-0000-000000000004', @Latte,       4),
    ('AA000000-0000-0000-0000-000000000005', @Espresso,    2),
    ('AA000000-0000-0000-0000-000000000005', @Cafe,        1),
    ('AA000000-0000-0000-0000-000000000006', @Cheesecake,  3),
    ('AA000000-0000-0000-0000-000000000007', @Cafe,        5),
    ('AA000000-0000-0000-0000-000000000008', @Latte,       2)
) AS v (IdPedido, IdProducto, Cantidad)
JOIN Productos p ON p.Id = v.IdProducto;
GO

-- Cuadrar el total de cada pedido con su detalle.
UPDATE pe
   SET pe.Total = ISNULL(d.Suma, 0)
  FROM Pedidos pe
  LEFT JOIN (
        SELECT IdPedido, SUM(Subtotal) AS Suma
          FROM DetallePedidos
         GROUP BY IdPedido
  ) d ON d.IdPedido = pe.Id;
GO

