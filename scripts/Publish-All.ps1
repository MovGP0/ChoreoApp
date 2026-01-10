param(
    [string]$Configuration = 'Release'
)

$projectPath = Join-Path $PSScriptRoot '..\ChoreoApp\ChoreoApp.csproj'
$projectPath = Resolve-Path $projectPath

function Publish-Target
{
    param(
        [string]$Framework,
        [string]$RuntimeIdentifier
    )

    $args = @(
        'publish',
        $projectPath,
        '-c', $Configuration,
        '-f', $Framework
    )

    if ($RuntimeIdentifier)
    {
        $args += @('-r', $RuntimeIdentifier)
    }

    dotnet @args
    if ($LASTEXITCODE -ne 0)
    {
        throw "Publish failed for $Framework $RuntimeIdentifier"
    }
}

# see https://learn.microsoft.com/en-us/dotnet/core/rid-catalog for valid runtime identifiers
$targets = @(
    @{ Framework = 'net10.0-android'; RuntimeIdentifier = $null; Requires = 'Windows' },
    @{ Framework = 'net10.0-ios'; RuntimeIdentifier = $null; Requires = 'Mac' },
    @{ Framework = 'net10.0-maccatalyst'; RuntimeIdentifier = $null; Requires = 'Mac' },
    @{ Framework = 'net10.0-windows10.0.19041.0'; RuntimeIdentifier = 'win-x64'; Requires = 'Windows' },
    @{ Framework = 'net10.0-windows10.0.19041.0'; RuntimeIdentifier = 'win-x86'; Requires = 'Windows' },
    @{ Framework = 'net10.0-windows10.0.19041.0'; RuntimeIdentifier = 'win-arm64'; Requires = 'Windows' }
)

foreach ($target in $targets)
{
    $requires = $target.Requires
    if ($requires -eq 'Mac' -and -not $IsMacOS)
    {
        Write-Host "Skipping $($target.Framework) (requires macOS)."
        continue
    }

    if ($requires -eq 'Windows' -and -not $IsWindows)
    {
        Write-Host "Skipping $($target.Framework) (requires Windows)."
        continue
    }

    Publish-Target -Framework $target.Framework -RuntimeIdentifier $target.RuntimeIdentifier
}
