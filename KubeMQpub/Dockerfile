FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY KubeMQpub/KubeMQpub.csproj KubeMQpub/
RUN dotnet restore KubeMQpub/KubeMQpub.csproj
COPY . .
WORKDIR /src/KubeMQpub
RUN dotnet build KubeMQpub.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish KubeMQpub.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KubeMQpub.dll"]
