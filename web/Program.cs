using duelsys.ApplicationLayer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using mysql;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.LoginPath = new PathString("/auth/SignIn");
        options.AccessDeniedPath = new PathString("/auth/SignIn");
    });

// Add services to the container.
builder.Services.AddRazorPages();

string connectionUrl = "Server=localhost;Uid=root;Database=duelsys;Pwd=123456";

var userStore = new UserStore(connectionUrl);
var tournamentStore = new TournamentStore(connectionUrl);
var matchStore = new MatchStore(connectionUrl);
var gameStore = new GameStore(connectionUrl);
var sportStore = new SportStore(connectionUrl);
var tournamentSystemStore = new TournamentSystemStore(connectionUrl);

var aService = new AuthenticationService(userStore);
var tService = new TournamentService(tournamentStore, matchStore, gameStore);
var tsService = new TournamentSystemService(tournamentSystemStore);
var sService = new SportService(sportStore);
var uService = new UserService(userStore);
var mService = new MatchService(matchStore);

builder.Services.AddSingleton(aService);
builder.Services.AddSingleton(tService);
builder.Services.AddSingleton(tsService);
builder.Services.AddSingleton(sService);
builder.Services.AddSingleton(uService);
builder.Services.AddSingleton(mService);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
