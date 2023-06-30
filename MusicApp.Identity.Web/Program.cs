using MusicApp.Identity.DataAccess;
using MusicApp.Identity.BusinessLogic;
using MusicApp.Identity.BusinessLogic.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.Services.SeedDatabaseAsync();

app.Run();
