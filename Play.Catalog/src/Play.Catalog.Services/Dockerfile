FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src

COPY ["Play.Common/src/Play.Common/Play.Common.csproj", "Play.Common/"]
COPY ["Play.Catalog/src/Play.Catalog.Contracts/Play.Catalog.Contracts.csproj", "Play.Catalog.Contracts/"]
COPY ["Play.Catalog/src/Play.Catalog.Services/Play.Catalog.Services.csproj", "Play.Catalog.Services/"]

RUN dotnet restore "Play.Catalog.Services/Play.Catalog.Services.csproj"

COPY . .

RUN dotnet publish "Play.Catalog/src/Play.Catalog.Services/Play.Catalog.Services.csproj" -c Release -o /published /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app

COPY --from=build /published .
ENTRYPOINT [ "dotnet", "Play.Catalog.Services.dll" ]
