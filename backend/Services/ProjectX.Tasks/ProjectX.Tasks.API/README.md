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