FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 3001
EXPOSE 44374

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY XDelivered.Starter.Core.EF.Website/XDelivered.Starter.Core.CosmosDb.Website.csproj XDelivered.Starter.Core.EF.Website/
RUN dotnet restore XDelivered.Starter.Core.CosmosDb.Website/XDelivered.Starter.Core.CosmosDb.Website.csproj
COPY . .
WORKDIR /src/XDelivered.Starter.Core.CosmosDb
RUN dotnet build XDelivered.Starter.Core.CosmosDb.Website.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish XDelivered.Starter.Core.CosmosDb.Website.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "XDelivered.Starter.Core.CosmosDb.Website.dll"]
