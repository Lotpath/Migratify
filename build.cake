// Get arguments passed to the script

var target = Argument<string>("target", "All");
var configuration = Argument<string>("configuration", "Release");
var buildLabel = Argument("buildLabel", string.Empty);
var buildInfo = Argument("buildInfo", string.Empty);

// Parse release notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

// Set version.
var version = releaseNotes.Version.ToString();
var semVersion = version + (buildLabel != "" ? ("-" + buildLabel) : string.Empty);
Information("Building version {0} of Migratify", version);

// Define directories.
var buildResultDir = "./build";
var testResultsDir = buildResultDir + "/test-results";

//////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	CleanDirectories(new DirectoryPath[] {
		buildResultDir, testResultsDir });    
});

Task("Clean-Solution")
	.IsDependentOn("Clean")
	.Does(() =>
{
	MSBuild("./src/Migratify.sln", s => 
		{ 
			s.Configuration = configuration;
			s.ToolVersion = MSBuildToolVersion.NET40;
			s.WithTarget("Clean");
		});
});

Task("Restore-NuGet-Packages")
	.IsDependentOn("Clean")
	.Does(context =>
{
	// Restore NuGet packages.
	NuGetRestore("./src/Migratify.sln");    
});

Task("Patch-Assembly-Info")
	.Description("Patches the AssemblyInfo files.")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
{
	var file = "./src/SolutionInfo.cs";
	CreateAssemblyInfo(file, new AssemblyInfoSettings {
		Product = "Migratify",
		Version = version,
		FileVersion = version,
		InformationalVersion = (version + buildInfo).Trim(),
		Copyright = "Copyright (c) Lotpath " + DateTime.Now.Year
	});
});

Task("Build-Solution")
	.IsDependentOn("Patch-Assembly-Info")
	.Does(() =>
{
	MSBuild("./src/Migratify.sln", s => 
		{ 
			s.Configuration = configuration;
			s.ToolVersion = MSBuildToolVersion.NET40;
		});
});

Task("Run-Unit-Tests")	
	.IsDependentOn("Build-Solution")
	.Does(() =>
{
	var testPaths = GetFiles("./src/**/bin/" + configuration + "/*.Tests.dll");

	XUnit(testPaths, new XUnitSettings {
		OutputDirectory = testResultsDir,
		XmlReport = true,
		HtmlReport = true
	});
});

Task("Create-NuGet-Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    NuGetPack("./src/Migratify/Migratify.nuspec", new NuGetPackSettings {
        Version = semVersion,
        ReleaseNotes = releaseNotes.Notes.ToArray(),
        Symbols = false,
        NoPackageAnalysis = true,
		OutputDirectory = "./nuget"
    });

});

Task("All")
	.Description("Final target.")
	.IsDependentOn("Create-NuGet-Package")
;

//////////////////////////////////////////////////////////////////////////

RunTarget(target);