#load "CakeHelperScripts/InspectCode.cake"
#load "CakeHelperScripts/NativeScriptDownloader.cake"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// --------------------------------------------------------------------------------
// FILES & DIRECTORIES
// --------------------------------------------------------------------------------

var solutionFile = File("SafeApp.sln");

var coreTestProject = File("SafeApp.Tests.Core/SafeApp.Tests.Core.csproj");
var coreTestBin = Directory("SafeApp.Tests.Core/bin") + Directory(configuration);

// --------------------------------------------------------------------------------
// PREPARATION
// --------------------------------------------------------------------------------

Task("Clean")
  .Does(() => {
    CleanDirectory(coreTestBin);
  });

Task("Restore-NuGet")
  .IsDependentOn("Clean")
  .Does(() => {
    NuGetRestore(solutionFile);
  });

// --------------------------------------------------------------------------------
// Desktop
// --------------------------------------------------------------------------------

Task("Build-Desktop")
  .IsDependentOn("Restore-NuGet")
  .Does(() => {
    var dotnetBuildArgument = string.Empty;
    var osFamily = (int)Context.Environment.Platform.Family;
    if(osFamily == 1)
      dotnetBuildArgument = @"--runtime win10-x64";
    else if(osFamily == 2)
      dotnetBuildArgument = @"--runtime linux-x64";
    else if (osFamily == 3)
      dotnetBuildArgument = @"--runtime win-x64";
    
    DotNetCoreBuild(coreTestProject,
    new DotNetCoreBuildSettings() {
      Configuration = configuration,
      ArgumentCustomization = args => args.Append(dotnetBuildArgument)
      });
  });

Task("Run-Tests")
  .IsDependentOn("Build-Desktop")
  .Does(() => {
      DotNetCoreTest(
        coreTestProject.Path.FullPath,
        new DotNetCoreTestSettings()
        {
          Configuration = configuration,
          ArgumentCustomization = args => args.Append("--logger \"trx;LogFileName=results.xml\"")
        });
  })
  .Finally(() =>
  {  
    var resultsFile = File("SafeApp.Tests.Core/TestResults/results.xml");
    if(AppVeyor.IsRunningOnAppVeyor)
    {
      AppVeyor.UploadTestResults(resultsFile.Path.FullPath, AppVeyorTestResultsType.MSTest);
    }
  });

Task("Default")
  .IsDependentOn("UnZip-Libs")
  .IsDependentOn("Analyze-Project-Report")
  .IsDependentOn("Run-Tests")
  .Does(() => {

  });

RunTarget(target);
