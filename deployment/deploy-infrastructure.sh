az container create --resource-group Fides --file ./deployment/containers/sql-template.yaml
az container create --resource-group Fides --file ./deployment/containers/rabbitmq-template.yaml
az container create --resource-group Fides --file ./deployment/containers/elk-template.yaml