# ProjextX Tasks project

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
```


### Build Image
Run from project root folder:
```
docker build -f .\backend\Services\ProjectX.Tasks\ProjectX.Tasks.API\Dockerfile --force-rm -t andriy1024/projectx-tasks:latest .\backend

docker login -u andriy1024 -p ${{ secrets.DOCKER_TOKEN }} 

docker push andriy1024/projectx-tasks:latest

docker run -d -p 5555:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_URLS=http://+:80" -P --name ProjectX.Tasks.API andriy1024/projectx-tasks:latest

```

### Migrations
```
dotnet ef migrations add "init" -c TasksDbContext -o "Migrations/Tasks"
dotnet ef migrations remove
dotnet ef database update --connection your_connection
```