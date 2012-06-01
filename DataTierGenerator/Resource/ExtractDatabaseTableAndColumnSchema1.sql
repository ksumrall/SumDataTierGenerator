-- This file is being preserved because it is compact and may be of use in the future. It was abandoned due to 
-- it's inability to work on databases with compatability level set to 80
SELECT
    SCHEMA_NAME(tbl.schema_id) AS [SchemaName]
    , tbl.name AS [TableName]
    , clmns.name AS [ColumnName]
    , clmns.column_id AS [ColumnId]
    , usrt.name AS [DataType]
    , sclmns.name AS [DataTypeSchema]
    , ISNULL(baset.name, N'') AS [SystemType]
    , CAST(CASE WHEN baset.name IN (N'nchar', N'nvarchar') AND clmns.max_length <> -1 THEN clmns.max_length/2 ELSE clmns.max_length END AS int) AS [Length]
    , CAST(clmns.precision AS int) AS [NumericPrecision]
    , CAST(clmns.scale AS int) AS [NumericScale]
    , clmns.is_nullable AS [IsNullable]
    , clmns.is_rowguidcol AS [IsRowGuid]
    , clmns.is_identity AS [IsIdentity]
    , CAST(ISNULL(ic.seed_value,0) AS bigint) AS [IdentitySeed]
    , CAST(ISNULL(ic.increment_value,0) AS bigint) AS [IdentityIncrement]
    , clmns.is_computed AS [IsComputed]
    , CASE WHEN (clmns.default_object_id IS NULL) THEN NULL ELSE object_definition(clmns.default_object_id) END AS [DefaultValue]
    , CASE WHEN (cmc.column_id IS NULL) THEN NULL ELSE cmc.definition END AS formular
    , (SELECT value FROM fn_listextendedproperty('MS_Description', 'SCHEMA', SCHEMA_NAME(tbl.schema_id), 'TABLE', tbl.Name, NULL, NULL)) [TableDescription]
    , (select value FROM fn_listextendedproperty('MS_Description', 'SCHEMA', SCHEMA_NAME(tbl.schema_id), 'TABLE', tbl.Name, 'COLUMN', clmns.Name)) [ColumnDescription]
FROM
    sys.tables AS tbl
    INNER JOIN sys.columns AS clmns ON clmns.object_id=tbl.object_id
    LEFT OUTER JOIN sys.types AS usrt ON usrt.user_type_id = clmns.user_type_id
    LEFT OUTER JOIN sys.schemas AS sclmns ON sclmns.schema_id = usrt.schema_id
    LEFT OUTER JOIN sys.types AS baset ON baset.user_type_id = clmns.system_type_id AND baset.user_type_id = baset.system_type_id
    LEFT OUTER JOIN sys.identity_columns AS ic ON ic.object_id = clmns.object_id AND ic.column_id = clmns.column_id
    LEFT OUTER JOIN sys.computed_columns AS cmc ON cmc.object_id = clmns.object_id AND cmc.column_id = clmns.column_id
ORDER BY
	[SchemaName] ASC, [TableName] ASC,[ColumnId] ASC
