# eGlobalShop Podman Deployment Script
# This script builds and starts all microservices using podman-compose
# Usage: .\Start-PodmanContainers.ps1 [environment]
#   environment: optional parameter (local, dev, or prod) - defaults to local

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("local", "dev", "prod")]
    [string]$Environment = "local"
)

# Import common setup functions
. (Join-Path $PSScriptRoot "Common-PodmanSetup.ps1")

# Initialize the environment and get original location for cleanup
$originalLocation = Initialize-PodmanEnvironment -Environment $Environment -ScriptName "Deploying eGlobalShop with Podman"


try {
    # Build services first
    & (Join-Path $PSScriptRoot "Build-Images.ps1") $Environment

    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Build failed" -ForegroundColor Red
        exit $LASTEXITCODE
    }

    # Start all services
    & podman-compose -f compose.yaml -f compose.infra.yaml -f compose.app.yaml up -d

    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Services failed to start" -ForegroundColor Red
        exit $LASTEXITCODE
    }

    Write-Host "🎉 All services deployed successfully!" -ForegroundColor Green
    Write-Host ""

    # Show running containers
    & podman ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

    Write-Host ""
    Write-Host "🌐 Services available at:" -ForegroundColor Cyan
    Write-Host "   Order: http://localhost:5000   Inventory: http://localhost:6000   Customer: http://localhost:7000" -ForegroundColor White
    Write-Host "   Shipping: http://localhost:8000   Billing: http://localhost:9000" -ForegroundColor White
    Write-Host "   PostgreSQL: :5432   PgAdmin: http://localhost:5050   Seq: http://localhost:5341" -ForegroundColor White
}
finally {
    # Always restore the original location
    Set-Location $originalLocation
}