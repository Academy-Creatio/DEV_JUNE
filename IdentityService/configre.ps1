# DOCUMENTATION
# https://academy.creatio.com/docs/user/setup_and_administration/on-site_deployment/deployment_additional_setup/set_up_oauth20_authorization_for_integrated_applications

[string]$clientId = "creatio-designer"
[string]$clientSecret = "thisIsMySuperSecretPasswordThatIWontTellAnyone"
[string]$serverUrl = "http://k_krylov:7071"
[string]$AppName = "studiojune" #this is your clio registed app


#Install API extention package
$clioCmd = "clio install-gate -e ${AppName}";
Invoke-Expression $clioCmd;

# https://academy.creatio.com/docs/user/setup_and_administration/on-site_deployment/deployment_additional_setup/set_up_oauth20_authorization_for_integrated_applications#title-2002-2
#Execute sql script to rnable feature
$clioCmd = "clio sql -f enable.sql -e ${AppName}";
Invoke-Expression $clioCmd;

$clioCmd = "clio install-gate -e ${AppName}";
Invoke-Expression $clioCmd;

#Set Client id for OAuth 2.0 integrations
$clioCmd = "clio set-syssetting OAuth20IdentityServerClientId '${clientId}' Text -e ${AppName}";
Invoke-Expression $clioCmd;

#Set Client secret for OAuth 2.0 integrations
$clioCmd = "clio set-syssetting OAuth20IdentityServerClientSecret '${clientSecret}' Text -e ${AppName}";
Invoke-Expression $clioCmd;

#Set Authorization server Url for OAuth 2.0 integrations
$clioCmd = "clio set-syssetting OAuth20IdentityServerUrl '${serverUrl}' Text -e ${AppName}";
Invoke-Expression $clioCmd;

