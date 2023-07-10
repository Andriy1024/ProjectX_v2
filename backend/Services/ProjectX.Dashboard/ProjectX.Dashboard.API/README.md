# ProjectX Dashboard

### Docker Doc
```
docker system df //docker disk usage
docker system prune // remove unused data
```


### Build Image
Run from project root folder:
```
docker build -f .\backend\Services\ProjectX.Dashboard\ProjectX.Dashboard.API\Dockerfile --force-rm -t andriy1024/projectx-dashboard:latest .\backend

docker login -u andriy1024 -p ${{ secrets.DOCKER_TOKEN }} 

docker push andriy1024/projectx-dashboard:latest

docker run -d -p 5555:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_URLS=http://+:80" -P --name ProjectX.Dashboard.API andriy1024/projectx-dashboard:latest

```

### Migrations
```
dotnet ef migrations add "migration name" -c DashboardDbContext -o "Migrations/Dashboard"
dotnet ef migrations remove
dotnet ef database update --connection your_connection
```