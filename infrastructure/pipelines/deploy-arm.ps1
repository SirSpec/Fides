$resourceGroupName = 'Fides'
$keyVaultName = 'fides-keyvault'
$location = 'westeurope'

New-AzSubscriptionDeployment `
    -Name deployResourceGroup `
    -Location $location `
    -TemplateFile "infrastructure\arm\resource-group\template.json" `
    -TemplateParameterFile "infrastructure\arm\resource-group\parameters.json" `
    -rgName $resourceGroupName

New-AzKeyVault `
    -Name $keyVaultName `
    -ResourceGroupName $resourceGroupName `
    -Location $location `
    -EnabledForTemplateDeployment

New-AzResourceGroupDeployment `
    -Name deployRegistry `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "infrastructure\arm\registry\template.json" `
    -TemplateParameterFile "infrastructure\arm\registry\parameters.json"

New-AzADApplication -DisplayName Dashboard -AvailableToOtherTenants $false

New-AzResourceGroupDeployment `
    -Name deployDashboard `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "infrastructure\arm\dashboard\template.json" `
    -TemplateParameterFile "infrastructure\arm\dashboard\parameters.json"

New-AzResourceGroupDeployment `
    -Name deployFunction `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "infrastructure\arm\sync-function\template.json" `
    -TemplateParameterFile "infrastructure\arm\sync-function\parameters.json"

New-AzResourceGroupDeployment `
    -Name deployConsumers `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "infrastructure\arm\sync-consumers\template.json" `
    -TemplateParameterFile "infrastructure\arm\sync-consumers\parameters.json"
