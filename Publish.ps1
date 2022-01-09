$Version = "1.0.1"
$PackagesFolder = '.\packages'
$ArtifactsFolder = "build-artifacts"
Write-Host "--> Packaging..." -ForegroundColor Yellow
if (Test-Path -Path $PackagesFolder)
{
    Get-ChildItem -Path .\packages -Include *.* -File -Recurse | foreach {$_.Delete()}    
}
else {
    New-Item -Path . -Name "packages" -ItemType "directory"
}
dotnet pack --no-build --no-restore .\src\Forge\Forge.csproj -o .\packages -c Release /p:Version="$($Version)" /p:OutputPath="..\..\$($ArtifactsFolder)\Forge"
dotnet pack --no-build --no-restore .\src\Forge.Logging\Forge.Logging.csproj -o .\packages -c Release /p:Version="$($Version)" /p:OutputPath="..\..\$($ArtifactsFolder)\Forge.Logging"
dotnet pack --no-build --no-restore .\src\Forge.MediatR.CQRS\Forge.MediatR.CQRS.csproj -o .\packages -c Release /p:Version="$($Version)" /p:OutputPath="..\..\$($ArtifactsFolder)\Forge.MediatR.CQRS"
dotnet pack --no-build --no-restore .\src\Forge.MessageBroker.RabbitMQ\Forge.MessageBroker.RabbitMQ.csproj -o .\packages -c Release /p:Version="$($Version)" /p:OutputPath="..\..\$($ArtifactsFolder)\Forge.MessageBroker.RabbitMQ"
dotnet pack --no-build --no-restore .\src\Forge.Api\Forge.Api.csproj -o .\packages -c Release /p:Version="$($Version)" /p:OutputPath="..\..\$($ArtifactsFolder)\Forge.Api"
Write-Host "--> Packaging [COMPLETED] !" -ForegroundColor Green

Write-Host "--> Pushing packages..." -ForegroundColor Yellow
dotnet nuget push "$($PackagesFolder)\*.nupkg" --source Forge_Feed --api-key $(Get-ChildItem -Path Env:FORGE_KEY).Value 
Write-Host "--> Pushing packages..." -ForegroundColor Green
