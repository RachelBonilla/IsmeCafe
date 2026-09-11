CREATE TABLE [dbo].[Servicios] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (NEWID()) NOT NULL,
    [Nombre]          VARCHAR(100)    NOT NULL,
    [Descripcion]     VARCHAR(500)    NOT NULL,
    [Duracion]        INT             NOT NULL 
                        CONSTRAINT CK_Servicios_Duracion CHECK (Duracion > 0),
    [Precio]          DECIMAL(18,2)   NOT NULL 
                        CONSTRAINT CK_Servicios_Precio CHECK (Precio >= 0),
    [CupoMaximo]      INT             NOT NULL
                        CONSTRAINT CK_Servicios_Cupo CHECK (CupoMaximo > 0),
    [Imagen]          VARCHAR(300)    NULL,
    [Activo]          BIT             NOT NULL 
                        CONSTRAINT DF_Servicios_Activo DEFAULT (1),
    [FechaCreacion]   DATETIME        NOT NULL 
                        CONSTRAINT DF_Servicios_FechaCreacion DEFAULT (GETDATE()),
    PRIMARY KEY CLUSTERED ([Id] ASC)
);