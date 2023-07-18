### Build Image
Run from project root folder:
```
docker build -f .\backend\Services\ProjectX.Dashboard\ProjectX.Dashboard.API\Dockerfile --force-rm -t andriy1024/projectx-dashboard:latest .\backend

docker login -u andriy1024 -p ${{ secrets.DOCKER_TOKEN }} 

docker push andriy1024/projectx-dashboard:latest

docker run -d -p 5555:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_URLS=http://+:80" -P --name ProjectX.Dashboard.API andriy1024/projectx-dashboard:latest

```

### Build File Storage Image
Run from project root folder:
```
docker build -f .\backend\Services\ProjectX.FileStorage\ProjectX.FileStorage.API\Dockerfile --force-rm -t andriy1024/projectx-filestorage:latest .\backend

docker push andriy1024/projectx-filestorage:latest

docker run -d -p 5556:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_URLS=http://+:80" -P --name ProjectX.FileStorage.API andriy1024/projectx-filestorage:latest

```
