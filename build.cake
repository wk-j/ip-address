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
        Zip($"publish/MyIpAddress", "publish/my-ip-address.0.1.0.zip");
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