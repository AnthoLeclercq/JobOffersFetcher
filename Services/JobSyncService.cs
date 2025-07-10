using Data;
using Models;

namespace Services;

public class JobSyncService
{
    private readonly JobDbContext _db;

    public JobSyncService(JobDbContext db) => _db = db;

    /// <summary>
    /// Enregistre les nouvelles offres en BDD si son URL n'existe pas déjà.
    /// </summary>
    public void SaveOffers(List<JobOffer> offers)
    {
        foreach (var offer in offers)
        {
            if (!_db.JobOffers.Any(o => o.Url == offer.Url))
                _db.JobOffers.Add(offer);
        }

        _db.SaveChanges();
    }
}