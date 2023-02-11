# ProjextX Identity Persistence

### Migrations
```
dotnet ef migrations add "init" -c IdentityDatabase -o "Migrations/Identity"
dotnet ef migrations remove
dotnet ef database update --connection your_connection
```