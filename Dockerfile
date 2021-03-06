FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

ARG VAULT_TOKEN
ENV HTDC_VAULT_TOKEN=$VAULT_TOKEN

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG GITHUB_TOKEN

WORKDIR /src/app

COPY . .

RUN dotnet restore HappyTravel.Hiroshima.Api

RUN dotnet build -c Release HappyTravel.Hiroshima.Api

FROM build AS publish

RUN dotnet publish -c Release -o /app/release HappyTravel.Hiroshima.Api

FROM base AS final

WORKDIR /app
COPY --from=publish /app/release .

ENTRYPOINT ["dotnet", "HappyTravel.Hiroshima.Api.dll"]
