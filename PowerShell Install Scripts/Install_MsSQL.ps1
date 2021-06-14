#.\Install.ps1 -AppName Studio -FolderName '7.17.1.2164_Studio_Softkey_MSSQL_ENU' -dbUser Creatio -port 8000 -redisDbNum 10

#Install Creatio
param($AppName, $FolderName, $dbUser, $port, $redisDbNum)
$ServerInstance = "K_KRYLOV\SQLEXPRESS";
$myHost = Invoke-Expression 'hostname';

#Step 1 - Create Directory for new Installation
$AppPath = "C:\inetpub\wwwroot\${AppName}";
New-Item -ItemType directory -Path $AppPath -Force;
Write-Host "`nNEW $AppPath DIRECTORY CREATED " -ForegroundColor Green;

#Step2 - Copy folder to $AppPath Folder
Write-Host "Copying necessary files to $AppPath" -ForegroundColor Green
Copy-Item "C:\Build\${FolderName}\*" -Destination $AppPath -Recurse;

#Restoring DataBase
Write-Host "Started Restoring DataBase"
$DbPath = "$AppPath\db"
$dbFile = Get-ChildItem $DbPath | Where-Object {$_.FullName -match ".bak$"}

$mdf = 'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\'+$AppName+'.mdf'
$ldf = 'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\'+$AppName+'.ldf'
$fileName = $dbFile.FullName

# Prepare restore Query
$query = "USE [master] `
RESTORE DATABASE [$AppName] FROM `
DISK = N'$fileName' WITH  FILE = 1, `
MOVE N'TSOnline_Data' TO N'$mdf', `
MOVE N'TSOnline_Log' TO N'$ldf', `
NOUNLOAD,  STATS = 5 `
GO"
# Execute Restore Query
Invoke-Sqlcmd -Query $query -ServerInstance $ServerInstance -QueryTimeout 0
Write-Host "`nDATABASE RESTORED:"-ForegroundColor Green

#Create db user
$dbuserQuery = "if not Exists (select loginname from master.dbo.syslogins where name = '$dbUser')`
Begin`
	declare @SqlStatement as nvarchar(max) = 'USE [master]'`
	EXEC sp_executesql @SqlStatement`
	select @SqlStatement = 'CREATE LOGIN [$dbUser] WITH PASSWORD=N'+'''Supervisor'''+', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF'`
	EXEC sp_executesql @SqlStatement`
End
"
Invoke-Sqlcmd -Query $dbuserQuery -ServerInstance $ServerInstance
Write-Host "`nCHECKING DATABASE USER:"-ForegroundColor Green


#prep Permission Query
$permQuery = "USE [$AppName] `
GO`
CREATE USER [$dbUser] FOR LOGIN [$dbUser]`
GO`
USE [$AppName]`
GO`
ALTER ROLE [db_owner] ADD MEMBER [$dbUser]`
GO
"
Invoke-Sqlcmd -Query $permQuery -ServerInstance $ServerInstance
Write-Host "`nGRANTED DB_OWNER ROLE TO DATABASE USER:"-ForegroundColor Green


#Delete db folder
Remove-Item $AppPath\db\*
Remove-Item $AppPath\db\
Write-Host "\nDeleted db folder" - -ForegroundColor Green;


#Edit ConnectionString.config file
# Replace Redis

$file = "$AppPath\ConnectionStrings.config";
[xml]$doc  =  Get-Content $file;
[System.Xml.XmlNode]$redis = $doc.SelectSingleNode("/connectionStrings/add[@name='redis']");
[System.Xml.XmlAttribute]$redisAttr = $redis.Attributes.GetNamedItem("connectionString","");
$redisAttr.Value = "host=localhost; db=${redisDbNum}; port=6379";

[System.Xml.XmlNode]$db = $doc.SelectSingleNode("/connectionStrings/add[@name='db']");
[System.Xml.XmlAttribute]$dbAttr = $db.Attributes.GetNamedItem("connectionString","");
$dbAttr.Value = "Data Source=${ServerInstance};Initial Catalog=$AppName;User ID=$dbUser;Password=Supervisor;MultipleActiveResultSets=True;Pooling=true;Max Pool Size=100";
$doc.Save($file);

#IIS
# Create new webPool
New-WebAppPool -Name $AppName -Force;

# Create IIS Site
New-WebSite -Name $AppName -ApplicationPool $AppName -Port $port -HostHeader $myHost -PhysicalPath $AppPath -Force

