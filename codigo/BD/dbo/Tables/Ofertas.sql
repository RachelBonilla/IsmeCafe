CREATE TABLE [dbo].[Ofertas] (
    [Id]                 UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Nombre]             VARCHAR (150)    NOT NULL,
    [Descripcion]        VARCHAR (500)    NULL,
    [TipoOferta]         VARCHAR (20)     NOT NULL,
    [PrecioCombo]        DECIMAL (18, 2)  NULL,
    [FechaInicio]        DATETIME         NOT NULL,
    [FechaFin]           DATETIME         NOT NULL,
    [Activo]             BIT              NOT NULL,
    [FechaCreacion]      DATETIME         NULL,
    [FechaActualizacion] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

