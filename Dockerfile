FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["to-do-api.csproj", "./"]
RUN dotnet restore "./to-do-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "to-do-api.csproj" -c Release -o /app/build

# Add dotnet ef tool
RUN dotnet tool install --global dotnet-ef

# Run migration command
# RUN dotnet ef database update

FROM build AS publish
RUN dotnet publish "to-do-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "to-do-api.dll","--environment=Development"]