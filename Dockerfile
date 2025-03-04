FROM oven/bun:1.2.4 AS build-react
WORKDIR /app
COPY src/Api/ClientApp/package.json .
RUN bun install
COPY src/Api/ClientApp .
RUN bun run build

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-dotnet
WORKDIR /src

COPY *.sln .
COPY src/Api/Api.csproj src/Api/
COPY src/Shared/Shared.csproj src/Shared/

RUN dotnet restore src/Api/Api.csproj

COPY src/Api/. src/Api/
COPY src/Shared/. src/Shared/

RUN dotnet publish src/Api/Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build-dotnet /app/publish .
COPY --from=build-react /app/build ./wwwroot
ENTRYPOINT ["dotnet", "Api.dll"]