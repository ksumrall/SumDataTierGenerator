SELECT SCHEMA_NAME(schema_id) AS schema_name
    ,name AS function_name
    ,type_desc
    ,create_date
    ,modify_date
FROM sys.objects
WHERE type_desc LIKE '%FUNCTION%';
