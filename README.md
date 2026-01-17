# ChoreoApp

Choreography Viewer and Editor

## Git

Note that the build does not support the git `refstorage` format.
You might need to migrate your git refs before building:
```shell
git refs migrate --ref-format=files
```

## Prerequisites
- .NET 10 SDK or higher installed
```shell
winget install -e --id "Microsoft.DotNet.SDK.10" --source winget --accept-source-agreements --accept-package-agreements
```

- MAUI workload installed
```shell
dotnet workload install maui
```

- Android workload (for Android builds)
```shell
dotnet workload install android
```

Install current Java Development Kit (JDK)
```shell
winget install -e --id Microsoft.OpenJDK.21
```

Wix Installer
```powershell
dotnet tool install --global wix --version 6.0.2
wix --help
```

Psake
```powershell
Install-Module psake
Import-Module psake
```

## Build for Windows

```shell
dotnet build ChoreoApp.slnx -f net10.0-windows10.0.19041.0
dotnet run ChoreoApp.slnx -f net10.0-windows10.0.19041.0
```

## Build for Android

```powershell
dotnet build ChoreoApp.slnx `
    -t:InstallAndroidDependencies `
    -f net10.0-android `
    -p:JavaSdkDirectory="C:\Program Files\Microsoft\jdk-21.0.x" `
    -p:AndroidSdkDirectory="$env:LOCALAPPDATA\Local\Android\Sdk" `
    -p:AcceptAndroidSdkLicenses=True;
```
