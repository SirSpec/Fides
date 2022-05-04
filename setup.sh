#!/bin/bash

echo Create .env
cat << EOF > .env
AZURE_COSMOS_EMULATOR_IP_ADDRESS_OVERRIDE=$(ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1)
EOF
echo "$(cat .env)"

echo Create secrets:
mkdir -p ./secrets
read -p 'Set MsSql SA password: ' mssql_sa_password
printf "%s\n" "MSSQL_SA_PASSWORD=$mssql_sa_password" >> .env

read -p 'RabbitMQ default user: ' rabbitmq_default_user
printf "%s\n" "RABBITMQ_DEFAULT_USER=$rabbitmq_default_user" >> .env

read -p 'RabbitMQ default password: ' rabbitmq_default_password
printf "%s\n" "RABBITMQ_DEFAULT_PASS=$rabbitmq_default_password" >> .env
