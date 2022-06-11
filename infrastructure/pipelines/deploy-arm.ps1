param (
    $ServicePrincipleName,
    $ResourceGroupName,
    $Location,
    $KeyVaultName,
    $DashboardADApplicationName,
    $RegistryName,
    $DashboardName,
    $FunctionName,
    $ConsumersName)

New-AzSubscriptionDeployment `
    -Name deployResourceGroup `
    -Location $Location `
    -TemplateFile "infrastructure\arm\resource-group\template.json" `
    -rgName $ResourceGroupName `
    -rgLocation $Location

New-AzKeyVault `
    -Name $KeyVaultName `
    -ResourceGroupName $ResourceGroupName `
    -Location $Location `
    -EnabledForTemplateDeployment

$servicePrinciple = (Get-AzADServicePrincipal -AppId (Get-AzADApplication -DisplayName $ServicePrincipleName).AppId).Id

Set-AzKeyVaultAccessPolicy `
    -VaultName $KeyVaultName `
    -ObjectId $servicePrinciple `
    -PermissionsToSecrets recover,delete,backup,set,restore,list,get `
    -PermissionsToKeys recover,delete,backup,restore,list,get `
    -PermissionsToCertificates recover,delete,backup,restore,list,get

New-AzResourceGroupDeployment `
    -Name deployRegistry `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile "infrastructure\arm\registry\template.json" `
    -TemplateParameterFile "infrastructure\arm\registry\parameters.json" `
    -registryName $RegistryName `
    -registryLocation $Location

New-AzADApplication -DisplayName $DashboardADApplicationName -AvailableToOtherTenants $false

New-AzResourceGroupDeployment `
    -Name deployDashboard `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile "infrastructure\arm\dashboard\template.json" `
    -TemplateParameterFile "infrastructure\arm\dashboard\parameters.json" `
    -resourceName $DashboardName `
    -location $Location `
    -serverFarmResourceGroup $ResourceGroupName

New-AzResourceGroupDeployment `
    -Name deployFunction `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile "infrastructure\arm\sync-function\template.json" `
    -TemplateParameterFile "infrastructure\arm\sync-function\parameters.json" `
    -resourceName $FunctionName `
    -location $Location `
    -serverFarmResourceGroup $ResourceGroupName `
    -storageAccountName "$($FunctionName)storage"

New-AzResourceGroupDeployment `
    -Name deployConsumers `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile "infrastructure\arm\sync-consumers\template.json" `
    -TemplateParameterFile "infrastructure\arm\sync-consumers\parameters.json" `
    -resourceName $ConsumersName `
    -location $Location `
    -serverFarmResourceGroup $ResourceGroupName
