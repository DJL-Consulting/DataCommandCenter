USE B2B_Datamart
GO

--truncate table DataCommandCenter.meta.SQL_Column

insert into DataCommandCenter.meta.SQL_Column
([ObjectID], [ColumnName], [DataType], [MaxLength], [Precision], [Scale], [Nullable], [PrimaryKey], OrdinalPosition)

SELECT 
	(select ID from DataCommandCenter.meta.SQL_Object where DatabaseID = (select ID from DataCommandCenter.meta.SQL_Database where DatabaseName = DB_NAME() AND ServerID = (select ID from DataCommandCenter.meta.Server where ServerName = @@SERVERNAME)) AND SchemaName = s.name AND ObjectName = o.Name) as ObjectID,
	--o.name as ObjectName,
	--s.name as SchemaName,
    c.name as ColumnName,
    t.Name as DataType,
    c.max_length as MaxLength,
    c.Precision ,
    c.Scale ,
    c.is_nullable as Nullable,
    ISNULL(i.is_primary_key, 0) as PrimaryKey,
	c.column_id as OrdinalPosition
FROM    
    sys.columns c
INNER JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
LEFT OUTER JOIN sys.index_columns ic 
	ON ic.object_id = c.object_id AND ic.column_id = c.column_id
LEFT OUTER JOIN sys.indexes i 
	ON ic.object_id = i.object_id AND ic.index_id = i.index_id
left outer join sys.objects o
	on o.object_id = c.object_id
left outer join sys.schemas s
      ON  s.schema_id = o.schema_id
   WHERE  o.type NOT IN ('S'  --SYSTEM_TABLE
                        ,'PK' --PRIMARY_KEY_CONSTRAINT
                        ,'D'  --DEFAULT_CONSTRAINT
                        ,'C'  --CHECK_CONSTRAINT
                        ,'F'  --FOREIGN_KEY_CONSTRAINT
                        ,'IT' --INTERNAL_TABLE
                        ,'SQ' --SERVICE_QUEUE
                        ,'TR' --SQL_TRIGGER
                        ,'UQ' --UNIQUE_CONSTRAINT
                        )
