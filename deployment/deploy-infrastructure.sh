az container create --resource-group Fides --file ./deployment/containers/sql-template.yaml
az container create --resource-group Fides --file ./deployment/containers/rabbitmq-template.yaml
az container create --resource-group Fides --file ./deployment/containers/elk-template.yaml --environment-variables discovery.type=single-node