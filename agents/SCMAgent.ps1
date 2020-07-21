﻿###### SCM 
### HELP
# To setup schedule run in powershell SCMConfig;setup-scmconfig 


## Setup
#scm script path
$scmpath = "c:\SCM\SCMConfig.ps1"

function setup-SCMConfig(){


$scmpath = "c:\SCM\SCMConfig.ps1"
$trigger1 =  New-ScheduledTaskTrigger -Once -at '00:00' -RepetitionInterval(New-TimeSpan -Minutes 1) 
$trigger2 =  New-ScheduledTaskTrigger -Once -at '00:00' -RepetitionInterval(New-TimeSpan -Minutes 10) 

$heartbeat = New-ScheduledTaskAction -Execute "PowerShell.exe $scmpath;heartbeat"
$SMCCheckConfig = New-ScheduledTaskAction -Execute "PowerShell.exe $scmpath;check_for_new_config"

Register-ScheduledTask -TaskName "SCMHeartbeat" -Trigger $trigger1 -User "SYSTEM" -Action $command
Register-ScheduledTask -TaskName "SCMCheckConfig" -Trigger $trigger2 -User "SYSTEM" -Action $command


}

## ID
$id = 'win-123456'

## Config password
$KeyEncryptionPassword = ConvertTo-SecureString -AsPlainText -String "Pa55w.rd" -Force

## URL
$ROOT_URL='http://localhost';
$HEARTHBEAT_URL=$ROOT_URL+"/api/hearthbeat";
$DOWNLOAD_URL=$ROOT_URL+"/api/config";

## Config path

$pathRoot = "C:\SCM"
$pathConfig = "c:\SCM\config"
$pathBaseline = "c:\scm\baseline"
$pathExported = "c:\SCM\exported"

If(!(test-path $pathRoot) -or !(test-path $pathConfig) -or !(test-path $pathBaseline) -or !(test-path $pathExported) )
{
      New-Item -ItemType Directory -Force -Path $pathRoot
      New-Item -ItemType Directory -Force -Path $pathConfig
      New-Item -ItemType Directory -Force -Path $pathBaseline
      New-Item -ItemType Directory -Force -Path $pathExported
}

## heartbeat

function heartbeat(){
    # ping server on url 
    Invoke-WebRequest -UseBasicParsing $HEARTHBEAT_URL -ContentType "application/json" -Method POST -Body "{ 'id':$id }"
}


## config data
function export_config_files(){

Export-IISConfiguration -PhysicalPath "C:\SCM\config" -KeyEncryptionPassword $keyEncryptionPassword -Force
Compress-Archive -LiteralPath C:\scm\config -DestinationPath C:\SCM\baseline\exported.zip -Force

}

function Set-SCMConfig([string]$configurl){
Invoke-WebRequest -uri $configurl -outfile $pathBaseline\baseline.zip
Expand-Archive -LiteralPath C:\scm\baseline\baseline.zip -DestinationPath c:\scm\config
Enable-IISSharedConfig -PhysicalPath $pathConfig -KeyEncryptionPassword $KeyEncryptionPassword -Force
Disable-IISSharedConfig

}

function check_for_new_config(){
    
    export_config_files

    $config_sha1 = (Get-FileHash -Path C:\SCM\baseline\exported.zip -Algorithm SHA1).hash
    $data = @{
        id=$id
        config_sha1=$config_sha1
    }

    $json = $data | ConvertTo-Json
    $response = Invoke-RestMethod https://jsonplaceholder.typicode.com/todos/ -Method POST -Body $json -ContentType 'application/json'

    if(!($response.config_sha1 = $config_sha1)){
    Set-SCMConfig($response.configurl)
    }

}


