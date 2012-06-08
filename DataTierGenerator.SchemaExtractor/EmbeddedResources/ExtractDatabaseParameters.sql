SELECT SCHEMA_NAME(schema_id) AS schema_name
    ,o.NAME AS procedure_name
    ,o.type_desc
    ,p.parameter_id
    ,p.NAME AS parameter_name
    ,TYPE_NAME(p.user_type_id) AS parameter_type
    ,p.max_length
    ,p.precision
    ,p.scale
    ,p.is_output
FROM sys.objects AS o
INNER JOIN sys.parameters AS p ON o.object_id = p.object_id
ORDER BY schema_name
    ,o.NAME
    ,p.parameter_id;
