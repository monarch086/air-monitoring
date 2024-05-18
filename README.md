# Nanoservices Diploma Project

## Getting started

App is available at: <http://air.monitoring.s3-website.eu-central-1.amazonaws.com>

## Build packages

````sh
cd ./src/backend
./build.ps1
````

## Deploy backend

````sh
cd ./src/backend
serverless deploy
````

## Run frontend

````sh
cd ./src/frontend
npm i
npm start
````

The frontend will run on <http://localhost:4201/>

## Deploy frontend

````sh
cd ./src/frontend
npm run build
serverless deploy
````
