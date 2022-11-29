CREATE TABLE [dbo].[Character](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [MovieId] [int] NOT NULL,
  [ActorId] [int] NOT NULL,
  [CharacterName] [varchar](max) NOT NULL,
  CONSTRAINT [PK_Character] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

ALTER TABLE [dbo].[Character]  ADD  CONSTRAINT [FK_Character_Actor] FOREIGN KEY([ActorId])
    REFERENCES [dbo].[Actor] ([Id])
GO

ALTER TABLE [dbo].[Character] CHECK CONSTRAINT [FK_Character_Actor]
GO

ALTER TABLE [dbo].[Character]  ADD  CONSTRAINT [FK_Character_Movie] FOREIGN KEY([MovieId])
    REFERENCES [dbo].[Movie] ([Id])
GO

ALTER TABLE [dbo].[Character] CHECK CONSTRAINT [FK_Character_Movie]
GO

