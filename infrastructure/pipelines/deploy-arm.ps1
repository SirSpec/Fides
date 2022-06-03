$resourceGroupName = 'Fides'

New-AzSubscriptionDeployment `
    -Name 'deployResourceGroup' `
    -Location westeurope `
    -TemplateFile "infrastructure\arm\resource-group\template.json" `
    -TemplateParameterFile "infrastructure\arm\resource-group\parameters.json" `
    -rgName $resourceGroupName `

New-AzKeyVault -Name "fides-keyvault" -ResourceGroupName $resourceGroupName -Location "westeurope"

New-AzResourceGroupDeployment `
    -Name deployRegistry `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "infrastructure\arm\registry\template.json" `
    -TemplateParameterFile "infrastructure\arm\registry\parameters.json"

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
