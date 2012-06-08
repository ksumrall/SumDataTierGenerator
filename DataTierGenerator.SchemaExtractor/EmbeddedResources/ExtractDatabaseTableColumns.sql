SELECT SCHEMA_NAME(t.schema_id) AS schema_name
    ,t.[name] AS table_name
    ,c.[name] AS column_name
    ,c.[column_id]
    ,TYPE_NAME(c.user_type_id) AS [data_type]
    ,c.[max_length]
    ,c.[precision]
    ,c.[scale]
    ,c.[collation_name]
    ,c.[is_nullable]
    ,c.[is_ansi_padded]
    ,c.[is_rowguidcol]
    ,c.[is_identity]
    ,c.[is_computed]
    ,c.[is_filestream]
    ,c.[is_replicated]
    ,c.[is_non_sql_subscribed]
    ,c.[is_merge_published]
    ,c.[is_dts_replicated]
    ,c.[is_xml_document]
    ,c.[xml_collection_id]
    ,c.[default_object_id]
    ,c.[rule_object_id]
    ,c.[is_sparse]
    ,c.[is_column_set]
    ,ep.value AS [Description]
FROM sys.columns c
INNER JOIN sys.tables t ON c.object_id = t.object_id
LEFT JOIN sys.extended_properties ep ON c.object_id = ep.major_id
    AND c.column_id = ep.minor_id
    AND ep.NAME = 'MS_Description'
