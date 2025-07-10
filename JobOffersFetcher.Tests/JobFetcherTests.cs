using Models;
using Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System;

namespace JobOffersFetcher.Tests;

public class JobFetcherTests
{
    [Fact]
    public async Task FetchOffersAsync_ReturnsJobOffers_WhenResponseIsValid()
    {
        // Arrange: mock HTTP response content JSON
        var jsonResponse = @"{
            ""resultats"": [
                {
                    ""intitule"": ""Développeur .NET"",
                    ""entreprise"": { ""nom"": ""ACME Corp"" },
                    ""typeContratLibelle"": ""CDI"",
                    ""description"": ""Super poste"",
                    ""origineOffre"": { ""urlOrigine"": ""https://example.com/job/1"" }
                }
            ]
        }";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>("SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(jsonResponse)
           });

        var httpClient = new HttpClient(handlerMock.Object);
        var fetcher = new JobFetcher("fake-token", httpClient);

        // Act
        var result = await fetcher.FetchOffersAsync("Paris");

        // Assert
        Assert.Single(result);
        Assert.Equal("Développeur .NET", result[0].Title);
        Assert.Equal("ACME Corp", result[0].Company);
        Assert.Equal("CDI", result[0].ContractType);
        Assert.Equal("Super poste", result[0].Description);
        Assert.Equal("https://example.com/job/1", result[0].Url);
        Assert.Equal("Paris", result[0].Location);
    }

    [Fact]
    public async Task FetchOffersAsync_ThrowsArgumentException_ForUnsupportedCity()
    {
        var fetcher = new JobFetcher("fake-token", new HttpClient());

        await Assert.ThrowsAsync<ArgumentException>(() => fetcher.FetchOffersAsync("Lyon"));
    }
}
