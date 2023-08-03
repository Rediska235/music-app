FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MusicApp.SongService.Web/MusicApp.SongService.Web.csproj", "MusicApp.SongService.Web/"]
COPY ["MusicApp.SongService.Application/MusicApp.SongService.Application.csproj", "MusicApp.SongService.Application/"]
COPY ["MusicApp.SongService.Domain/MusicApp.SongService.Domain.csproj", "MusicApp.SongService.Domain/"]
COPY ["MusicApp.SongService.Infrastructure/MusicApp.SongService.Infrastructure.csproj", "MusicApp.SongService.Infrastructure/"]
RUN dotnet restore "MusicApp.SongService.Web/MusicApp.SongService.Web.csproj"
COPY . .
WORKDIR "/src/MusicApp.SongService.Web"
RUN dotnet build "MusicApp.SongService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicApp.SongService.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MusicApp.SongService.Web.dll"]