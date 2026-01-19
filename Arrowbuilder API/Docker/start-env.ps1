#requires -Version 5.1
param(
    [switch]$Rebuild,   # optional: Images neu bauen
    [switch]$Pull       # optional: Images vorab ziehen
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $root

function Write-Info($msg) { Write-Host "[INFO] $msg" -ForegroundColor Cyan }
function Write-Warn($msg) { Write-Host "[WARN] $msg" -ForegroundColor Yellow }
function Write-Err($msg)  { Write-Host "[ERR ] $msg" -ForegroundColor Red }

# 1) Docker Engine prüfen
Write-Info "Prüfe Docker Engine..."
try {
    docker info | Out-Null
} catch {
    Write-Err "Docker Engine ist nicht erreichbar. Bitte Docker Desktop starten und erneut ausführen."
    exit 1
}

# 2) .env und docker-compose.yml prüfen
$envFile = Join-Path $root ".env"
$composeFile = Join-Path $root "docker-compose.yml"

if (!(Test-Path $composeFile)) {
    Write-Err "docker-compose.yml nicht gefunden unter: $composeFile"
    exit 1
}
if (!(Test-Path $envFile)) {
    Write-Err ".env nicht gefunden unter: $envFile"
    Write-Warn "Erstelle Beispiel-.env..."
    @'
DB_HOST=db
DB_PORT=3306
DB_NAME=Arrowbuilder
DB_USER=appuser
DB_PASSWORD=Str0ngAndSecret!
DB_ROOT_PASSWORD=AnotherRootSecret!
'@ | Set-Content -Path $envFile -Encoding UTF8
    Write-Info "Bitte passe die Secrets in .env an und starte das Skript erneut."
    exit 1
}

# 3) Warnung bei version: in compose (Compose v2 ignoriert es)
$composeText = Get-Content $composeFile -Raw
if ($composeText -match '^\s*version\s*:' ) {
    Write-Warn "Compose 'version:' ist obsolet und wird ignoriert. Du kannst die Zeile entfernen."
}

# 4) Optional: Pull/Rebuild
if ($Pull) {
    Write-Info "Pull Images..."
    docker compose --env-file $envFile pull
}
if ($Rebuild) {
    Write-Info "Baue API-Image neu..."
    docker compose --env-file $envFile build --no-cache
}

# 5) Starten
Write-Info "Starte Services via docker compose up -d..."
docker compose --env-file $envFile up -d

# 6) Auf MySQL warten
$mysqlContainer = "arrowbuilder-db"
Write-Info "Warte auf MySQL ($mysqlContainer), bis Port 3306 bereit ist..."
$maxWaitSec = 90
$elapsed = 0
$ready = $false

while ($elapsed -lt $maxWaitSec) {
    try {
        $state = docker inspect -f '{{.State.Health.Status}}' $mysqlContainer 2>$null
        if ($state -eq "healthy") { $ready = $true; break }
    } catch {
        # HealthCheck evtl. nicht aktiviert; fallback: Portprobe
        try {
            $tcp = New-Object Net.Sockets.TcpClient
            $tcp.Connect("localhost", [int](Get-Content $envFile | Select-String -Pattern '^DB_PORT=(\d+)$' | % { $_.Matches[0].Groups[1].Value }))
            $tcp.Close()
            $ready = $true
            break
        } catch { }
    }
    Start-Sleep -Seconds 3
    $elapsed += 3
}

if ($ready) {
    Write-Info "MySQL ist bereit."
} else {
    Write-Warn "MySQL wurde innerhalb von ${maxWaitSec}s nicht 'healthy'. Prüfe Logs."
}

# 7) Statusübersicht
Write-Info "Aktive Container:"
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

Write-Info "Fertig. API erreichbar unter http://localhost:5032 (gemäß docker-compose.yml)."
Write-Info "Zum Stoppen: docker compose down"