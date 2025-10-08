# eGlobalShop Podman Build Script
# This script builds all microservices using podman-compose
# Usage: .\Build-Images.ps1 [environment]
#   environment: required parameter (local, dev, or prod)

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("local", "dev", "prod")]
    [string]$Environment = "local"
)

# Import common setup functions
. (Join-Path $PSScriptRoot "Common-PodmanSetup.ps1")

# Initialize the environment and get original location for cleanup
$originalLocation = Initialize-PodmanEnvironment -Environment $Environment -ScriptName "Building eGlobalShop services"


try {
    # Build all services
    & podman-compose -f compose.yaml -f compose.infra.yaml -f compose.app.yaml build

    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Build failed" -ForegroundColor Red
        exit $LASTEXITCODE
    }

    Write-Host "✅ Build completed successfully!" -ForegroundColor Green
}
finally {
    # Always restore the original location
    Set-Location $originalLocation
}