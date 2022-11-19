EXECUTE sp_addextendedproperty @name = N'gdpr_category', @value = N'not_sensitive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Movie', @level2type = N'COLUMN', @level2name = N'ReleaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'gdpr_brand', @value = N'whatever_2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Movie', @level2type = N'COLUMN', @level2name = N'ReleaseDate';

