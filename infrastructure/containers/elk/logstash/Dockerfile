FROM logstash:8.2.2

ENV LS_JAVA_OPTS='-Xms512m -Xmx512m'
ENV LOGSTASH_INTERNAL_PASSWORD=password
ENV ELASTICSEARCH_URL='fides-elk.germanywestcentral.azurecontainer.io:9200'

COPY ./config/ /usr/share/logstash/config/
COPY ./pipeline/ /usr/share/logstash/pipeline/
