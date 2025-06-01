FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RestaurantApi.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create App_Data directory and set permissions
RUN mkdir -p /app/App_Data && \
    chown -R $APP_UID:$APP_UID /app/App_Data && \
    chmod -R 755 /app/App_Data

ENTRYPOINT ["dotnet", "RestaurantApi.dll"] 