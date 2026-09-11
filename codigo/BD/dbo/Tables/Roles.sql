CREATE TABLE [dbo].[Roles] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Nombre]      VARCHAR (50)     NOT NULL,
    [Descripcion] VARCHAR (250)    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

