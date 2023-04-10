# PrjectX solution

### Run
```
dotnet run --project "D:\ProjectX 2.0\backend\Services\ProjectX.Identity\ProjectX.Identity.API" --configuration release

dotnet run --project "D:\ProjectX 2.0\backend\Services\ProjectX.Dashboard\ProjectX.Dashboard.API" --configuration release

dotnet run --project "D:\ProjectX 2.0\backend\Services\ProjectX.Realtime\ProjectX.Realtime.API" --configuration release

dotnet run --project "D:\ProjectX 2.0\backend\Services\ProjectX.Messenger\ProjectX.Messenger.API" --configuration release
```

### Docker Compose
```
docker compose ps

docker compose stop

docker-compose -f services.yml up -d --force-recreate --build <service name>

docker-compose -f infrastructure.yml up -d --force-recreate --build <service name>

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
docker run --name my-jaeger -p 5775:5775/udp -p 5778:5778 -p 6831:6831/udp -p 6832:6832/udp -p 9411:9411 -p 14268:14268 -p 16686:16686 -d --restart=unless-stopped jaegertracing/opentelemetry-all-in-one

jaeger:
  image: jaegertracing/opentelemetry-all-in-one
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
<br>
https://hub.docker.com/r/datalust/seq
```
docker run --name my-seq -e ACCEPT_EULA=y -p 5341:80 -d --restart=unless-stopped datalust/seq

docker run \
  --name seq \
  -d \
  --restart unless-stopped \
  -e ACCEPT_EULA=Y \
  -e SEQ_FIRSTRUN_ADMINPASSWORDHASH="$PH" \
  -v /path/to/seq/data:/data \
  -p 80:80 \
  -p 5341:5341 \
  datalust/seq

seq:
  image: datalust/seq:latest
  environment:
    - ACCEPT_EULA=Y
  ports:
    - "5341:80"
``` 

### RabbitMq
```
docker run -d -p 15672:15672 -p 5672:5672 --name my-rabbit -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest rabbitmq:3-management
```