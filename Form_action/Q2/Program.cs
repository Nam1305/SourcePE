using Microsoft.EntityFrameworkCore;
using Q2.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

//Use the connection string below to connect to the database.
var connectionStr = builder.Configuration.GetConnectionString("MyCnn");

builder.Services.AddDbContext<PePrnSum25B5WaContext>(options =>
    options.UseSqlServer(connectionStr)
);

var app = builder.Build();

app.MapRazorPages();
app.Run();
