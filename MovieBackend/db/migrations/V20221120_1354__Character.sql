CREATE TABLE [dbo].[Character] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [MovieID]   INT NOT NULL,
    [ActorID]   INT NOT NUll,
    [CharacterName] VARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_Character] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Character_Movie] FOREIGN KEY (MovieID) REFERENCES [dbo].[Movie] (ID),
    CONSTRAINT [FK_Character_Actor] FOREIGN KEY (ActorID) REFERENCES [dbo].[Actor] (ID)
);
