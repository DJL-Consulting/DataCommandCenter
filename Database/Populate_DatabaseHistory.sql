use B2B_Datamart
go

--truncate table DataCommandCenter.meta.[SQL_DatabaseHistory]

insert into DataCommandCenter.meta.[SQL_DatabaseHistory]
(DatabaseID, CheckDatetime, [DataSizeMB], [LogSizeMB])

select 
	(select ID from DataCommandCenter.meta.SQL_Database where DatabaseName = DB_NAME() AND ServerID = (select ID from DataCommandCenter.meta.Server where ServerName = @@SERVERNAME) ) as DatabaseID, 
	GETDATE() as CheckDatetime,
	(SELECT sum(size/128.0) AS CurrentSizeMB FROM sys.database_files where type = 0) as DataSizeMB,
	(SELECT sum(size/128.0) AS CurrentSizeMB FROM sys.database_files where type = 1) as LogSizeMB
--from sys.databases d
--where name not in ('master', 'tempdb', 'model', 'msdb')
