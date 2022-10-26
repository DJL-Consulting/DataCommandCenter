SET ANSI_NULLS OFF
SET QUOTED_IDENTIFIER OFF

/*  
Description: Calculates SQL object data lineage on local server
from system objects
=============================================
exec Calc_Lineage 
*/

CREATE PROCEDURE [dbo].[Calc_Lineage] 
AS
BEGIN   
  SET NOCOUNT ON;

  SET ansi_warnings off;

  declare
	@ID			bigint,
	@Cnt		bigint,
	@SQL		nvarchar(max),
	@DB			nvarchar(500),
	@Sch		nvarchar(500),
	@ObjName	nvarchar(500),
	@Type		nvarchar(500),
	@LocalDB	nvarchar(500) = 'DataCommandCenter.dbo';

	/*  REMMED below, use to re-populate database data

	delete from DataCommandCenter.meta.SQL_Database 
	DBCC CHECKIDENT ('DataCommandCenter.meta.SQL_Database', RESEED, 0);

	PRINT 'Getting DB metadata';

	set @SQL = (SELECT QuerySQL from Query where QueryName = 'PopulateDB')
	exec sp_executeSQL @SQL;

	*/
	   
	delete from DataCommandCenter.meta.SQL_Object 
	DBCC CHECKIDENT ('DataCommandCenter.meta.SQL_Object', RESEED, 0);

	PRINT 'Getting Object metadata';
	--First, run a query to populate object metadata
	set @SQL = (SELECT QuerySQL from Query where QueryName = 'PopulateObject')

	--print @sql;
	exec sp_MSforeachdb @SQL;
	
	delete from DataCommandCenter.meta.SQL_Column
	DBCC CHECKIDENT ('DataCommandCenter.meta.SQL_Column', RESEED, 0);

	PRINT 'Getting Column metadata';
	--Next, run a query to populate column metadata
	set @SQL = (SELECT QuerySQL from Query where QueryName = 'PopulateColumn')

	--print @sql;
	exec sp_MSforeachdb @SQL;
	
	truncate table LineageFlow;

	set @ID = 0;

	while (SELECT top 1 ObjectID from meta.SQL_ObjectMetadata where ObjectID > @ID and ObjectType <> 'USER_TABLE') > 0
	BEGIN
		SELECT top 1
			@DB = DatabaseName,
			@Sch = SchemaName,
			@ObjName = ObjectName,
			@Type = ObjectType,
			@ID =  ObjectID
		from meta.SQL_ObjectMetadata
		where ObjectID > @ID and ObjectType <> 'USER_TABLE'
		order by ObjectID;

		print @db + '.'+@sch+'.'+@objName+' '+@type+' - '+cast(@ID as nvarchar);


		set @SQL = '
			Insert Into LineageFlow (SourceObjectID, DestinationObjectID, Operation)
			SELECT DISTINCT 
				(SELECT ObjectID from meta.SQL_ObjectMetadata 
				where ServerName = ISNULL(r.referenced_server_name, @@SERVERNAME) 
					AND DatabaseName = ISNULL(r.referenced_database_name, '''+@db+''')
					AND SchemaName = ISNULL(r.referenced_schema_name, '''+@sch+''')
					AND ObjectName = R.Referenced_Entity_Name) as Source_ID,
				' + cast(@ID as varchar)  + ' as Destination_ID,
				''Select'' as Operation
			from ' + @DB + '.sys.dm_sql_referenced_entities(''' + @Sch + '.' + @ObjName + ''', ''Object'') r
			WHERE r.Is_Selected = 1 OR r.Is_Select_All = 1
				AND r.Referenced_Database_Name NOT IN (SELECT name from sys.databases where state <> 0)
			';

		--print @sql
		print 'Get Select objects for @ID = '+cast(@ID as nvarchar);

		BEGIN TRY
			exec sp_executeSQL @SQL;
		END TRY
		BEGIN CATCH
		  -- Do nothing here, we just don't want an exception stopping the loop
		END CATCH;

		set @SQL = '
			Insert Into LineageFlow (SourceObjectID, DestinationObjectID, Operation)
			SELECT DISTINCT
			' + CAST(@ID as varchar) + ' as Source_ID,
			(SELECT ObjectID from  meta.SQL_ObjectMetadata 
					where ServerName = ISNULL(r.referenced_server_name, @@SERVERNAME) 
					AND DatabaseName = ISNULL(r.referenced_database_name, '''+@db+''')
					AND SchemaName = ISNULL(r.referenced_schema_name, '''+@sch+''')
					AND ObjectName = R.Referenced_Entity_Name) as Destination_ID,
					''Write'' as Operation
			from ' + @DB + '.sys.dm_sql_referenced_entities(''' + @Sch + '.' + @ObjName + ''', ''Object'') r
			where r.is_updated = 1
				AND r.Referenced_Database_Name NOT IN (SELECT name from sys.databases where state <> 0)
		';

		--print @sql
		print 'Get Write objects for @ID = '+cast(@ID as nvarchar);

		BEGIN TRY
			exec sp_executeSQL @SQL;
		END TRY
		BEGIN CATCH
		  -- Do nothing here, we just don't want an exception stopping the loop
		END CATCH;	
	END;  
	
END;
			




GO
