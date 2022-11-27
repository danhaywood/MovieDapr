CREATE TABLE [dbo].[Movie] (
       [Id]          INT             IDENTITY (1, 1) NOT NULL,
       [Title]       NVARCHAR (MAX)  NOT NULL,
       [ReleaseDate] DATETIME2 (7)   NOT NULL,
       [Genre]       NVARCHAR (MAX)  NOT NULL,
       [Price]       DECIMAL (18, 2) NOT NULL,
       CONSTRAINT [PK_Movie] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

