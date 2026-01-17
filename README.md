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

### Windows MSIX signing (dev cert)
- Create a dev certificate and get its thumbprint:
```powershell
.\scripts\Create-DevCert.ps1
```
- Use the printed thumbprint when publishing (required for MSIX packaging):
```powershell
dotnet publish .\ChoreoApp\ChoreoApp.csproj --output .\publish\win10\ --framework net10.0-windows10.0.19041.0 --self-contained --nologo -p:PackageCertificateThumbprint=YOUR_THUMBPRINT_HERE
```
- Publish as a single-file self-contained app (Windows):
```powershell
dotnet publish .\ChoreoApp\ChoreoApp.csproj --output .\publish\win10\ --framework net10.0-windows10.0.19041.0 --self-contained --nologo -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:WindowsPackageType=None -p:GenerateAppxPackageOnBuild=false -p:PackageCertificateThumbprint=YOUR_THUMBPRINT_HERE
```
- MSIX output location:
`.\ChoreoApp\bin\Release\net10.0-windows10.0.19041.0\win-x64\AppPackages\`

> [!note]
> For the `net10.0-windows10.0.19041.0` target, Android SDK and Mono are not required.

### Android APK publish
```powershell
dotnet publish .\ChoreoApp\ChoreoApp.csproj --output .\publish\android\ --framework net10.0-android --configuration Release --nologo -p:AndroidPackageFormat=apk -p:AndroidSdkDirectory="$env:LOCALAPPDATA\Android\Sdk"
```

APK output locations:
- `.\ChoreoApp\bin\Release\net10.0-android\io.github.choreoapp-Signed.apk`
- `.\ChoreoApp\bin\Release\net10.0-android\publish\io.github.choreoapp-Signed.apk`
