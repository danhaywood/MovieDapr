EXECUTE sp_addextendedproperty
        'description'  -- Textual description of what this table/column is used for.
    ,'unique identifier of a Title'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'gdpr_category' --  GDPR categorisation: Personal (personal data), Special (sensitive data), None (no GDPR impact)
    ,'None'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_physical_data' --  Whether holds information around physical characteristics of the individual (e.g. Height, Hair Colour etc.)
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_photographic_data' --  Whether holds photographic data.  The processing of photographs should not systematically be considered to be processing of special categories of personal data as they are covered by the definition of biometric data only when processed through a specific technical means allowing the unique identification or authentication of a natural person
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_name_data' -- as per https://gdpr-info.eu/art-4-gdpr/[GDPR art. 4 (1)]: whether this identifies a natural person, directly or indirectly, by their name
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_identification_number_data' -- as per https://gdpr-info.eu/art-4-gdpr/[GDPR art. 4 (1)]: whether this identifies a natural person, directly or indirectly, by an identification number
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_identification_data_data' -- as per https://gdpr-info.eu/art-4-gdpr/[GDPR art. 4 (1)]: whether this identifies a natural person, directly or indirectly, by location data ; e.g. Address, GPS coordinates etc.
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_online_identifier_data' -- as per https://gdpr-info.eu/recitals/no-30/[GDPR recital 30]: whether this identifies a natural persons through an *online identifier* provided by their devices, applications, tools and protocols, such as internet protocol addresses, cookie identifiers or other identifiers such as radio frequency identification tags.
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_racial_ethnic_data'   -- as per https://gdpr-info.eu/recitals/no-51/[GDPR recital 51]: personal data revealing *racial or ethnic* origin 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_political_opinion_data' -- as per https://gdpr-info.eu/recitals/no-56/[GDPR recital 56]: whether this holds data on a person's *political opinions*
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_belief_system_data'   -- as per https://gdpr-info.eu/recitals/no-75/[GDPR recital 75]: whether this hold data on a person's "religion or philosophical beliefs"
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_trade_union_data'  -- as per https://gdpr-info.eu/art-9-gdpr/[GDPR art. 9] and elsewhere, whether this indicates membership thereof 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_genetic_data'  -- as per https://gdpr-info.eu/recitals/no-34/[GDPR recital 34]: whether this holds personal data relating to the inherited or acquired genetic characteristics of a natural person which give unique information about the physiology or the health of that natural person 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_biometric_data'  -- as per https://gdpr-info.eu/art-4-gdpr/[GDPR art. 4 (14)]: whet this holds personal data resulting from specific *technical processing* relating to the physical, physiological or behavioural characteristics of a natural person; eg facial images or fingerprints 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_medical_data'  -- as per https://gdpr-info.eu/art-4-gdpr/[GDPR art. 4 (15)]: whether this holds data related to the physical or mental health of a natural person, including the provision of health care services 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_sexual_orientation_data'  -- unlikely to be stored directly, may be "inferred" (incorrectly) from other information e.g. "Civil Partner" 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_minor_related_data'  -- as per https://gdpr-info.eu/recitals/no-38/[GDPR recital 38]: 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_system_generated'  -- Generated by the database directly, e.g. `DEFAULT` values, `IDENTITY` values etc.  It should not be assumed there is no inherent information in these e.g. children of the same age may be assigned a `customer_id` in a similar range
    ,'Yes'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_employee_related'  -- e.g. `last_updated_by` column 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_financial_related'  -- e.g. information about the earnings of a customer (e.g. Means Assessments) 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_financial_fraud_target'  -- whether this holds information that may be useful to commit fraud against this customer (e.g. account details) 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_surrogate_identifier'  -- whether this is an identifier that has a one to one relationship to a customer (e.g. `customer_id`, psc card number) 
    ,'Yes'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_free_text'  -- where the user has the freedom to enter any value into the column - unstructured data may be contained within 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
EXECUTE sp_addextendedproperty
        'is_lookup_table'  -- identifies that this table is only used to store values. 
    ,'No'
    , 'SCHEMA', 'dbo'
    , 'TABLE', 'Movie'
    , 'COLUMN', 'Id'
go
