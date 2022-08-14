#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
#Build context must be backend folder
COPY ./Common ./Common/
COPY ./Services/ProjectX.Identity ./Services/ProjectX.Identity/
WORKDIR "/src/Services/ProjectX.Identity/ProjectX.Identity.API"
RUN dotnet restore "ProjectX.Identity.API.csproj"
RUN dotnet build "ProjectX.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectX.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectX.Identity.API.dll"]