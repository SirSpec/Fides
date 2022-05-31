FROM kibana:8.2.2

ENV KIBANA_SYSTEM_PASSWORD=password
ENV ELASTICSEARCH_URL='http://fides-elk.germanywestcentral.azurecontainer.io:9200/'

COPY ./config/ /usr/share/kibana/config/
