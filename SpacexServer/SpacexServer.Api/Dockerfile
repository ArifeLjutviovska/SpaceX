# Stage 1: Build and Publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /source

# Copy only the .csproj file and restore dependencies
COPY ../../SpacexServer/SpacexServer.Api/SpacexServer.Api.csproj SpacexServer.Api/
COPY ../../SpacexServer/SpacexServer.Common/SpacexServer.Common.csproj SpacexServer.Common/
COPY ../../SpacexServer/SpacexServer.Services/SpacexServer.Services.csproj SpacexServer.Services/
COPY ../../SpacexServer/SpacexServer.Contracts/SpacexServer.Contracts.csproj SpacexServer.Contracts/
COPY ../../SpacexServer/SpacexServer.Entities/SpacexServer.Entities.csproj SpacexServer.Entities/
COPY ../../SpacexServer/SpacexServer.Storage/SpacexServer.Storage.csproj SpacexServer.Storage/
RUN dotnet restore "SpacexServer.Api/SpacexServer.Api.csproj"

# Copy the rest of the source code and build the project
COPY ../../SpacexServer/SpacexServer.Api/ SpacexServer.Api/
COPY ../../SpacexServer/SpacexServer.Common/ SpacexServer.Common/
COPY ../../SpacexServer/SpacexServer.Services/ SpacexServer.Services/
COPY ../../SpacexServer/SpacexServer.Contracts/ SpacexServer.Contracts/
COPY ../../SpacexServer/SpacexServer.Entities/ SpacexServer.Entities/
COPY ../../SpacexServer/SpacexServer.Storage/ SpacexServer.Storage/
WORKDIR /source/SpacexServer.Api
RUN dotnet publish "SpacexServer.Api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Set the entrypoint to run the application
ENTRYPOINT ["dotnet", "SpacexServer.Api.dll"]