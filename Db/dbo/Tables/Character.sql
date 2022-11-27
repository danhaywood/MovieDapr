CREATE TABLE [dbo].[Character](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [MovieId] [int] NOT NULL,
  [ActorId] [int] NOT NULL,
  [CharacterName] [varchar](max) NOT NULL,
  CONSTRAINT [PK_Character] PRIMARY KEY CLUSTERED ([ID] ASC)
)
GO

ALTER TABLE [dbo].[Character]  WITH CHECK ADD  CONSTRAINT [FK_Character_Actor] FOREIGN KEY([ActorID])
    REFERENCES [dbo].[Actor] ([Id])
GO

ALTER TABLE [dbo].[Character] CHECK CONSTRAINT [FK_Character_Actor]
GO

ALTER TABLE [dbo].[Character]  WITH CHECK ADD  CONSTRAINT [FK_Character_Movie] FOREIGN KEY([MovieID])
    REFERENCES [dbo].[Movie] ([Id])
GO

ALTER TABLE [dbo].[Character] CHECK CONSTRAINT [FK_Character_Movie]
GO

