SELECT SCHEMA_NAME(schema_id) AS schema_name
    ,NAME AS view_name
    ,create_date
    ,modify_date
FROM sys.views;
