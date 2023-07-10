
# PostgreSQL

```
docker run -p 5432:5432 --name projectx-postgres -e 'POSTGRES_USER=postgres' -e 'POSTGRES_PASSWORD=root' -d postgres
```

## Connection String
```
"Host=localhost;Database=ProjectX.<DbName>;Username=postgres;Password=root"
```

## psql commands in your console
```
docker exec -it <container-name> /bin/bash

psql -h localhost -U postgres
```

## PdAdmin4
```
docker run -p 5050:80 --name projectx-pgadmin -e 'PGADMIN_DEFAULT_EMAIL=postgres@test.com' -e 'PGADMIN_DEFAULT_PASSWORD=root' -d dpage/pgadmin4

docker network create --driver bridge pgnetwork

docker network connect pgnetwork projectx-postgres

docker network connect pgnetwork projectx-pgadmin

docker network inspect pgnetwork
```

## PdAdmin URL
```
http://localhost:5050/browser/
```
