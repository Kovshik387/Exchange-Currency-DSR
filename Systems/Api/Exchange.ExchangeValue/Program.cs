using Exchange.Common.Settings;
using Exchange.ExchangeVolute;
using Exchange.ExchangeVolute.Configuration;
using Exchange.Services.Settings.SettingsConfigure;
using Exchange.Context;
using Exchange.Context.Setup;
using Exchange.ExchangeValute.Configuration;
using Exchange.ExchangeValute.Services;

var builder = WebApplication.CreateBuilder(args);

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");

builder.AddAppLogger(mainSettings, logSettings);

builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen().AddCors();
builder.Services.RegisterServices();
builder.Services.AddAppAutoMappers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGrpcService<ValuteService>();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

DbInitializer.Execute(app.Services);

app.Run();
