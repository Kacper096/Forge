var packagesOutput = Directory("..\\packages");

Task("Clean-Nuget")
    .Does(() => CleanDirectory(packagesOutput));

Task("Pack")
    .IsDependentOn("Clean-Nuget")
    .IsDependentOn("Build")
    .Does(() => {
       foreach (var directory in directories)
       {
            DotNetPack(directory.FullPath, new DotNetPackSettings {
                Configuration = configuration,
                OutputDirectory = packagesOutput,
                NoRestore = true,
            });
       } 
    });