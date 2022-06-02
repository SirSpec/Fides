FROM logstash:8.2.2

ARG logstashInternalPassword
ARG elasticsearchUrl

ENV LS_JAVA_OPTS='-Xms512m -Xmx512m'
ENV LOGSTASH_INTERNAL_PASSWORD=${logstashInternalPassword}
ENV ELASTICSEARCH_URL=${elasticsearchUrl}

COPY ./config/ /usr/share/logstash/config/
COPY ./pipeline/ /usr/share/logstash/pipeline/
