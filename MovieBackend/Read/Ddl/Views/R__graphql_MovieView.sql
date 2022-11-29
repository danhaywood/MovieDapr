CREATE OR ALTER VIEW [graphql].[MovieView] AS
    SELECT 
         Id as Id
        ,[Title]       As [Title] 
        ,[ReleaseDate] As [ReleaseDate]
        ,[Genre]       As [Genre] 
        ,[Price]       As [Price]
FROM dbo.Movie
go



