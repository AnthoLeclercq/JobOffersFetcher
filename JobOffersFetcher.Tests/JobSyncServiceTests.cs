using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

namespace JobOffersFetcher.Tests;

public class JobSyncServiceTests
{
    private JobDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<JobDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())  // base unique par test
            .Options;
        return new JobDbContext(options);
    }

    [Fact]
    public async Task SaveOffers_AddsNewOffersToDatabase()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var syncService = new JobSyncService(dbContext);

        var offers = new List<JobOffer>
        {
            new JobOffer { Url = "url1", Title = "Dev", Company = "Comp1" },
            new JobOffer { Url = "url2", Title = "Testeur", Company = "Comp2" }
        };

        // Act
        syncService.SaveOffers(offers);

        // Assert
        var count = await dbContext.JobOffers.CountAsync();
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task SaveOffers_DoesNotAddDuplicateOffers()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        dbContext.JobOffers.Add(new JobOffer { Url = "url1", Title = "Dev", Company = "Comp1" });
        await dbContext.SaveChangesAsync();

        var syncService = new JobSyncService(dbContext);

        var offers = new List<JobOffer>
        {
            new JobOffer { Url = "url1", Title = "Dev", Company = "Comp1" }, // duplicate
            new JobOffer { Url = "url3", Title = "Analyste", Company = "Comp3" }
        };

        // Act
        syncService.SaveOffers(offers);

        // Assert
        var count = await dbContext.JobOffers.CountAsync();
        Assert.Equal(2, count);
    }
}
