using API;
using API.Middlewares;
using BLL;
using DAL;
using MODEL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddBLLServices();
builder.Services.AddRepositories();
builder.Services.AddApiServices(builder.Configuration);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(builder.Configuration["cors:development"]);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
