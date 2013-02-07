SELECT TABLE_NAME
    ,COALESCE(DATE_CREATED, GETDATE()) DATE_CREATED
    ,COALESCE(DATE_MODIFIED, GETDATE()) DATE_MODIFIED
FROM INFORMATION_SCHEMA.TABLES;