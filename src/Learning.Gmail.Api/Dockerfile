#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Learning.Gmail.Api/Learning.Gmail.Api.csproj", "src/Learning.Gmail.Api/"]
RUN dotnet restore "src/Learning.Gmail.Api/Learning.Gmail.Api.csproj"
COPY . .
WORKDIR "/src/src/Learning.Gmail.Api"
RUN dotnet build "Learning.Gmail.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Learning.Gmail.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Learning.Gmail.Api.dll"]