using battleshipGPT.Hubs;
using battleshipGPT.Serives;
using battleshipGPT.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();

builder.Services.AddSingleton<RoomService>();
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<ShipChooseService>();
builder.Services.AddSingleton<LogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ShipChoose}/{id?}");

app.MapHub<ShipChooseHub>("/choose");
app.MapHub<GameHub>("/game");

app.Run();
