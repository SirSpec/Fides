az acr build --image fides/elasticsearch:$(Build.BuildId) \
    --registry containerregistryfides \
    --file Dockerfile ./deployment/containers/elk/elasticsearch

az acr build --image fides/kibana:$(Build.BuildId) \
    --registry containerregistryfides \
    --file Dockerfile ./deployment/containers/elk/kibana

az acr build --image fides/logstash:$(Build.BuildId) \
    --registry containerregistryfides \
    --file Dockerfile ./deployment/containers/elk/logstash

# az container create --resource-group Fides --file ./deployment/containers/sql-template.yaml
# az container create --resource-group Fides --file ./deployment/containers/rabbitmq-template.yaml
# az container create --resource-group Fides --file ./deployment/containers/elk-template.yaml