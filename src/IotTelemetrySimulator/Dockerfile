FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src/src/IotTelemetrySimulator
COPY src/IotTelemetrySimulator/*.csproj .
RUN dotnet restore -a $TARGETARCH

WORKDIR /src
COPY . .
WORKDIR /src/src/IotTelemetrySimulator
RUN dotnet publish --no-restore -o /app/publish -a $TARGETARCH

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IotTelemetrySimulator.dll"]
