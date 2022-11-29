CREATE or ALTER VIEW [graphql].[CharacterView] AS
SELECT
    [Id]            as [Id]
    ,[MovieId]       as [MovieId]       
    ,[ActorId]       as [ActorId]      
    ,[CharacterName] as [CharacterName] 
FROM dbo.Character
go
