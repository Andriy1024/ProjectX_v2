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
```


### Build Image
Run from project root folder:
```
docker build -f .\backend\Services\ProjectX.Tasks\ProjectX.Tasks.API\Dockerfile --force-rm -t andriy1024/projectx-tasks:latest .\backend

docker login -u andriy1024 -p ${{ secrets.DOCKER_TOKEN }} 

docker push andriy1024/projectx-tasks:latest

docker run -d -p 5555:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_URLS=http://+:80" -P --name ProjectX.Tasks.API andriy1024/projectx-tasks:latest

```

### Jaeger
https://www.jaegertracing.io/docs/1.6/deployment/
<br>
https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
<br>
UI at http://localhost:16686
<br>
Export endpoint http://localhost:6831 //env: OTEL_EXPORTER_JAEGER_ENDPOINT
```
docker run --name jaeger -p 5775:5775/udp -p 5778:5778 -p 6831:6831/udp -p 6832:6832/udp -p 9411:9411 -p 14268:14268 -p 16686:16686 -d --restart=unless-stopped jaegertracing/opentelemetry-all-in-one

jaeger:
    image: jaegertracing/all-in-one
    container_name: jaeger
    restart: unless-stopped
    ports:
      - 5775:5775/udp
      - 5778:5778
      - 6831:6831/udp
      - 6832:6832/udp
      - 9411:9411
      - 14268:14268
      - 16686:16686
```

### Seq
http://localhost:5341/
```
docker run --rm -it -e ACCEPT_EULA=y -p 5341:80 datalust/seq
docker run -d -e ACCEPT_EULA=y -p 5341:80 datalust/seq
```