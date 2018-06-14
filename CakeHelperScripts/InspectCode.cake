#tool nuget:?package=JetBrains.ReSharper.CommandLineTools
#addin Cake.Issues
#addin Cake.Issues.InspectCode

var logPath = @"resharper-clt-output.xml";
var buildDirectory = Directory(".");

Task("Analyze-Project")
  .WithCriteria(IsRunningOnWindows())
  .IsDependentOn("Restore-Nuget")
  .Does(() =>
  {
    var settings = new InspectCodeSettings() {
      OutputFile = logPath
    };
    InspectCode(solutionFile.Path, settings);
  });

Task("Analyze-Project-Report")
  .WithCriteria(IsRunningOnWindows())
  .IsDependentOn("Analyze-Project")
  .Does(() =>
  {
    var issues = ReadIssues(
      InspectCodeIssuesFromFilePath(logPath),
      buildDirectory);
    
    var issueMessage = string.Format("Code inspection failed. ");
    var issueDetail = string.Format("{0} issues are found.", issues.Count());
    
    if(issues.Count()>0) {
      if(AppVeyor.IsRunningOnAppVeyor)
        AppVeyor.AddMessage(issueMessage,  AppVeyorMessageCategoryType.Error, issueDetail);
      else
        Information(issueMessage + issueDetail);
    }
    else {
      if(AppVeyor.IsRunningOnAppVeyor)
        AppVeyor.AddMessage("No code issues.",  AppVeyorMessageCategoryType.Information);
      else
        Information("InspectCode : No code issues.");
    }
  });
