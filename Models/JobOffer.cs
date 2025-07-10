namespace Models;

public class JobOffer
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Company { get; set; }
    public string? ContractType { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Location { get; set; }
    public string Country { get; set; } = "France";
    public DateTime DateFetched { get; set; }
}