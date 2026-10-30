FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Restore first so the package layer is cached until the csproj changes
COPY *.csproj ./
RUN dotnet restore
# Copy the rest and publish a release build
COPY . ./
RUN dotnet publish dotnet-tipotarefa-api.csproj -c Release -o out --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "dotnet-tipotarefa-api.dll"]
