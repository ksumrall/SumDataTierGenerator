SELECT SCHEMA_NAME(t.schema_id) AS [schema_name]
    ,t.NAME table_name
    ,i.NAME AS index_name
    ,c.NAME AS column_name
    ,key_ordinal
FROM sys.indexes AS i
INNER JOIN sys.index_columns AS ic ON i.object_id = ic.object_id
    AND i.index_id = ic.index_id
INNER JOIN sys.tables t ON i.object_id = t.object_id
INNER JOIN sys.columns AS c ON ic.object_id = c.object_id
    AND c.column_id = ic.column_id
WHERE i.is_primary_key = 1
    AND i.is_disabled = 0
