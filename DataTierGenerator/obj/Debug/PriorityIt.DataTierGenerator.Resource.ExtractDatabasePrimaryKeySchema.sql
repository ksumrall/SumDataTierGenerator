SELECT
    SCHEMA_NAME(tbl.schema_id) AS [SchemaName]
    , tbl.name AS [TableName]
    , i.name AS [IndexName]
    , clmns.name AS [ColumnName]
    , ic.key_ordinal AS [Ordinal]
FROM
    sys.tables AS tbl
    INNER JOIN sys.indexes AS i ON (i.index_id > 0 and i.is_hypothetical = 0) AND (i.object_id=tbl.object_id)
    INNER JOIN sys.index_columns AS ic ON (ic.column_id > 0 and (ic.key_ordinal > 0 or ic.partition_ordinal = 0)) AND (ic.index_id=CAST(i.index_id AS int) AND 
    ic.object_id=i.object_id)
    INNER JOIN sys.columns AS clmns ON clmns.object_id = ic.object_id and clmns.column_id = ic.column_id
WHERE i.is_primary_key = 1 AND i.is_disabled = 0
ORDER BY
    [SchemaName] ASC, [TableName] ASC, [IndexName], [Ordinal] ASC
