using duelsys.ApplicationLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mysql;

namespace desktop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            Application.Run(ServiceProvider.GetRequiredService<Login>());
        }

        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        static IHostBuilder CreateHostBuilder()
        {
            var connectionUrl = "Server=localhost;Uid=root;Database=duelsys;Pwd=123456";

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

            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton(aService);
                    services.AddSingleton(tService);
                    services.AddSingleton(tsService);
                    services.AddSingleton(sService);
                    services.AddSingleton(uService);
                    services.AddSingleton(mService);
                    services.AddTransient<Login>();
                    services.AddTransient<Tournaments>();
                    services.AddTransient<PlayerRegisterer>();
                    services.AddTransient<MatchResultRegister>();
                });
        }
    }
}