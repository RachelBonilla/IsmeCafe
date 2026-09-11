CREATE TABLE [dbo].[Categorias] (
    [Id]          UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [Nombre]      VARCHAR (50)   NOT NULL,
    [Descripcion] VARCHAR (200)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
