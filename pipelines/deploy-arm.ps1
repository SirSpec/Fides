$resourceGroupName = 'Fides'

$subscription = Get-AzSubscription

New-AzSubscriptionDeployment `
    -Name 'deployResourceGroup' `
    -Location westeurope `
    -TemplateFile "..\arm\resource-group\template.json" `
    -TemplateParameterFile "..\arm\resource-group\parameters.json" `
    -rgName $resourceGroupName `

New-AzResourceGroupDeployment `
    -Name deployRegistry `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "..\arm\registry\template.json" `
    -TemplateParameterFile "..\arm\registry\parameters.json"

New-AzResourceGroupDeployment `
    -Name deployFunction `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile "..\arm\sync-function\template.json" `
    -TemplateParameterFile "..\arm\sync-function\parameters.json" `
    -subscriptionId $subscription.Id
