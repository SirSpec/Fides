param (
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

Set-AzKeyVaultAccessPolicy `
    -VaultName $KeyVaultName `
    -ObjectId (Get-AzADApplication).Id `
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
    -name $DashboardName `
    -location $Location `
    -serverFarmResourceGroup $ResourceGroupName

New-AzResourceGroupDeployment `
    -Name deployFunction `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile "infrastructure\arm\sync-function\template.json" `
    -TemplateParameterFile "infrastructure\arm\sync-function\parameters.json" `
    -name $FunctionName `
    -location $Location `
    -serverFarmResourceGroup $ResourceGroupName `
    -storageAccountName "$($FunctionName)storage"

New-AzResourceGroupDeployment `
    -Name deployConsumers `
    -ResourceGroupName $ResourceGroupName `
    -TemplateFile "infrastructure\arm\sync-consumers\template.json" `
    -TemplateParameterFile "infrastructure\arm\sync-consumers\parameters.json" `
    -name $ConsumersName `
    -location $Location `
    -serverFarmResourceGroup $ResourceGroupName
