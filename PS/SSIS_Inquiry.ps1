cls

New-PSDrive -Name filesearch -PSProvider FileSystem -Root "C:\Toptal\Project\Repo\ETL\"

Set-Location -Path "filesearch:\"

$DBServer = "XPS17"
$DBName = "DataCommandCenter"
$sqlConnection = New-Object System.Data.SqlClient.SqlConnection
$sqlConnection.ConnectionString = "Server=$DBServer;Database=$DBName;Integrated Security=True;"
$sqlConnection.Open()

if ($sqlConnection.State -ne [Data.ConnectionState]::Open) {
	Write-Host "Connectioin to DB is not open"
	Exit
}

$SqlCmd = New-Object System.Data.SqlClient.SqlCommand
$SqlCmd.Connection = $sqlConnection

#Unsure on this block, SQL command should be select something to get last update date
#$SqlCommand = $SqlCommand - replace "@{#text=", ""
#$SqlCommand = $SqlCommand - replace "}", ""
#$SqlCommand = $SqlCommand - replace "'", "''"

$LastDt = '01/01/2000' #$SqlCmd.ExecuteScalar();
#Unsure on this block?

Write-Host "Pulling packages updated since $LastDt"

foreach ($PackagePath in Get-ChildItem -Include *.dtsx -Recurse | Where{$_.LastWriteTime -gt $LastDt}) {
	Write-Host "Interrogating SSIS package $PackagePath"
	$LastMod = (Get-Item $PackagePath).LastWriteTime
	$PackageName = $XmlDocument.Executable.ObjectName
	$DTSID = $XmlDocument>Executable.$DTSID

	$SqlCmd.CommandText = "SET NOCOUNT ON; "+
	"INSERT INTO etl.SSIS_PackageHeader "+
	"(PackagePath, PackageName, DTSID, LastModified) "+
	"VALUES ('$PackagePath', '$PackageName', '$DTSID', '$LastMod'); "

	$SqlCmd.ExecuteNonQuery()

}



function ProcessDataFlow($SqlCmd, $DataFlow) {
	[string] $CompName = $DataFlow.refId
	[string] $SqlCommand = $Dataflow.properties.property | ?{$_.name -eq 'SqlCommand'} | select '#text'
	[string] $OpenRowset = $Dataflow.properties.property | ?{$_.name -eq 'OpenRowset'} | select '#text'
	[string] $Conn = $DataFlow.connections.connection.connectionManagerID
	[string] $AccessMode = $Dataflow.properties.property | ?{$_.name -eq 'AccessMode'} | select '#text'

	Write—-Host $CompName
	
	$SqlCommand = $SqlCommand -replace "@{#text=", ""
	$SqlCommand = $SqlCommand -replace "}", ""
	$SqlCommand = $SqlCommand -replace "'", "''"
	$OpenRowset = $OpenRowset -replace "@{#text=", ""
	$OpenRowset = $OpenRowset -replace "}", ""
	$AccessMode = $AccessMode -replace "@{#text=", ""
	$AccessMode = $AccessMode -replace "}", ""
	
	$AccessModeDesc = 'Ohter'
	
	if ($AccessMode -eq '3') {
		Write-Host $OpenRowset
		$AccessModeDesc = 'OpenRowset'
	}

	if ($AccessMode -eq '2') {
		Write-Host $SqlCommand
		$AccessModeDesc = 'SqlCommand'
	}	

	Write-Host $Conn
	Write-Host $AccessMode

	$SqlCmd.CommandText = "SET NOCOUNT ON; "+
						  "INSERT INTO etl.SSIS_PackageDataflow "+
						  "(PackageID, DataflowComponentName, ConnectionManagerID, DataflowObjectAccessMode, DataflowObjectAccessModeDescription,DataflowObjectSQLCommand,DataflowObjectOpenRowset) "+
						  "VALUES ($PackageID, '$CompName', '$Conn', '$AccessMode', '$AccessModeDesc', '$SqlCommand', '$OpenRowset'); "

	$SqlCmd.ExecuteNonQuery()

	Write-Host "----Parsing Inputs1"
	foreach ($Input in $Dataflow.Inputs.Input){
		[string] $CachedName = $Col.$CachedName
		[string] $InputRefID = $Col.refID
		[string] $LineageID = $Col.lineageId
		$SqlCmd.CommandText = "SET NOCOUNT ON; "+
								"INSERT INTO etl.SSIS_PackageDataflowLineage "+
								"(PackageID, DataflowComponentName, RefID, CachedName, LineageID )"+
								"VALUES ($PackageID, '$CompName', '$InputRefID', '$CahcedName', '$LineageID'); "
		$SqlCmd.ExecuteNonQuery()

		Write-Host $col.CachedName
		Write-Host $col.lineageId
	}
}

function ProcessPaths($SqlCmd, $Path) {
	[string] $RefID = $Path.refId
	[string] $StartID = $Path.$StartID
	[string] $EndID = $path.$EndID
	[string] $PathName = $Path.name

	$SqlCmd.CommandText = "SET NOCOUNT ON; "+
							"INSERT INTO etl.SSIS_PackagePath "+
							"(PackageID, RefID, StartID, EndID, Name) "+
							"VALUES ($PackageID, '$RefID', '$StartID', '$EndId', '$PathName'); "

	$SqlCmd.ExecuteNonQuery()
}

function ProcessProcs($SqlCmd, $exec) {
	Write-Host "SQL task check for "
	Write-Host $exec.CreationName

	foreach($obj in $exec.ObjectData) {
		foreach ($SQL in $obj.SqlTaskData) {
			[string] $ConnectionGUID = $SQL.ConnectionManagerID
			[string] $SQLstmt = $SQL.SqlStatementSource

			Write-Host "SQL task $SQLstmt"

			$SQLstmt = $SQLstmt.Replace("'", "''")

			$SqlCmd.CommandText = "SET NOCOUNT ON; "+
									"INSERT INTO etl.SSIS_PackageSQLCommand "+
									"(PackageID, ConnectionGUID, SQL_Statement) "+
									"VALUES ($PackageID, '$ConnectionGUID', '$SQLstmt'); "

			$SqlCmd.ExecuteNonQuery()
		}
	}
}


Set-Location -Path "C:\Users\DJLCo\source\repos\DJL-Consulting\DataCommandCenter\PS\"
Remove-PSDrive -Name filesearch


