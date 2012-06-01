DECLARE @Schema VARCHAR(128)

DECLARE SchemaCursor CURSOR FOR
    SELECT  DISTINCT
	    TABLE_SCHEMA
    FROM
	    INFORMATION_SCHEMA.TABLES
    WHERE
	    TABLE_TYPE = 'BASE TABLE'
	    AND TABLE_NAME != 'dtProperties'
	    --AND TABLE_CATALOG = '#DatabaseName#'

CREATE TABLE #Temp
    (
    TABLE_CATALOG VARCHAR(128),
    TABLE_SCHEMA VARCHAR(128),
    TABLE_NAME VARCHAR(128),
	TABLE_TYPE VARCHAR(128),
    Description VARCHAR(128)
    )

OPEN SchemaCursor

FETCH NEXT
FROM SchemaCursor
INTO @Schema

WHILE (@@FETCH_STATUS = 0)
BEGIN
    
    INSERT INTO #Temp
        (
        TABLE_CATALOG,
	    TABLE_SCHEMA,
	    TABLE_NAME,
	    TABLE_TYPE,
        Description
        )
        SELECT
	        TABLE_CATALOG,
	        TABLE_SCHEMA,
	        TABLE_NAME,
	        TABLE_TYPE,
	        CAST(exDescription.Value AS VARCHAR(128)) AS Description
        FROM
	        INFORMATION_SCHEMA.TABLES
	        LEFT JOIN ::FN_LISTEXTENDEDPROPERTY('MS_Description', 'user', @Schema, 'table', default, default, default) AS exDescription 
	            ON INFORMATION_SCHEMA.TABLES.TABLE_NAME collate SQL_Latin1_General_CP1_CI_AS = exDescription.objName collate SQL_Latin1_General_CP1_CI_AS
        WHERE
	        TABLE_TYPE = 'BASE TABLE'
	        AND TABLE_SCHEMA = @Schema
	        AND TABLE_NAME != 'dtProperties'
	        --AND TABLE_CATALOG = '#DatabaseName#'
    
    FETCH NEXT
    FROM SchemaCursor
    INTO @Schema
    
END

CLOSE SchemaCursor
DEALLOCATE SchemaCursor

SELECT * FROM #Temp

DROP TABLE #Temp
