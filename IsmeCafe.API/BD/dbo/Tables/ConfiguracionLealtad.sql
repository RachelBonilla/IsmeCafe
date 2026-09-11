CREATE TABLE [dbo].[ConfiguracionLealtad] (
    [Id]                 UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [PuntosPorMonto]     DECIMAL (18, 2)  NOT NULL,
    [FechaActualizacion] DATETIME         NULL,
    [ValorPunto]         DECIMAL (18, 2)  DEFAULT ((5)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

