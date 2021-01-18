#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["POC.AmazonMQ.Producer/POC.AmazonMQ.Producer.csproj", "POC.AmazonMQ.Producer/"]
RUN dotnet restore "POC.AmazonMQ.Producer/POC.AmazonMQ.Producer.csproj"
COPY . .
WORKDIR "/src/POC.AmazonMQ.Producer"
RUN dotnet build "POC.AmazonMQ.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "POC.AmazonMQ.Producer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "POC.AmazonMQ.Producer.dll"]