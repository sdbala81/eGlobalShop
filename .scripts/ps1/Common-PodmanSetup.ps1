# eGlobalShop Common Podman Setup Functions
# This script contains common setup functions used by all Podman deployment scripts

function Initialize-PodmanEnvironment {
    param(
        [Parameter(Mandatory=$true)]
        [ValidateSet("local", "dev", "prod")]
        [string]$Environment,
        
        [Parameter(Mandatory=$false)]
        [string]$ScriptName = "eGlobalShop Podman Script"
    )

    # Enable strict mode to catch errors early
    Set-StrictMode -Version Latest
    $ErrorActionPreference = "Stop"

    Write-Host "🚀 $ScriptName (Environment: $Environment)..." -ForegroundColor Cyan

    # Store the original location
    $originalLocation = Get-Location

    # Navigate to the .podman directory for compose files
    Set-Location (Join-Path $PSScriptRoot "..\..\\.podman")

    # Determine environment file based on environment parameter
    $envFile = switch ($Environment) {
        "local" { "../.env.local" }
        "dev"   { "../.env.dev" }
        "prod"  { "../.env" }
    }

    # Check if environment file exists
    if (-not (Test-Path $envFile)) {
        Write-Host "❌ Error: Environment file $envFile not found" -ForegroundColor Red
        exit 1
    }

    # Load environment variables from file
    Get-Content $envFile | Where-Object { $_ -notmatch '^#' -and $_ -ne '' } | ForEach-Object {
        $parts = $_ -split '=', 2
        if ($parts.Length -eq 2) {
            $name = $parts[0].Trim()
            $value = $parts[1].Trim()
            [Environment]::SetEnvironmentVariable($name, $value, "Process")
        }
    }

    # Return the original location so it can be restored in finally block
    return $originalLocation
}