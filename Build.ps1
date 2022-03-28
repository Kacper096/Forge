$Version = "1.0.1"
$ArtifactsFolder = "build-artifacts"

Write-Host "--> Building..." -ForegroundColor Yellow
if (Test-Path -Path $ArtifactsFolder)
{
    Get-ChildItem -Path $ArtifactsFolder -Include *.* -File -Recurse | foreach {$_.Delete()}    
}
else {
    New-Item -Path . -Name $ArtifactsFolder -ItemType "directory"
}
dotnet restore
dotnet build ".\src\Forge" -c Release -o "$($ArtifactsFolder)\Forge"
dotnet build ".\src\Forge.Logging" -c Release -o "$($ArtifactsFolder)\Forge.Logging"
dotnet build ".\src\Forge.MediatR.CQRS" -c Release -o "$($ArtifactsFolder)\Forge.MediatR.CQRS"
dotnet build ".\src\Forge.MessageBroker.RabbitMQ" -c Release -o "$($ArtifactsFolder)\Forge.MessageBroker.RabbitMQ"
dotnet build ".\src\Forge.Persistence.InfluxDb" -c Release -o "$($ArtifactsFolder)\Forge.Persistence.InfluxDb"
dotnet build ".\src\Forge.Persistence.Redis" -c Release -o "$($ArtifactsFolder)\Forge.Persistence.Redis"
dotnet build ".\src\Forge.Api" -c Release -o "$($ArtifactsFolder)\Forge.Api"

Write-Host "--> Building [COMPLETED] !" -ForegroundColor Green
