# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
COPY GPMS.Backend.sln ./
COPY GPMS.Backend/*.csproj GPMS.Backend/
COPY GPMS.Backend.Data/*.csproj GPMS.Backend.Data/
COPY GPMS.Backend.Services/*.csproj GPMS.Backend.Services/
RUN dotnet restore --disable-parallel

# Copy the rest of the application code
COPY . .

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose the port your application will run on
# EXPOSE 80

# Start the application
RUN ls -al
ENTRYPOINT ["dotnet", "GPMS.Backend.dll"]
