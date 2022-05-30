az acr build --image fides/elasticsearch:v1 \
    --registry containerregistryfides \
    --file Dockerfile ./infrastructure/containers/elk/elasticsearch

az acr build --image fides/kibana:v1 \
    --registry containerregistryfides \
    --file Dockerfile ./infrastructure/containers/elk/kibana

az acr build --image fides/logstash:v1 \
    --registry containerregistryfides \
    --file Dockerfile ./infrastructure/containers/elk/logstash

# az container create --resource-group Fides --file ./deployment/containers/sql-template.yaml
# az container create --resource-group Fides --file ./deployment/containers/rabbitmq-template.yaml
# az container create --resource-group Fides --file ./deployment/containers/elk-template.yaml