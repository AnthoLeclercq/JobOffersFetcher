using Data;
using Reports;
using Services;
using Microsoft.Extensions.Configuration;

namespace JobOffersFetcher
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("==== Début du programme ====");

            // Chargement de la configuration depuis appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            using var db = new JobDbContext();
            db.Database.EnsureCreated();

            // Récupération du token depuis la config
            var token = configuration.GetSection("ApiSettings")["BearerToken"]
                        ?? throw new ArgumentException("BearerToken manquant dans la configuration.");

            var fetcher = new JobFetcher(token);
            var syncService = new JobSyncService(db);

            var cities = new[] { "Paris", "Rennes", "Bordeaux" };
            foreach (var city in cities)
            {
                var offers = await fetcher.FetchOffersAsync(city);
                syncService.SaveOffers(offers);
            }

            Console.WriteLine("→ Génération du rapport...");
            var reporter = new ReportGenerator(db);
            reporter.Generate();

            Console.WriteLine("==== Fin du programme ====");
        }
    }
}
