FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MusicApp.PlaylistService.Web/MusicApp.PlaylistService.Web.csproj", "MusicApp.PlaylistService.Web/"]
COPY ["MusicApp.PlaylistService.Application/MusicApp.PlaylistService.Application.csproj", "MusicApp.PlaylistService.Application/"]
COPY ["MusicApp.PlaylistService.Domain/MusicApp.PlaylistService.Domain.csproj", "MusicApp.PlaylistService.Domain/"]
COPY ["MusicApp.PlaylistService.Infrastructure/MusicApp.PlaylistService.Infrastructure.csproj", "MusicApp.PlaylistService.Infrastructure/"]
RUN dotnet restore "MusicApp.PlaylistService.Web/MusicApp.PlaylistService.Web.csproj"
COPY . .
WORKDIR "/src/MusicApp.PlaylistService.Web"
RUN dotnet build "MusicApp.PlaylistService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicApp.PlaylistService.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicApp.PlaylistService.Web.dll"]