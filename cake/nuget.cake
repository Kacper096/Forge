var packages = Directory("..\\packages");
var nugetUrl = EnvironmentVariable<string>("FORGE_NUGET", "UNDEFINED");
var nugetKey = EnvironmentVariable<string>("FORGE_KEY", "UNDEFINED");

Task("Clean-Nuget")
    .Does(() => CleanDirectory(packages));

Task("Pack")
    .IsDependentOn("Clean-Nuget")
    .IsDependentOn("Build")
    .Does(() => {
       foreach (var directory in directories)
       {
            DotNetPack(directory.FullPath, new DotNetPackSettings {
                Configuration = configuration,
                OutputDirectory = packages,
                NoRestore = true,
            });
       } 
    });

Task("Pack-Push")
    .IsDependentOn("Pack")
    .Does(() => {
        foreach (var package in GetPaths($"{packages}\\*.nupkg"))
        {
            DotNetNuGetPush(package.FullPath, new DotNetNuGetPushSettings {
                Source = nugetUrl,
                ApiKey = nugetKey,
                Interactive = true,
                SkipDuplicate = true,
            });
        }
    });