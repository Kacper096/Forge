$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'

dotnet tool install Cake.Tool --version 2.2.0

dotnet tool restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet cake ./cake/build.cake @args --project=Forge
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }