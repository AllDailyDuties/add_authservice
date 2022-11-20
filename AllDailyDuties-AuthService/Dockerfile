# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy csproj and restore as distinct layers
COPY . .

#COPY *.csproj ./
RUN dotnet restore
    
# Build project
RUN dotnet publish -c Release -o out
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AllDailyDuties-AuthService.dll"]
