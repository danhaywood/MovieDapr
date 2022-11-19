EXECUTE sp_addextendedproperty @name = N'gdpr_category', @value = N'sensitive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Movie', @level2type = N'COLUMN', @level2name = N'Title';


GO
EXECUTE sp_addextendedproperty @name = N'gdpr_brand', @value = N'whatever', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Movie', @level2type = N'COLUMN', @level2name = N'Title';


GO
