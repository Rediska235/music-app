using Hangfire.Dashboard;

namespace MusicApp.SongService.Web.Hangfire;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}