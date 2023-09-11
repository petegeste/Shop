$cr = $Env:DevContainerRegistry
echo "Building containers and publishing to ${cr}"

docker build shop-db/ -t $cr/shop-db
docker push $cr/shop-db

docker build shop-api/ -t $cr/shop-api
docker push $cr/shop-api

docker build shop-webapp/ -t $cr/shop-webapp
docker push $cr/shop-webapp

docker build shop-api/ -t $cr/shop-api
docker push $cr/shop-api