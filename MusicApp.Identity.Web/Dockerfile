FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MusicApp.Identity.Web/MusicApp.Identity.Web.csproj", "MusicApp.Identity.Web/"]
COPY ["MusicApp.Identity.Application/MusicApp.Identity.Application.csproj", "MusicApp.Identity.Application/"]
COPY ["MusicApp.Identity.Domain/MusicApp.Identity.Domain.csproj", "MusicApp.Identity.Domain/"]
COPY ["MusicApp.Identity.Infrastructure/MusicApp.Identity.Infrastructure.csproj", "MusicApp.Identity.Infrastructure/"]
RUN dotnet restore "MusicApp.Identity.Web/MusicApp.Identity.Web.csproj"
COPY . .
WORKDIR "/src/MusicApp.Identity.Web"
RUN dotnet build "MusicApp.Identity.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicApp.Identity.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicApp.Identity.Web.dll"]