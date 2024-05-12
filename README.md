# Nanoservices Diploma Project

## Getting started

App is available at: <http://smarthome-front.s3-website.eu-central-1.amazonaws.com/>

## Build packages

````sh
cd ./src/backend/service-users
dotnet lambda package --configuration Release --framework net6.0 --output-package bin/Release/net6.0/deploy-package.zip
````

## Deploy backend

````sh
cd ./src/backend
serverless deploy
````

## Deploy frontend

````sh
cd ./src/frontend
serverless deploy
````
