CREATE VIEW [graphql].[CharacterData] AS
SELECT
    [Id]            as [Id]
    ,[MovieId]       as [MovieId]       
    ,[ActorId]       as [ActorId]      
    ,[CharacterName] as [CharacterName] 
FROM dbo.Character
go
