using Data;

namespace Reports;

public class ReportGenerator
{
    private readonly JobDbContext _db;

    public ReportGenerator(JobDbContext db) => _db = db;

    public void Generate()
    {
        Console.WriteLine("=== Rapport des offres ===");

        if (!_db.JobOffers.Any())
        {
            Console.WriteLine("❗ Aucune offre en base de données.");
            return;
        }

        Console.WriteLine("-- Par type de contrat --");
        foreach (var g in _db.JobOffers.GroupBy(o => o.ContractType))
            Console.WriteLine($"{g.Key}: {g.Count()} offres");

        Console.WriteLine("-- Par entreprise --");
        foreach (var g in _db.JobOffers.GroupBy(o => o.Company))
            Console.WriteLine($"{g.Key}: {g.Count()} offres");

        Console.WriteLine("-- Par pays --");
        foreach (var g in _db.JobOffers.GroupBy(o => o.Country))
            Console.WriteLine($"{g.Key}: {g.Count()} offres");
    }
}
