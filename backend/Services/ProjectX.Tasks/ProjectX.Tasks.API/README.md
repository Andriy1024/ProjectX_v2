# ProjextX

## WSL

```
wsl --install 
wsl -l -v
wsl --set-default-version 2

sudo usermod -aG sudo $USER

sudo apt update && sudo apt upgrade

sudo apt-get install ca-certificates curl gnupg lsb-release

curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg


echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update

sudo apt-get install docker-ce docker-ce-cli containerd.io docker-compose-plugin

docker -v

sudo usermod -aG docker $USER

code /etc/docker/daemon.json

{
  "hosts": ["unix:///mnt/wsl/shared-docker/docker.sock"]
}

sudo dockerd

export DOCKER_HOST="unix:///mnt/wsl/shared-docker/docker.sock"

sudo service docker start

docker run --rm hello-world
```
https://dev.to/bowmanjd/install-docker-on-windows-wsl-without-docker-desktop-34m9

## Migrations

```
dotnet ef migrations add Tasks/Init
```


### Docker Doc
```
docker system df //docker disk usage
docker system prune // remove unused data

docker build -f "D:\ProjectX 2.0\backend\Services\ProjectX.Tasks\ProjectX.Tasks.API\Dockerfile" 
    --force-rm -t projectxtasksapi:dev 
    --target base  
    --label "com.microsoft.created-by=visual-studio" 
    --label "com.microsoft.visual-studio.project-name=ProjectX.Tasks.API" 
    "D:\ProjectX 2.0\backend" //build context


docker run -dt 
    -v "C:\Users\Admin\vsdbg\vs2017u5:/remote_debugger:rw" 
    -v "C:\Users\Admin\AppData\Roaming\Microsoft\UserSecrets:/root/.microsoft/usersecrets:ro" 
    -v "C:\Users\Admin\AppData\Roaming\ASP.NET\Https:/root/.aspnet/https:ro" 
    -v "D:\ProjectX 2.0\backend\Services\ProjectX.Tasks\ProjectX.Tasks.API:/app" 
    -v "D:\ProjectX 2.0\backend:/src/" 
    -v "C:\Users\Admin\.nuget\packages\:/root/.nuget/fallbackpackages" 
    -e "ASPNETCORE_LOGGING__CONSOLE__DISABLECOLORS=true" 
    -e "ASPNETCORE_ENVIRONMENT=Development" 
    -e "ASPNETCORE_URLS=https://+:443;http://+:80" 
    -e "DOTNET_USE_POLLING_FILE_WATCHER=1" 
    -e "NUGET_PACKAGES=/root/.nuget/fallbackpackages" 
    -e "NUGET_FALLBACK_PACKAGES=/root/.nuget/fallbackpackages" 
    -P --name ProjectX.Tasks.API 
    --entrypoint tail projectxtasksapi:dev -f /dev/null 
```


### Build Image
```

docker build -f "backend\Services\ProjectX.Tasks\ProjectX.Tasks.API\Dockerfile" --force-rm -t projectx_v2/tasks_api:dev "backend"


docker run -d -p 5555:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_URLS=http://+:80" -P --name ProjectX.Tasks.API projectx_v2/tasks_api:dev

```
