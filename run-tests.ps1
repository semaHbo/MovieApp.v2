# Stop on first error
$ErrorActionPreference = "Stop"

Write-Host "Starting test execution..." -ForegroundColor Green

# Build solution
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build

# Run unit tests
Write-Host "Running unit tests..." -ForegroundColor Yellow
dotnet test MovieApp.v2.Tests --filter "Category!=UI&Category!=Integration" --logger "console;verbosity=detailed"

# Run integration tests
Write-Host "Running integration tests..." -ForegroundColor Yellow
dotnet test MovieApp.v2.Tests --filter "Category=Integration" --logger "console;verbosity=detailed"

# Run UI tests (Selenium)
Write-Host "Running UI tests..." -ForegroundColor Yellow
dotnet test MovieApp.v2.Tests --filter "Category=UI" --logger "console;verbosity=detailed"

Write-Host "All tests completed!" -ForegroundColor Green 