BEGIN
    DECLARE @TableName TABLE (RowID INT NOT NULL identity(1, 1) PRIMARY KEY, NAME NVARCHAR(100));

    SET NOCOUNT ON
    
    INSERT INTO @TableName (NAME)
    SELECT TABLE_NAME
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_TYPE = 'BASE TABLE';

    DECLARE @i INT;

    SELECT @i = MIN(RowID)
    FROM @TableName

    DECLARE @max INT

    SELECT @max = MAX(RowID)
    FROM @TableName

    DECLARE @name NVARCHAR(100);

    WHILE @i <= @max
    BEGIN
        SELECT @name = NAME
        FROM @TableName
        WHERE RowID = @i;

        EXEC sp_makeCRUD @name, 1, 1, 1, 'usp_zzz_autogen_crud__'

        SET @i = @i + 1;
    END
END
