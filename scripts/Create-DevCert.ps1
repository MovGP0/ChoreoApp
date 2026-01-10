param(
    [string]$Subject = 'CN=ChoreoApp Dev',
    [string]$FriendlyName = 'ChoreoApp Dev',
    [string]$StoreLocation = 'CurrentUser',
    [string]$StoreName = 'My'
)

$cert = New-SelfSignedCertificate -Type Custom -Subject $Subject -KeyUsage DigitalSignature -FriendlyName $FriendlyName -CertStoreLocation "Cert:\$StoreLocation\$StoreName" -TextExtension @(
    '2.5.29.37={text}1.3.6.1.5.5.7.3.3',
    '2.5.29.19={text}'
)

$cert.Thumbprint
