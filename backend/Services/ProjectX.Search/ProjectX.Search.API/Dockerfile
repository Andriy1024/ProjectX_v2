#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY Directory.Build.props .
COPY Directory.Packages.props .
#Build context must be backend folder
COPY ./Common ./Common/
COPY ./Services/ProjectX.Search ./Services/ProjectX.Search/
WORKDIR "/src/Services/ProjectX.Search/ProjectX.Search.API"
RUN dotnet restore "ProjectX.Search.API.csproj"
RUN dotnet publish "ProjectX.Search.API.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjectX.Search.API.dll"]