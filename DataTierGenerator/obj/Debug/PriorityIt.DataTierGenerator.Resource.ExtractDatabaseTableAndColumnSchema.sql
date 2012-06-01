
DECLARE @SchemaId INT
DECLARE @SchemaName VARCHAR(128)
DECLARE @TableId INT
DECLARE @TableName VARCHAR(128)

DECLARE SchemaCursor CURSOR FOR
    SELECT  DISTINCT
	    schema_id
		, name
    FROM
	    sys.schemas

CREATE TABLE #Temp
    (
    [SchemaName] NVARCHAR(128)
    , [TableName] NVARCHAR(128)
    , [ColumnName] NVARCHAR(128)
	, [ColumnId] INT
    , [DataType] NVARCHAR(128)
    , [DataTypeSchema] NVARCHAR(128)
    , [SystemType] NVARCHAR(128)
    , [Length] INT
    , [NumericPrecision] INT
    , [NumericScale] INT
    , [IsNullable] BIT
    , [IsRowGuid] BIT
    , [IsIdentity] BIT
    , [IdentitySeed] BIGINT
    , [IdentityIncrement] BIGINT
    , [IsComputed] BIT
    , [DefaultValue] NVARCHAR(128)
    , [Formular] NVARCHAR(MAX)
    , [TableDescription] NVARCHAR(128)
    , [ColumnDescription] NVARCHAR(128)
    )

OPEN SchemaCursor

FETCH NEXT
FROM SchemaCursor
INTO @SchemaId, @SchemaName
WHILE (@@FETCH_STATUS = 0)
BEGIN

	DECLARE TableCursor CURSOR FOR
		SELECT
			sys.tables.object_id
			, sys.tables.name
		FROM
			sys.schemas INNER JOIN sys.tables ON sys.schemas.schema_id = sys.tables.schema_id
		WHERE
			sys.tables.type = 'U'
			AND sys.schemas.name = @SchemaName
			AND sys.tables.name != 'dtProperties'

	OPEN TableCursor
    
	FETCH NEXT
	FROM TableCursor
	INTO @TableId, @TableName

	WHILE (@@FETCH_STATUS = 0)
	BEGIN

		INSERT INTO #Temp
			(
			[SchemaName]
			, [TableName]
			, [ColumnName]
			, [ColumnId]
			, [DataType]
			, [DataTypeSchema]
			, [SystemType]
			, [Length]
			, [NumericPrecision]
			, [NumericScale]
			, [IsNullable]
			, [IsRowGuid]
			, [IsIdentity]
			, [IdentitySeed]
			, [IdentityIncrement]
			, [IsComputed]
			, [DefaultValue]
			, [Formular]
			, [TableDescription]
			, [ColumnDescription]
			)
		SELECT
			@SchemaName
			, @TableName
			, clmns.name AS [ColumnName]
			, clmns.column_id AS [ColumnId]
			, usrt.name AS [DataType]
			, sclmns.name AS [DataTypeSchema]
			, ISNULL(baset.name, N'') AS [SystemType]
			, CAST(CASE WHEN baset.name IN (N'nchar', N'nvarchar') AND clmns.max_length <> -1 THEN clmns.max_length/2 ELSE clmns.max_length END AS INT) AS [Length]
			, CAST(clmns.precision AS INT) AS [NumericPrecision]
			, CAST(clmns.scale AS INT) AS [NumericScale]
			, clmns.is_nullable AS [IsNullable]
			, clmns.is_rowguidcol AS [IsRowGuid]
			, clmns.is_identity AS [IsIdentity]
			, CAST(ISNULL(ic.seed_value,0) AS BIGINT) AS [IdentitySeed]
			, CAST(ISNULL(ic.increment_value,0) AS BIGINT) AS [IdentityIncrement]
			, clmns.is_computed AS [IsComputed]
			, CASE WHEN (clmns.default_object_id IS NULL) THEN NULL ELSE object_definition(clmns.default_object_id) END AS [DefaultValue]
			, CASE WHEN (cmc.column_id IS NULL) THEN NULL ELSE cmc.definition END AS [Formular]
			, CAST(TableDescription.Value AS NVARCHAR(128))
			, CAST(ColumnDescription.Value AS NVARCHAR(128))
		FROM
			sys.tables AS tbl
			INNER JOIN sys.columns AS clmns ON clmns.object_id = tbl.object_id
			LEFT OUTER JOIN sys.types AS usrt ON usrt.user_type_id = clmns.user_type_id
			LEFT OUTER JOIN sys.schemas AS sclmns ON sclmns.schema_id = usrt.schema_id
			LEFT OUTER JOIN sys.types AS baset ON baset.user_type_id = clmns.system_type_id AND baset.user_type_id = baset.system_type_id
			LEFT OUTER JOIN sys.identity_columns AS ic ON ic.object_id = clmns.object_id AND ic.column_id = clmns.column_id
			LEFT OUTER JOIN sys.computed_columns AS cmc ON cmc.object_id = clmns.object_id AND cmc.column_id = clmns.column_id
			LEFT OUTER JOIN ::fn_listextendedproperty('MS_Description', 'SCHEMA', @SchemaName, 'TABLE', @TableName, default, default) AS TableDescription 
				ON tbl.name collate SQL_Latin1_General_CP1_CI_AS = TableDescription.objName collate SQL_Latin1_General_CP1_CI_AS
			LEFT OUTER JOIN ::fn_listextendedproperty('MS_Description', 'SCHEMA', @SchemaName, 'TABLE', @TableName, 'COLUMN', default) AS ColumnDescription
				ON clmns.name collate SQL_Latin1_General_CP1_CI_AS = ColumnDescription.objName collate SQL_Latin1_General_CP1_CI_AS
		WHERE
			tbl.object_id = @TableId

		FETCH NEXT
		FROM TableCursor
		INTO @TableId, @TableName
	    
	END

	CLOSE TableCursor
	DEALLOCATE TableCursor

    FETCH NEXT
    FROM SchemaCursor
    INTO @SchemaId, @SchemaName
    
END

CLOSE SchemaCursor
DEALLOCATE SchemaCursor

SELECT *
FROM #Temp
ORDER BY [SchemaName] ASC, [TableName] ASC, [ColumnId] ASC

DROP TABLE #Temp
