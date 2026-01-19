# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["Arrowbuilder API/ArrowBuilder API.csproj", "Arrowbuilder API/"]
COPY ["ORM/ORM.csproj", "ORM/"]
COPY ["Models/Models.csproj", "Models/"]

RUN dotnet restore "Arrowbuilder API/ArrowBuilder API.csproj"

COPY . .

RUN dotnet publish "Arrowbuilder API/ArrowBuilder API.csproj" -c Release -o /app

# Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "ArrowBuilder API.dll"]