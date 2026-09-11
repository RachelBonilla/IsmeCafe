CREATE TABLE [dbo].[Descuentos] (
    [Id]                  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [IdProducto]          UNIQUEIDENTIFIER NOT NULL,
    [PorcentajeDescuento] DECIMAL (5, 2)   NOT NULL,
    [FechaInicio]         DATETIME         NOT NULL,
    [FechaFin]            DATETIME         NOT NULL,
    [Activo]              BIT              NOT NULL,
    [FechaCreacion]       DATETIME         NULL,
    [FechaActualizacion]  DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Descuentos_Productos] FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[Productos] ([Id])
);

