#addin nuget:?package=ProjectParser&version=0.3.0

using ProjectParser;

var name = "MyIpAddress";
var project = $"src/{name}/{name}.csproj";

Task("Publish")
    .Does(() => {
        CleanDirectory("publish");
        DotNetCorePublish(project, new DotNetCorePublishSettings {
            OutputDirectory = "publish/MyIpAddress"
        });
    });

Task("Zip")
    .IsDependentOn("Publish")
    .Does(() => {
        var info = Parser.Parse(project);
        Zip($"publish/MyIpAddress", $"publish/my-ip-address.{info.Version}.zip");
    });


Task("Build").Does(() => {
    MSBuild(project, settings => {
        settings.WithTarget("Build");
    });
});

Task("Run")
    .IsDependentOn("Build")
    .Does(() => {
        MSBuild(project, settings => {
            settings.WithTarget("Run");
        });
});

var target = Argument("target", "default");
RunTarget(target);