# Add App
New-WebApplication -Name 0 -Site $Appname -ApplicationPool $AppName -PhysicalPath $AppPath"\Terrasoft.WebApp" -Force
Write-Host "IIS Site Created"
Write-Host "`nCREATED NEW SITE AND APP_POOL:"-ForegroundColor Green

#Update clio to the latest version
Invoke-Expression 'dotnet tool update clio -g'

#register with clio
$clioCmd = "clio reg-web-app -u http://${myHost}:${port} -p Supervisor -l Supervisor -m Customer -c true -e ${AppName}" ;
Invoke-Expression $clioCmd;

#Set Active Environment
#$clioCmd = "clio reg-web-app -a ${AppName}";
#Invoke-Expression $clioCmd;

$clioCmd = "clio install-gate -e ${AppName}";
Invoke-Expression $clioCmd;

#Do NOT Show widget on intro page
$clioCmd = "clio set-syssetting ShowWidgetOnIntroPage false Boolean -e ${AppName}";
Invoke-Expression $clioCmd;

#Do NOT Show widget on login page
$clioCmd = "clio set-syssetting ShowWidgetOnLoginPage false Boolean -e ${AppName}";
Invoke-Expression $clioCmd;

#set Default TimeZone to "Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius (GMT+02:00)"
$clioCmd = "clio set-syssetting DefaultTimeZone 97c71d34-55d8-df11-9b2a-001d60e938c6 Lookup -e ${AppName}";
Invoke-Expression $clioCmd;

#set Session Timeout, Max is 720 minutes (12 hours)"
$clioCmd = "clio set-syssetting UserSessionTimeout 720 Integer -e ${AppName}";
Invoke-Expression $clioCmd;

#Disable updating contact ages
$clioCmd = "clio set-syssetting ActualizeAge false Boolean -e ${AppName}";
Invoke-Expression $clioCmd;
$clioCmd = "clio set-syssetting RunAgeActualizationDaily false Boolean -e ${AppName}";
Invoke-Expression $clioCmd;

#Enable Debug mode
$clioCmd = "clio set-syssetting IsDebug true Boolean -e ${AppName}";
Invoke-Expression $clioCmd;

$clioCmd = "clio set-syssetting SchemaNamePrefix ' ' Text -e ${AppName}";
Invoke-Expression $clioCmd;

#Configure Exchange Listener
#https://academy.creatio.com/docs/developer/platform_overview/components_and_services/exchange_listener_synchronization_service
$clioCmd = "clio set-syssetting ExchangeListenerServiceUri 'http://k_krylov:10000/api/listeners' Text -e ${AppName}";
Invoke-Expression $clioCmd;

$clioCmd = "clio set-syssetting BpmonlineExchangeEventsEndpointUrl 'http://k_krylov:10000/NewEmail' Text -e ${AppName}";
Invoke-Expression $clioCmd;


## Prepare workspaceConsole
## https://academy.creatio.com/docs/developer/development_tools/deployment/transfer_solutions_workspaceconsole_utility#case-1227
## https://academy.creatio.com/docs/developer/development_tools/deployment/transfer_solutions_workspaceconsole_utility#case-1227
## PrepareWorkspaceConsole.bat copies all contents of 
## \Terrasoft.WebApp\bin\ to \Terrasoft.WebApp\DesktopBin\WorkspaceConsole\ with replacement.
# $WorkspaceConsolePath = "C:\inetpub\wwwroot\${AppName}\Terrasoft.WebApp\DesktopBin\WorkspaceConsole";
# $WorkspaceConsoleCmd = "${WorkspaceConsolePath}\PrepareWorkspaceConsole.x64.bat"
# Invoke-Expression $WorkspaceConsoleCmd;

## TODO: Simulate key press after copy

## Configure connectionSstring
# $file = "${WorkspaceConsolePath}\Terrasoft.Tools.WorkspaceConsole.exe.config";
# [xml]$doc = Get-Content $file;
# [System.Xml.XmlNode]$doc = $file.SelectSingleNode("configuration/terrasoft/connectionStrings/add[@name='db']");
# [System.Xml.XmlAttribute]$dbAttr = $db.Attributes.GetNamedItem("connectionString","");
# $dbAttr.Value = "Data Source=${ServerInstance};Initial Catalog=$AppName;User ID=$dbUser;Password=Supervisor;MultipleActiveResultSets=True;Pooling=true;Max Pool Size=100";
# $doc.Save($file);



#Open Application
Start-Process "http://${myHost}:${port}"
Write-Host "`n!!! ENJOY YOUR CREATIO !!!" -ForegroundColor Green
