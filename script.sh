#!/bin/bash

az group create --name rg-sandufe --location brazilsouth

docker build -f Dockerfile.sql -t mysql .

az acr create \
    --resource-group rg-sandufe \
    --name mysqlsandufe \
    --sku Standard \
    --location brazilsouth \
    --public-network-enabled true \
    --admin-enabled true

MYSQL_LOGIN_SERVER=$(az acr show --name mysqlsandufe --resource-group rg-sandufe --query loginServer --output tsv)
MYSQL_ADMIN_USERNAME=$(az acr credential show --name mysqlsandufe --resource-group rg-sandufe --query username --output tsv)
MYSQL_ADMIN_PASSWORD=$(az acr credential show --name mysqlsandufe --resource-group rg-sandufe --query passwords[0].value --output tsv)

echo "MySQL ACR Login Server: $MYSQL_LOGIN_SERVER"

az acr login --name mysqlsandufe

docker tag mysql $MYSQL_LOGIN_SERVER/mysql-sandufe:v1
docker push $MYSQL_LOGIN_SERVER/mysql-sandufe:v1

az container create \
    --resource-group rg-sandufe \
    --name mysql-sandufe \
    --image $MYSQL_LOGIN_SERVER/mysql-sandufe:v1 \
    --cpu 1 \
    --memory 2 \
    --registry-login-server $MYSQL_LOGIN_SERVER \
    --registry-username $MYSQL_ADMIN_USERNAME \
    --registry-password $MYSQL_ADMIN_PASSWORD \
    --ports 3306 \
    --os-type Linux \
    --environment-variables MYSQL_ROOT_PASSWORD=senha MYSQL_DATABASE=sandufedb \
    --ip-address Public

MYSQL_IP=$(az container show --resource-group rg-sandufe --name mysql-sandufe --query ipAddress.ip --output tsv)
echo "MySQL IP Público: $MYSQL_IP"


docker build -f Dockerfile.dotnet -t api-dotnet .

az acr create \
    --resource-group rg-sandufe \
    --name dotnetsandufe \
    --sku Standard \
    --location brazilsouth \
    --public-network-enabled true \
    --admin-enabled true

API_LOGIN_SERVER=$(az acr show --name dotnetsandufe --resource-group rg-sandufe --query loginServer --output tsv)
API_ADMIN_USERNAME=$(az acr credential show --name dotnetsandufe --resource-group rg-sandufe --query username --output tsv)
API_ADMIN_PASSWORD=$(az acr credential show --name dotnetsandufe --resource-group rg-sandufe --query passwords[0].value --output tsv)

echo "API ACR Login Server: $API_LOGIN_SERVER"

az acr login --name dotnetsandufe

docker tag api-dotnet $API_LOGIN_SERVER/dotnet-api:v1
docker push $API_LOGIN_SERVER/dotnet-api:v1


az container create \
    --resource-group rg-sandufe \
    --name api-dotnet \
    --image $API_LOGIN_SERVER/dotnet-api:v1 \
    --cpu 1 \
    --memory 2 \
    --registry-login-server $API_LOGIN_SERVER \
    --registry-username $API_ADMIN_USERNAME \
    --registry-password $API_ADMIN_PASSWORD \
    --os-type Linux \
    --ports 8080 \
    --ip-address Public \
    --environment-variables ConnectionStrings__DefaultConnection="Server=$MYSQL_IP;Port=3306;Database=sandufedb;Uid=root;Pwd=senha;"
