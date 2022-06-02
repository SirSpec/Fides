FROM kibana:8.2.2

ARG kibanaSystemPassword
ARG elasticsearchUrl

ENV KIBANA_SYSTEM_PASSWORD=${kibanaSystemPassword}
ENV ELASTICSEARCH_URL=${elasticsearchUrl}

COPY ./config/ /usr/share/kibana/config/
