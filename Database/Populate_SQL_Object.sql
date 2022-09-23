USE B2B_Datamart
GO


--truncate table DataCommandCenter.meta.SQL_TableHistory 
--truncate table DataCommandCenter.meta.SQL_Column 
--delete from DataCommandCenter.meta.SQL_Object 
--DBCC CHECKIDENT ('DataCommandCenter.meta.SQL_Object', RESEED, 0);

insert into DataCommandCenter.meta.SQL_Object 

([DatabaseID], [SchemaName], [ObjectName], [ObjectType], Rows, SizeMB, ObjectDefinition)

SELECT distinct (select ID from DataCommandCenter.meta.SQL_Database where DatabaseName = DB_NAME() AND ServerID = (select ID from DataCommandCenter.meta.Server where ServerName = @@SERVERNAME) ) as DatabaseID 
        ,s.name AS SchemaName
        ,o.name AS ObjectName
	    ,o.type_desc AS ObjectType
		,p.Rows
        ,CAST(ROUND(((SUM(a.total_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS SizeMB
		,OBJECT_DEFINITION(o.Object_ID) as ObjectDefinition
    FROM  sys.objects o 
    INNER JOIN  sys.schemas s
      ON  s.schema_id = o.schema_id
	LEFT OUTER JOIN sys.tables t
		ON t.object_id = o.object_id
	LEFT OUTER JOIN sys.indexes i 
		ON t.OBJECT_ID = i.object_id
	LEFT OUTER JOIN sys.partitions p 
		ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
	LEFT OUTER JOIN sys.allocation_units a 
		ON p.partition_id = a.container_id
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
GROUP BY
        s.name 
        ,o.name
	    ,o.type_desc
		,p.Rows
		,OBJECT_DEFINITION(o.Object_ID)

ORDER BY  ObjectType,
          SchemaName,
          ObjectName

