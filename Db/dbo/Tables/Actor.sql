CREATE TABLE [dbo].[Actor](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Name] [nvarchar](max) NOT NULL,
  CONSTRAINT [PK_Actor] PRIMARY KEY CLUSTERED ([Id] ASC)
);

go