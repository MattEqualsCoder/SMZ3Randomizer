using Randomizer.Multiworld.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
var app = builder.Build();

app.MapHub<MultiworldHub>("/multiworld");

app.Run();
