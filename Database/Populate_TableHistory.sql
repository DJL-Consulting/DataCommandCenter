USE B2B_Datamart
GO


--truncate table DataCommandCenter.meta.SQL_TableHistory 

Declare @ChckDate datetime = GETDATE();

insert into DataCommandCenter.meta.SQL_TableHistory 
([ObjectID], [CheckDatetime], [Rows], [SizeMB])

SELECT 
	(select ID from DataCommandCenter.meta.SQL_Object where DatabaseID = (select ID from DataCommandCenter.meta.SQL_Database where DatabaseName = DB_NAME() AND ServerID = (select ID from DataCommandCenter.meta.Server where ServerName = @@SERVERNAME)) AND SchemaName = s.name AND ObjectName = t.Name) as ObjectID,
    @ChckDate as CheckDatetime,
	--t.NAME AS TableName,
    --s.Name AS SchemaName,
    p.Rows,
    CAST(ROUND(((SUM(a.total_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS SizeMB
FROM 
   sys.tables t
INNER JOIN      
    sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN 
    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
INNER JOIN 
    sys.allocation_units a ON p.partition_id = a.container_id
LEFT OUTER JOIN 
    sys.schemas s ON t.schema_id = s.schema_id
WHERE 
    t.NAME NOT LIKE 'dt%' 
    AND t.is_ms_shipped = 0
    AND i.OBJECT_ID > 255 
GROUP BY 
    t.Name, s.Name, p.Rows
--ORDER BY 
--    TotalSpaceMB DESC, t.Name