FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
COPY . /source
WORKDIR /source

ARG TARGETARCH

RUN --mount=type=cache,id=nuget-background,target=/root/.nuget/packages \
    dotnet publish CurrencyBackgroundService/CurrencyBackgroundService.csproj \
    -a ${TARGETARCH/amd64/x64} \
    --use-current-runtime \
    --self-contained false \
    -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "CurrencyBackgroundService.dll"]