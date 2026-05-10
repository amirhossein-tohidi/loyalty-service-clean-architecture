using Loyalty.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.ConfigureScalar();
app.GrpcServerConfig();
app.UseExceptionHandler();
app.ConfigureEndpoints();

app.Run();


public partial class Program;