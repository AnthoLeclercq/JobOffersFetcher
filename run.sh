#!/bin/bash

if [ "$1" == "test" ]; then
  echo "Restoring and running tests..."
  dotnet restore JobOffersFetcher.Tests/JobOffersFetcher.Tests.csproj
  dotnet build JobOffersFetcher.Tests/JobOffersFetcher.Tests.csproj
  dotnet test JobOffersFetcher.Tests/JobOffersFetcher.Tests.csproj
else
  echo "Restoring and running application..."
  dotnet restore JobOffersFetcher.csproj
  dotnet build JobOffersFetcher.csproj
  dotnet run --project JobOffersFetcher.csproj
fi