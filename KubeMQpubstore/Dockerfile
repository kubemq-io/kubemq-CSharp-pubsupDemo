FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY KubeMQpubstore/KubeMQpubstore.csproj KubeMQpubstore/
RUN dotnet restore KubeMQpubstore/KubeMQpubstore.csproj
COPY . .
WORKDIR /src/KubeMQpubstore
RUN dotnet build KubeMQpubstore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish KubeMQpubstore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KubeMQpubstore.dll"]
