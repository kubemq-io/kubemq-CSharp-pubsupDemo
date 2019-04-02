FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY KubeMQsub/KubeMQsub.csproj KubeMQsub/
RUN dotnet restore KubeMQsub/KubeMQsub.csproj
COPY . .
WORKDIR /src/KubeMQsub
RUN dotnet build KubeMQsub.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish KubeMQsub.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KubeMQsub.dll"]
