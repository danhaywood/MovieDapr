CREATE TABLE [dbo].[Actor] (
       [ID]          INT             IDENTITY (1, 1) NOT NULL,
       [Name]       NVARCHAR (MAX)  NOT NULL,
       CONSTRAINT [PK_Actor] PRIMARY KEY CLUSTERED ([ID] ASC)
);

