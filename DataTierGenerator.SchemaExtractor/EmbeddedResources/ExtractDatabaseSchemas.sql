SELECT *
FROM sys.schemas
WHERE NAME NOT LIKE 'db\_%' ESCAPE '\'
    AND NAME NOT IN (
        'guest'
        ,'sys'
        ,'INFORMATION_SCHEMA'
        )
