param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    [ValidateSet('Windows', 'MacOS', 'iOS', 'Android')]
    [string]$Platform = 'Windows',
    [ValidateSet('x86', 'x64', 'arm64')]
    [string]$Architecture = 'x64',
    [switch]$Publish,
    [switch]$Installer,
    [switch]$Restore,
    [string]$ProductVersion = '1.0.0'
)

$ErrorActionPreference = 'Stop'

$buildScript = Join-Path $PSScriptRoot 'BuildScripts\build.ps1'

Import-Module psake

$PSDefaultParameterValues['Invoke-Psake:docs'] = $false
$PSDefaultParameterValues['Invoke-Psake:detailedDocs'] = $false

$properties = @{
    Configuration = $Configuration
    Platform = $Platform
    Architecture = $Architecture
    Publish = $Publish
    Installer = $Installer
    ProductVersion = $ProductVersion
}

if ($Installer)
{
    Invoke-Psake -buildFile $buildScript -taskList Installer -properties $properties -nologo -docs:$false -detailedDocs:$false
}
elseif ($Publish)
{
    Invoke-Psake -buildFile $buildScript -taskList Publish -properties $properties -nologo -docs:$false -detailedDocs:$false
}
elseif ($Restore)
{
    Invoke-Psake -buildFile $buildScript -taskList Restore -properties $properties -nologo -docs:$false -detailedDocs:$false
}
else
{
    Invoke-Psake -buildFile $buildScript -taskList Build -properties $properties -nologo -docs:$false -detailedDocs:$false
}

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}
