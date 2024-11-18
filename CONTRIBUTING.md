## Build

1. Clone repo.
2. Install local tools with `dotnet tool restore`

## Testing

Testing against different .NET runtimes is a bit of a chore because we want to let the runtime load the current runtime's Microsoft.Build assemblies. Unfortunately, we can't have the `global.json` set to `net9.0` because the `Microsoft.Build` assemblies are compatible with the .NET 9 runtime, and will try to load them, even in a net8.0 TFM context.

Our current algorithm is

* run `dotnet --version` in the workspace directory
* use the output of that to determine which SDK to load from

So if you set global.json to a 9.0.xxx SDK, you'll _always_ use the 9.x MSBuild libraries, which will require the 9.0 runtime to load.  If you want to test while loading older MSBuilds, you'll need to somehow constraint the tests to 8.0.xxx SDKs, and the easiest way to do this is to make a global.json with a 8.0.xxx version and a  `rollForward: latestPatch` constraint on it.

### Against LTS (net8.0)
1. Run tests with `dotnet run --project .\build\ -- -t Test`
    1. This should chose the `Test:net9.0` target and run against that runtime


### Against STS (net9.0)
1. Change global.json to use net9.0
    ```json
        "sdk": {
            "version": "9.0.100",
            "rollForward": "latestMinor",
            "allowPrerelease": true
        }
    ```
2. Set environment variable `BuildNet9` to `true`
    1. Bash: `export BuildNet9=true`
    2. PowerShell: `$env:BuildNet9 = "true"`
3. Run tests with `dotnet run --project .\build\ -- -t Test`
    1. This should chose the `Test:net9.0` target and run against that runtime

## Release

1. Update version in CHANGELOG.md and add notes
    1. If possible link the pull request of the changes and mention the author of the pull request
2. Create new commit
    1. `git add CHANGELOG.md`
    1. `git commit -m "changelog for v0.45.0"`
3. Make a new version tag (for example, `v0.45.0`)
    1. `git tag v0.45.0`
4. Push changes to the repo.
    1. `git push --atomic origin main v0.45.0`


## Nighty

To use the `-nightly` packages, you'll need to add a custom nuget feed for FSharp.Compiler.Service:

- https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet7/nuget/v3/index.json
