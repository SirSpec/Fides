#!/bin/bash
while [ $# -gt 0 ] ; do
  case $1 in
    -RegistryName) RegistryName="$2" ;;
    -ResourceGroupName) ResourceGroupName="$2" ;;
    -KeyVaultName) KeyVaultName="$2" ;;
    -Location) Location="$2" ;;
    -ElkContainerName) ElkContainerName="$2" ;;
    -SqlContainerName) SqlContainerName="$2" ;;
    -RabbitMqContainerName) RabbitMqContainerName="$2" ;;
    -ImageRegistryLoginServer) ImageRegistryLoginServer="$2" ;;

  esac
  shift
done

az acr build --image elasticsearch:v1 \
  --registry $RegistryName \
  --file ./infrastructure/containers/elk/elasticsearch/Dockerfile ./infrastructure/containers/elk/elasticsearch/

az acr build --image kibana:v1 \
  --registry $RegistryName \
  --file ./infrastructure/containers/elk/kibana/Dockerfile ./infrastructure/containers/elk/kibana/

az acr build --image logstash:v1 \
  --registry $RegistryName \
  --file ./infrastructure/containers/elk/logstash/Dockerfile ./infrastructure/containers/elk/logstash/

az deployment group create \
  --name deploySQL \
  --resource-group $ResourceGroupName \
  --template-file "infrastructure\containers\arm\sql-template\template.json" \
  --parameters "infrastructure\containers\arm\sql-template\parameters.json" \
  --parameters vaultName=$KeyVaultName vaultResourceGroupName=$ResourceGroupName location=$Location containerName=$SqlContainerName

az deployment group create \
  --name deployRabbitMQ \
  --resource-group $ResourceGroupName \
  --template-file "infrastructure\containers\arm\rabbitmq-template\template.json" \
  --parameters "infrastructure\containers\arm\rabbitmq-template\parameters.json" \
  --parameters vaultName=$KeyVaultName vaultResourceGroupName=$ResourceGroupName location=$Location containerName=$RabbitMqContainerName

az deployment group create \
  --name deployELK \
  --resource-group $ResourceGroupName \
  --template-file "infrastructure\containers\arm\elk-template\template.json" \
  --parameters "infrastructure\containers\arm\elk-template\parameters.json" \
  --parameters vaultName=$KeyVaultName vaultResourceGroupName=$ResourceGroupName location=$Location containerName=$ElkContainerName imageRegistryLoginServer=$ImageRegistryLoginServer
