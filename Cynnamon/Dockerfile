FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Cynnamon/Cynnamon.csproj", "Cynnamon/"]
RUN dotnet restore "Cynnamon/Cynnamon.csproj"
COPY . .
WORKDIR "/src/Cynnamon"
RUN dotnet build "Cynnamon.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cynnamon.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cynnamon.dll"]
