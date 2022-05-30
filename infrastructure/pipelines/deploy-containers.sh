az acr build --image fides/elasticsearch:v1 \
    --registry containerregistryfides \
    --file ./infrastructure/containers/elk/elasticsearch/Dockerfile .

az acr build --image fides/kibana:v1 \
    --registry containerregistryfides \
    --file ./infrastructure/containers/elk/kibana/Dockerfile .

az acr build --image fides/logstash:v1 \
    --registry containerregistryfides \
    --file ./infrastructure/containers/elk/logstash/Dockerfile .

# az container create --resource-group Fides --file ./deployment/containers/sql-template.yaml
# az container create --resource-group Fides --file ./deployment/containers/rabbitmq-template.yaml
# az container create --resource-group Fides --file ./deployment/containers/elk-template.yaml