$resourceGroupName = 'Fides'

$subscription = Get-AzSubscription

New-AzKeyVault -Name "fides-keyvault" -ResourceGroupName $resourceGroupName -Location "germanywestcentral"

New-AzSubscriptionDeployment `
    -Name 'deployResourceGroup' `
    -Location westeurope `
    -TemplateFile "infrastructure\arm\resource-group\template.json" `
    -TemplateParameterFile "infrastructure\arm\resource-group\parameters.json" `
    -rgName $resourceGroupName `

New-AzResourceGroupDeployment `
    -Name deployRegistry `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "infrastructure\arm\registry\template.json" `
    -TemplateParameterFile "infrastructure\arm\registry\parameters.json"

New-AzResourceGroupDeployment `
    -Name deployFunction `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "infrastructure\arm\sync-function\template.json" `
    -TemplateParameterFile "infrastructure\arm\sync-function\parameters.json" `
    -subscriptionId $subscription.Id
