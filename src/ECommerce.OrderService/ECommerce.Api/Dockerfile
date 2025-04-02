FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7070
EXPOSE 7071

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ECommerce.Api/ECommerce.Api.csproj", "ECommerce.Api/"]
RUN dotnet restore "ECommerce.Api/ECommerce.Api.csproj"
COPY . .
WORKDIR "/src/ECommerce.Api"
RUN dotnet build "ECommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommerce.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV MONGODB_HOST=localhost
ENV MONGODB_PORT=27017
ENV MONGODB_DATABASE=OrdersDatabase
ENV UsersMicroserviceName=localhost
ENV UsersMicroservicePort=9090
ENTRYPOINT ["dotnet", "ECommerce.Api.dll"]
