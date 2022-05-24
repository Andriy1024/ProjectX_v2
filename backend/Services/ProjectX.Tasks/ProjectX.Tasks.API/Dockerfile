#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./Common ./Common
COPY ./Services/ProjectX.Tasks ./Services/ProjectX.Tasks
RUN dotnet restore "./Services/ProjectX.Tasks/ProjectX.Tasks.API/ProjectX.Tasks.API.csproj"
WORKDIR "/src/Services/ProjectX.Tasks/ProjectX.Tasks.API"
RUN dotnet build "ProjectX.Tasks.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectX.Tasks.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectX.Tasks.API.dll"]