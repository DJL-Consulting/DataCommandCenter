

truncate table DataCommandCenter.meta.[SQL_Database]

insert into DataCommandCenter.meta.[SQL_Database]
([ServerID], [DatabaseName], [Compatability], [Recovery], [CreatedDatetime], [Collation], [Access], [ReadOnly], [DataSizeMB], [LogSizeMB], PullMetadata)

select 
	1 as ServerID,
	name as DatabaseName,
	compatibility_level as Compatability,
	recovery_model_desc as Recovery,
	create_date as CreatedDatetime,
	collation_name as Collation,
	user_access_desc as Access,
	is_read_only as ReadOnly,
	(SELECT sum(size/128.0) AS CurrentSizeMB FROM sys.database_files where type = 0) as DataSizeMB,
	(SELECT sum(size/128.0) AS CurrentSizeMB FROM sys.database_files where type = 1) as LogSizeMB,
	1 as PullMetadata
from sys.databases d
where name not in ('master', 'tempdb', 'model', 'msdb')
