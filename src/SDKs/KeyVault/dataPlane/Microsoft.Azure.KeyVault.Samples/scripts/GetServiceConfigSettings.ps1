# **********************************************************************************************
# This sample PowerShell gets the settings you'll need for the *.cscfg files
# **********************************************************************************************

# **********************************************************************************************
# You MUST set the following values before running this script
# **********************************************************************************************
$vaultName           = 'MyVaultName'
$resourceGroupName   = 'MyResourceGroupName'
$applicationName     = 'MyAppName'
$storageName         = 'MyStorageName'

# **********************************************************************************************
# You MAY set the following values before running this script
# **********************************************************************************************
$location            = 'East US'                          # Get-AzureLocation
$secretName          = 'MyStorageAccessSecret'

# **********************************************************************************************
# Should we bounce this script execution?
# **********************************************************************************************
if (($vaultName -eq 'MyVaultName') -or `
    ($resourceGroupName -eq 'MyResourceGroupName') -or `
	($applicationName -eq 'MyAppName') -or `
	($storageName -eq 'MyStorageName'))
{
	Write-Host 'You must edit the values at the top of this script before executing' -foregroundcolor Yellow
	exit
}

# **********************************************************************************************
# Log into Azure
# **********************************************************************************************
Write-Host 'Please log into Azure Resource Manager now' -foregroundcolor Green
Login-AzureRmAccount
$VerbosePreference = "SilentlyContinue"

# **********************************************************************************************
# Prep the cert credential data
# **********************************************************************************************

$certificateName = "$applicationName" + "cert"
$myCertThumbprint = (New-SelfSignedCertificate -Type Custom -Subject "$certificateName"-KeyUsage DigitalSignature -KeyAlgorithm RSA -KeyLength 2048 -CertStoreLocation "Cert:\CurrentUser\My" -Provider "Microsoft Enhanced Cryptographic Provider v1.0" ).Thumbprint
$x509 = (Get-ChildItem -Path cert:\CurrentUser\My\$myCertthumbprint)
$password = Read-Host -Prompt "Please enter the certificate password." -AsSecureString

# Saving the self-signed cert and pfx (private key) in case it's needed later
Export-Certificate -cert $x509 -FilePath ".\$certificateName.cer"
Export-PfxCertificate -Cert $x509 -FilePath ".\$certificateName.pfx" -Password $password


$credValue = [System.Convert]::ToBase64String($x509.GetRawCertData())
$now = [System.DateTime]::Now
$oneYearFromNow = $now.AddYears(1)

# **********************************************************************************************
# Create application in AAD if needed
# **********************************************************************************************
$SvcPrincipals = (Get-AzureRmADServicePrincipal -SearchString $applicationName)
if(-not $SvcPrincipals)
{
    # Create a new AD application if not created before
    $identifierUri = [string]::Format("http://localhost:8080/{0}",[Guid]::NewGuid().ToString("N"))
    $homePage = "http://contoso.com"
    Write-Host "Creating a new AAD Application"
    $ADApp = New-AzureRmADApplication -DisplayName $applicationName -HomePage $homePage -IdentifierUris $identifierUri  -CertValue $credValue -StartDate $now -EndDate $oneYearFromNow
    Write-Host "Creating a new AAD service principal"
    $servicePrincipal = New-AzureRmADServicePrincipal -ApplicationId $ADApp.ApplicationId
}
else
{
    # Assume that the existing app was created earlier with the right X509 credentials. We don't modify the existing app to add new credentials here.
    Write-Host "WARNING: An application with the specified name ($applicationName) already exists." -ForegroundColor Yellow -BackgroundColor Black
    Write-Host "         Proceeding with script execution assuming that the app has the correct X509 credentials already set." -ForegroundColor Yellow -BackgroundColor Black
    Write-Host "         If you are not sure about the existing app's credentials, choose an app name that doesn't already exist and the script with create it and set the specified credentials for you." -ForegroundColor Yellow -BackgroundColor Black
    $servicePrincipal = $SvcPrincipals[0]
}


# **********************************************************************************************
# Create the resource group and vault if needed
# **********************************************************************************************
$rg = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if (-not $rg)
{
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $location   
}

$vault = Get-AzureRmKeyVault -VaultName $vaultName -ErrorAction SilentlyContinue
if (-not $vault)
{
    Write-Host "Creating vault $vaultName"
    $vault = New-AzureRmKeyVault -VaultName $vaultName `
                             -ResourceGroupName $resourceGroupName `
                             -Sku premium `
                             -Location $location
}

# Specify full privileges to the vault for the application
Write-Host "Setting access policy"
Set-AzureRmKeyVaultAccessPolicy -VaultName $vaultName `
	-ObjectId $servicePrincipal.Id `
	-PermissionsToKeys all `
	-PermissionsToSecrets all `
	-PermissionsToCertificate all `
	-PermissionsToStorage all

# **********************************************************************************************
# Create a storage account if needed
# **********************************************************************************************
$sa = Get-AzureRmStorageAccount -ResourceGroupName $resourceGroupName -Name $storageName -ErrorAction SilentlyContinue
if (-not $sa) {
    Write-Host "Creating a new storage account"
    New-AzureRmStorageAccount -ResourceGroupName $resourceGroupName -Name $storageName -SkuName Standard_LRS -Location $location
}

# **********************************************************************************************
# Store storage account access key as a secret in the vault
# **********************************************************************************************

$storagekey = (Get-AzureRmStorageAccountKey -StorageAccountName $storageName -ResourceGroupName $resourceGroupName)[0].Value
if(-not $storageKey)
{
	Write-Host 'Storage key could not be retrieved. Make sure the storage account exists.' -foregroundcolor Yellow
	exit	
}
								  
Write-Host "Setting secret $secretName in vault $vaultName using primary storage key"
$secret = Set-AzureKeyVaultSecret -VaultName $vaultName `
								  -Name $secretName `
								  -SecretValue (ConvertTo-SecureString -String $storagekey -AsPlainText -Force) 

# **********************************************************************************************
# Print the XML settings that should be copied into the CSCFG files
# **********************************************************************************************
Write-Host "Place the following into both CSCFG files for the SampleAzureWebService project:" -ForegroundColor Cyan
'<Setting name="StorageAccountName" value="' + $storageName + '" />'
'<Setting name="StorageAccountKeySecretUrl" value="' + $secret.Id.Substring(0, $secret.Id.LastIndexOf('/')) + '" />'
'<Setting name="KeyVaultSecretCacheDefaultTimeSpan" value="00:00:00" />'
'<Setting name="KeyVaultAuthClientId" value="' + $servicePrincipal.ApplicationId + '" />'
'<Setting name="KeyVaultAuthCertThumbprint" value="' + $myCertThumbprint + '" />'
'<Certificate name="KeyVaultAuthCert" thumbprint="' + $myCertThumbprint + '" thumbprintAlgorithm="sha1" />'
Write-Host

