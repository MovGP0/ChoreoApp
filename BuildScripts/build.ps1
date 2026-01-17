param(
    [string]$Configuration = 'Release',
    [string]$Platform = 'Windows',
    [string]$Architecture = 'x64',
    [switch]$Publish,
    [switch]$Installer,
    [string]$ProductVersion = '1.0.0'
)

$ErrorActionPreference = 'Stop'

$repositoryRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$solutionPath = Join-Path $repositoryRoot 'ChoreoApp.slnx'
$projectPath = Join-Path $repositoryRoot 'ChoreoApp\ChoreoApp.csproj'
$publishRoot = Join-Path $repositoryRoot 'artifacts\publish'
$installerRoot = Join-Path $repositoryRoot 'artifacts\installer'
$wixVersion = '6.0.2'

function Get-TargetFramework
{
    switch ($Platform)
    {
        'Windows' { return 'net10.0-windows10.0.19041.0' }
        'MacOS' { return 'net10.0-maccatalyst' }
        'iOS' { return 'net10.0-ios' }
        'Android' { return 'net10.0-android' }
        default { throw "Unsupported platform: $Platform" }
    }
}

function Get-RuntimeIdentifier
{
    if ($Platform -ne 'Windows')
    {
        return $null
    }

    switch ($Architecture)
    {
        'x86' { return 'win-x86' }
        'x64' { return 'win-x64' }
        'arm64' { return 'win-arm64' }
        default { throw "Unsupported architecture: $Architecture" }
    }
}

function Assert-PlatformSupport
{
    if ($Platform -eq 'Windows' -and -not $IsWindows)
    {
        throw 'Windows builds require Windows.'
    }

    if (($Platform -eq 'MacOS' -or $Platform -eq 'iOS') -and -not $IsMacOS)
    {
        throw 'MacOS and iOS builds require macOS.'
    }

    if ($Installer -and $Platform -ne 'Windows')
    {
        throw 'MSI installers are only supported on Windows.'
    }
}

function Invoke-DotNet
{
    param(
        [string[]]$Arguments
    )

    & dotnet @Arguments
    if ($LASTEXITCODE -ne 0)
    {
        throw "dotnet failed: $($Arguments -join ' ')"
    }
}

Task Default -Depends Build

Task Validate -Action `
{
    Assert-PlatformSupport
}

Task Restore -Depends Validate -Action `
{
    $framework = Get-TargetFramework
    $rid = Get-RuntimeIdentifier

    $restoreArgs = @(
        'restore',
        $projectPath,
        "-p:TargetFramework=$framework"
    )

    if ($rid)
    {
        $restoreArgs += "-p:RuntimeIdentifier=$rid"
    }

    Invoke-DotNet $restoreArgs
}

Task Build -Depends Restore -Action `
{
    $framework = Get-TargetFramework
    Invoke-DotNet @(
        'build',
        $projectPath,
        '-c', $Configuration,
        '-f', $framework
    )
}

Task Publish -Depends Restore -Action `
{
    $framework = Get-TargetFramework
    $rid = Get-RuntimeIdentifier

    $platformFolder = $Platform.ToLowerInvariant()
    if ($Platform -eq 'Windows')
    {
        $platformFolder = "$platformFolder-$Architecture"
    }

    $outputPath = Join-Path $publishRoot $platformFolder
    New-Item -ItemType Directory -Force -Path $outputPath | Out-Null

    $args = @(
        'publish',
        $projectPath,
        '-c', $Configuration,
        '-f', $framework,
        '-o', $outputPath
    )

    if ($Platform -eq 'Windows')
    {
        $args += @(
            '-p:UseMonoRuntime=false',
            '-p:GenerateAppxPackageOnBuild=false',
            '-p:WindowsPackageType=None'
        )
    }

    if ($rid)
    {
        $args += @('-r', $rid, '--self-contained', 'true')
    }

    Invoke-DotNet $args
}

Task Installer -Depends Publish -Action `
{
    $rid = Get-RuntimeIdentifier
    if (-not $rid)
    {
        throw 'MSI installer requires a Windows runtime identifier.'
    }

    $publishDir = Join-Path $publishRoot "windows-$Architecture"
    if (-not (Test-Path $publishDir))
    {
        throw "Publish output not found: $publishDir"
    }

    $toolsPath = Join-Path $repositoryRoot 'artifacts\tools\wix'
    Invoke-DotNet @('tool', 'install', '--tool-path', $toolsPath, 'wix', '--version', $wixVersion)

    $arch = switch ($Architecture)
    {
        'x86' { 'x86' }
        'x64' { 'x64' }
        'arm64' { 'arm64' }
        default { throw "Unsupported architecture: $Architecture" }
    }

    New-Item -ItemType Directory -Force -Path $installerRoot | Out-Null
    $msiOut = Join-Path $installerRoot "ChoreoApp-$rid.msi"

    $env:PATH = "$(Resolve-Path $toolsPath);$env:PATH"
    & wix build -arch $arch -define PublishDir=$publishDir -define ProductVersion=$ProductVersion -define ProductName=ChoreoApp -define Manufacturer=ChoreoApp -out $msiOut "$(Join-Path $repositoryRoot 'installer\ChoreoApp.wxs')"
    if ($LASTEXITCODE -ne 0)
    {
        throw 'wix build failed.'
    }
}
