CREATE TABLE [dbo].[CampanasMarketing] (
    [Id]                    UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Tipo]                  VARCHAR (20)     NOT NULL,
    [IdOferta]              UNIQUEIDENTIFIER NULL,
    [IdDescuento]           UNIQUEIDENTIFIER NULL,
    [Asunto]                VARCHAR (200)    NOT NULL,
    [Contenido]             VARCHAR (MAX)    NULL,
    [FechaEnvio]            DATETIME         NULL,
    [CantidadDestinatarios] INT              DEFAULT ((0)) NOT NULL,
    [Exitosa]               BIT              DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdDescuento]) REFERENCES [dbo].[Descuentos] ([Id]),
    FOREIGN KEY ([IdOferta]) REFERENCES [dbo].[Ofertas] ([Id])
);

