FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY KubeMQsubstore/KubeMQsubstore.csproj KubeMQsubstore/
RUN dotnet restore KubeMQsubstore/KubeMQsubstore.csproj
COPY . .
WORKDIR /src/KubeMQsubstore
RUN dotnet build KubeMQsubstore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish KubeMQsubstore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KubeMQsubstore.dll"]
