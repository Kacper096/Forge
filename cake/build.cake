#tool "dotnet:?package=GitVersion.Tool&version=5.10.3"
#addin nuget:?package=Cake.FileHelpers&version=5.0.0

#load "version.cake"
#load "nuget.cake"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildArtifacts = Directory("..\\build-artifacts");
var packages = Directory("..\\packages");
var solutionFile = File("..\\Forge.sln");
var directories = GetDirectories("..\\src\\*");

Task("Clean").Does(() => CleanDirectory(buildArtifacts));

Task("Restore")
    .IsDependentOn("DirectoryBuildProps")
    .Does(() => {
        DotNetRestore(solutionFile, new DotNetCoreRestoreSettings {
            NoCache = true
        });
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        foreach (var directory in directories)
        {
            var folder = directory.Segments.Last();
            DotNetBuild(directory.FullPath, new DotNetBuildSettings{
                Configuration = configuration,
                NoRestore = true,
                OutputDirectory = System.IO.Path.Combine(buildArtifacts,folder),
            });
        }
    });

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Pack");

RunTarget(target);
