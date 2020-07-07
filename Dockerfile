FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base

ARG VAULT_TOKEN
ENV HTDC_VAULT_TOKEN=$VAULT_TOKEN

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG GITHUB_TOKEN

WORKDIR /src/app

COPY . .

RUN dotnet restore HappyTravel.Hiroshima.WebApi

RUN dotnet build -c Release HappyTravel.Hiroshima.WebApi

FROM build AS publish

RUN dotnet publish -c Release -o /app/release HappyTravel.Hiroshima.WebApi

FROM base AS final

WORKDIR /app
COPY --from=publish /app/release .

ENTRYPOINT ["dotnet", "HappyTravel.Hiroshima.WebApi.dll"]
