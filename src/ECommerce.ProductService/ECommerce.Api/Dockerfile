FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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
ENV POSTGRES_HOST=localhost
ENV POSTGRES_PORT=5432
ENV POSTGRES_DATABASE=ecommerce
ENV POSTGRES_USERNAME=db
ENV POSTGRES_PASSWORD=1234
ENTRYPOINT ["dotnet", "ECommerce.Api.dll"]
