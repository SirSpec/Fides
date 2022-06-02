az acr build --image elasticsearch:v1 \
  --registry containerregistryfides \
  --file ./infrastructure/containers/elk/elasticsearch/Dockerfile ./infrastructure/containers/elk/elasticsearch/

az acr build --image kibana:v1 \
  --registry containerregistryfides \
  --file ./infrastructure/containers/elk/kibana/Dockerfile ./infrastructure/containers/elk/kibana/

az acr build --image logstash:v1 \
  --registry containerregistryfides \
  --file ./infrastructure/containers/elk/logstash/Dockerfile ./infrastructure/containers/elk/logstash/

az deployment group create \
  --name deploySQL \
  --resource-group Fides \
  --template-file "infrastructure\containers\arm\sql-template\template.json" \
  --parameters "infrastructure\containers\arm\sql-template\parameters.json"

az deployment group create \
  --name deployRabbitMQ \
  --resource-group Fides \
  --template-file "infrastructure\containers\arm\rabbitmq-template\template.json" \
  --parameters "infrastructure\containers\arm\rabbitmq-template\parameters.json"

az deployment group create \
  --name deployELK \
  --resource-group Fides \
  --template-file "infrastructure\containers\arm\elk-template\template.json" \
  --parameters "infrastructure\containers\arm\elk-template\parameters.json"
