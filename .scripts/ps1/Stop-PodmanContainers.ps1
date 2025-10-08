# eGlobalShop Podman Cleanup Script
# This script stops and removes all eGlobalShop containers, networks, and volumes
# Usage: .\Stop-PodmanContainers.ps1 [environment]
#   environment: optional parameter (local, dev, or prod) - defaults to 'local'

param(
    [Parameter(Position=0)]
    [ValidateSet("local", "dev", "prod")]
    [string]$Environment = "local"
)

# Import common setup functions
. (Join-Path $PSScriptRoot "Common-PodmanSetup.ps1")

# Initialize the environment and get original location for cleanup
$originalLocation = Initialize-PodmanEnvironment -Environment $Environment -ScriptName "Cleaning up eGlobalShop resources"


try {
    # Stop and remove services
    try {
        podman-compose -f compose.yaml -f compose.infra.yaml -f compose.app.yaml stop
        podman-compose -f compose.yaml -f compose.infra.yaml -f compose.app.yaml down
    } catch {
        # Ignore errors if already stopped/removed
    }

    # Clean up remaining containers
    $containerFilters = @(
        "name=order-service", "name=customers-service", "name=inventory-service",
        "name=billing-service", "name=shipping-service", "name=postgres-db",
        "name=pgadmin", "name=nats", "name=seq"
    )
    
    $filterArgs = $containerFilters | ForEach-Object { "--filter"; $_ }
    $containers = & podman ps -a @filterArgs --format "{{.Names}}" 2>$null
    
    if ($containers) {
        $containers | ForEach-Object {
            try { podman rm -f $_ } catch { }
        }
    }

    # Clean up volumes and networks
    try {
        $volumeExists = podman volume exists seq-data 2>$null
        if ($LASTEXITCODE -eq 0) { podman volume rm seq-data }
    } catch { }

    try {
        $networkExists = podman network exists eglobalshop-network 2>$null
        if ($LASTEXITCODE -eq 0) { podman network rm eglobalshop-network }
    } catch { }

    # Remove dangling volumes
    try {
        $danglingVolumes = podman volume ls -q --filter "dangling=true" 2>$null
        if ($danglingVolumes) {
            $danglingVolumes | ForEach-Object { podman volume rm $_ }
        }
    } catch { }

    Write-Host "✨ Cleanup completed successfully!" -ForegroundColor Green

    # Quick verification
    $verifyFilters = @(
        "name=eglobalshop", "name=order-service", "name=customers-service",
        "name=inventory-service", "name=billing-service", "name=shipping-service",
        "name=postgres-db", "name=pgadmin", "name=nats", "name=seq"
    )
    
    $verifyFilterArgs = $verifyFilters | ForEach-Object { "--filter"; $_ }
    try {
        $remainingContainers = & podman ps -a @verifyFilterArgs --format "{{.Names}}" 2>$null
        if ($remainingContainers) {
            Write-Host "⚠️  Some containers still exist: $($remainingContainers -join ', ')" -ForegroundColor Yellow
        }
    } catch { }

} catch {
    Write-Host "❌ Error during cleanup: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} finally {
    # Always restore the original location
    Set-Location $originalLocation
}