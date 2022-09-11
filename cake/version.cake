var buildNumber = Argument("buildNumber", 0);
var versionFile = File("GitVersion.yml");
var versionRegex = new System.Text.RegularExpressions.Regex(@"next-version:\s* (?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<patch>[0-9]+)");

var globalAssemblyFile = File("..\\GlobalAssemblyInfo.cs");
var globalAssemblyVersionRegex = new System.Text.RegularExpressions.Regex("\\[assembly: AssemblyVersion\\(\"(?<version>.+)\"\\)\\]");
var globalAssemblyFileVersionRegex = new System.Text.RegularExpressions.Regex("\\[assembly: AssemblyFileVersion\\(\"(?<version>.+)\"\\)\\]");

var directoryBuildPropsFile = File("..\\Directory.Build.props");
var propsVersionRegex = new System.Text.RegularExpressions.Regex("\\<Version\\>(?<version>.+)\\</Version\\>");

string version;
string assemblyVersion;
string assemblyFileVersion;

Setup(context => 
{
    var versionInfo = FileReadText(versionFile);
    var versionMatch = versionRegex.Match(versionInfo);
    if (!versionMatch.Success)
        throw new InvalidOperationException("Unknown version from GitVersion.yml");
    
    if (buildNumber == 0 && int.TryParse(versionMatch.Groups["patch"].Value, out var parsed))
        buildNumber = parsed;
    
    version = $"{versionMatch.Groups["major"].Value}.{versionMatch.Groups["minor"].Value}.{buildNumber}";
    assemblyVersion = version + ".0";
    assemblyFileVersion = $"{version}.{buildNumber}";

    Information("Version info:");
    Information(" Version: {0}", version);
    Information(" Assembly version: {0}", assemblyVersion);
    Information(" Assembly file version: {0}", assemblyFileVersion);
});

Task("UpdateAssemblyInfo")
    .IsDependentOn("Clean")
    .Does(() => {
        var assemblyInfo = FileReadText(globalAssemblyFile);
        var versionAssembly = globalAssemblyFileVersionRegex.Replace(assemblyInfo, m => m.Value.Replace(m.Groups["version"].Value, version));
        var fileVersionAssembly = globalAssemblyFileVersionRegex.Replace(assemblyInfo, m => m.Value.Replace(m.Groups["version"].Value, version));

        FileWriteText(globalAssemblyFile, versionAssembly);
        FileWriteText(globalAssemblyFile, fileVersionAssembly);
    });

Task("DirectoryBuildProps")
    .IsDependentOn("UpdateAssemblyInfo")
    .Does(() => 
    {
        var directoryBuildProps = FileReadText(directoryBuildPropsFile);
        directoryBuildProps = propsVersionRegex.Replace(directoryBuildProps, m => m.Value.Replace(m.Groups["version"].Value, version));
        FileWriteText(directoryBuildPropsFile, directoryBuildProps);
    });