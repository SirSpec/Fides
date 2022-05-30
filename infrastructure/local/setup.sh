#!/bin/bash

echo Create .env
cat << EOF > .env
ELASTIC_VERSION=8.2.2
EOF
echo "$(cat .env)"

echo Set .env variables
read -p 'Set MsSql SA password(must contain uppercase, lowercase, digit and non-alphanumeric): ' mssql_sa_password
printf "%s\n" "MSSQL_SA_PASSWORD=$mssql_sa_password" >> .env

read -p 'RabbitMQ default user: ' rabbitmq_default_user
printf "%s\n" "RABBITMQ_DEFAULT_USER=$rabbitmq_default_user" >> .env

read -p 'RabbitMQ default password: ' rabbitmq_default_password
printf "%s\n" "RABBITMQ_DEFAULT_PASS=$rabbitmq_default_password" >> .env

read -p 'SqlPad admin user: ' sqlpad_admin
printf "%s\n" "SQLPAD_ADMIN=$sqlpad_admin" >> .env

read -p 'SqlPad admin password: ' sqlpad_admin_password
printf "%s\n" "SQLPAD_ADMIN_PASSWORD=$sqlpad_admin_password" >> .env

read -p 'Elastic password: ' elastic_password
printf "%s\n" "ELASTIC_PASSWORD=$elastic_password" >> .env

read -p 'Logstash internal password: ' logstash_internal_password
printf "%s\n" "LOGSTASH_INTERNAL_PASSWORD=$logstash_internal_password" >> .env

read -p 'Kibana system password: ' kibana_system_password
printf "%s\n" "KIBANA_SYSTEM_PASSWORD=$kibana_system_password" >> .env
