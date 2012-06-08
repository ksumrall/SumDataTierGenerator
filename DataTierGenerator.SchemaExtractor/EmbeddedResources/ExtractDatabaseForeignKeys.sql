SELECT SCHEMA_NAME(t.schema_id) AS schema_name
    ,OBJECT_NAME(f.parent_object_id) AS table_name
    ,f.NAME AS foreign_key_name
    ,COL_NAME(fc.parent_object_id, fc.parent_column_id) AS constraint_column_name
    ,fc.constraint_column_id
    ,OBJECT_NAME(f.referenced_object_id) AS referenced_object
    ,COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS referenced_column_name
    ,is_disabled
    ,delete_referential_action_desc
    ,update_referential_action_desc
FROM sys.foreign_keys AS f
INNER JOIN sys.tables t ON f.parent_object_id = t.object_id
INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
ORDER BY schema_name
    ,table_name
    ,foreign_key_name
    ,constraint_column_id
