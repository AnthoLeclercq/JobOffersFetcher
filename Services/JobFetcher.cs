using Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Services;

public class JobFetcher
{
    private readonly HttpClient _client;
    private readonly string _bearerToken;

    public JobFetcher(string bearerToken, HttpClient? client = null)
    {
        _bearerToken = bearerToken ?? throw new ArgumentNullException(nameof(bearerToken));
        _client = client ?? new HttpClient();
    }

    public async Task<List<JobOffer>> FetchOffersAsync(string city)
    {
        var locationCode = city.ToLower() switch
        {
            "paris" => "75",
            "rennes" => "35",
            "bordeaux" => "33",
            _ => throw new ArgumentException($"Ville non supportée : {city}")
        };

        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        var url = $"https://api.francetravail.io/partenaire/offresdemploi/v2/offres/search?lieux={locationCode}&range=0-9";

        var response = await _client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("❌ Échec de la requête à l'API.");
            return new List<JobOffer>();
        }

        var json = JsonDocument.Parse(content).RootElement;

        if (!json.TryGetProperty("resultats", out var results))
        {
            Console.WriteLine("⚠️ Pas de propriété 'resultats' dans la réponse.");
            return new List<JobOffer>();
        }

        var list = new List<JobOffer>();
        foreach (var o in results.EnumerateArray())
        {
            list.Add(new JobOffer
            {
                Title = o.GetProperty("intitule").GetString() ?? "",
                Company = o.GetProperty("entreprise").GetProperty("nom").GetString() ?? "",
                ContractType = o.GetProperty("typeContratLibelle").GetString() ?? "",
                Description = o.GetProperty("description").GetString() ?? "",
                Url = o.GetProperty("origineOffre").GetProperty("urlOrigine").GetString() ?? "",
                Location = city,
                DateFetched = DateTime.Now
            });
        }

        return list;
    }
}
