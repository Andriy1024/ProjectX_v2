#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
#Build context must be backend folder
COPY ./Common ./Common/
COPY ./Services/ProjectX.FileStorage ./Services/ProjectX.FileStorage/
WORKDIR "/src/Services/ProjectX.FileStorage/ProjectX.FileStorage.API"
RUN dotnet restore "ProjectX.FileStorage.API.csproj"
RUN dotnet build "ProjectX.FileStorage.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectX.FileStorage.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectX.FileStorage.API.dll"]