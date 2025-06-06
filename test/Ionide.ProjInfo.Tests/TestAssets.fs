module DotnetProjInfo.TestAssets

open FileUtils

type TestAssetProjInfo = {
    ProjDir: string
    AssemblyName: string
    ProjectFile: string
    TargetFrameworks: Map<string, TestAssetProjInfoByTfm>
    ProjectReferences: TestAssetProjInfo list
}

and TestAssetProjInfoByTfm = {
    SourceFiles: string list
    Props: Map<string, string>
}

let sourceFiles sources = {
    SourceFiles = sources
    Props = Map.empty
}

let andProps props x =
    let n = [
        yield!
            x.Props
            |> Map.toList
        yield! props
    ]

    {
        x with
            Props =
                n
                |> Map.ofList
    }

/// old sdk, one net461 lib l1
let ``sample1 OldSdk library`` = {
    ProjDir = "sample1-oldsdk-lib"
    AssemblyName = "Lib1"
    ProjectFile =
        "l1"
        / "l1.fsproj"
    TargetFrameworks =
        Map.ofList [
            "net461",
            sourceFiles [
                "AssemblyInfo.fs"
                "Library.fs"
            ]
        ]
    ProjectReferences = []
}

/// dotnet sdk, one netstandard2.0 lib n1
let ``sample NetSdk library with a bad FSAC cache`` = {
    ProjDir = "sample-netsdk-bad-cache"
    AssemblyName = "n1"
    ProjectFile =
        "n1"
        / "n1.fsproj"
    TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Library.fs" ] ]
    ProjectReferences = []
}

/// dotnet sdk, one netstandard2.0 lib n1
let ``sample2 NetSdk library`` = {
    ProjDir = "sample2-netsdk-lib"
    AssemblyName = "n1"
    ProjectFile =
        "n1"
        / "n1.fsproj"
    TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Library.fs" ] ]
    ProjectReferences = []
}

/// dotnet sdk, one netstandard2.0 lib n1, custom msbuild targets
let ``sample10 NetSdk library with custom targets`` = {
    ProjDir = "sample10-netsdk-custom-targets"
    AssemblyName = "n1"
    ProjectFile =
        "n1"
        / "n1.fsproj"
    TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [] ] // Source files are added by custom targets
    ProjectReferences = []
}

/// dotnet sdk, a netcoreapp2.1 console c1
/// reference:
/// - netstandard2.0 lib l1 (C#)
/// - netstandard2.0 lib l2 (F#)
let ``sample3 Netsdk projs`` = {
    ProjDir = "sample3-netsdk-projs"
    AssemblyName = "c1"
    ProjectFile =
        "c1"
        / "c1.fsproj"
    TargetFrameworks = Map.ofList [ "netcoreapp2.1", sourceFiles [ "Program.fs" ] ]
    ProjectReferences = [
        {
            ProjDir =
                "sample3-netsdk-projs"
                / "l1"
            AssemblyName = "l1"
            ProjectFile =
                "l1"
                / "l1.csproj"
            TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Class1.cs" ] ]
            ProjectReferences = []
        }
        {
            ProjDir =
                "sample3-netsdk-projs"
                / "l2"
            AssemblyName = "l2"
            ProjectFile =
                "l2"
                / "l2.fsproj"
            TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Library.fs" ] ]
            ProjectReferences = []
        }
    ]
}

/// dotnet sdk, m1 library multi tfm:
/// - netstandard2.0 with file LibraryA.fs and prop MyProperty=AAA
/// - net461 with file LibraryB.fs and prop MyProperty=BBB
let ``sample4 NetSdk multi tfm`` = {
    ProjDir = "sample4-netsdk-multitfm"
    AssemblyName = "m1"
    ProjectFile =
        "m1"
        / "m1.fsproj"
    TargetFrameworks =
        Map.ofList [
            "netstandard2.0",
            (sourceFiles [ "LibraryA.fs" ]
             |> andProps [ "MyProperty", "AAA" ])
            "net461",
            (sourceFiles [ "LibraryB.fs" ]
             |> andProps [ "MyProperty", "BBB" ])
        ]
    ProjectReferences = []
}

/// dotnet sdk, a C# netstandard2.0 library l2
let ``sample5 NetSdk CSharp library`` = {
    ProjDir = "sample5-netsdk-lib-cs"
    AssemblyName = "l2"
    ProjectFile =
        "l2"
        / "l2.csproj"
    TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Class1.cs" ] ]
    ProjectReferences = []
}

/// dotnet sdk, a c1 console app (netcoreapp) who reference:
/// - netstandard2.0 l2 library
let ``sample6 Netsdk Sparse/1`` = {
    ProjDir = "sample6-netsdk-sparse"
    AssemblyName = "c1"
    ProjectFile =
        "c1"
        / "c1.fsproj"
    TargetFrameworks = Map.ofList [ "netcoreapp2.1", sourceFiles [ "Program.fs" ] ]
    ProjectReferences = [
        {
            ProjDir =
                "sample6-netsdk-sparse"
                / "l2"
            AssemblyName = "l2"
            ProjectFile =
                "l2"
                / "l2.fsproj"
            TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Library.fs" ] ]
            ProjectReferences = []
        }
    ]
}

/// dotnet sdk, a netstandard2.0 library l1
let ``sample6 Netsdk Sparse/2`` = {
    ProjDir =
        "sample6-netsdk-sparse"
        / "l1"
    AssemblyName = "l1"
    ProjectFile =
        "l1"
        / "l1.fsproj"
    TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Library.fs" ] ]
    ProjectReferences = []
}

/// sln
let ``sample6 Netsdk Sparse/sln`` = {
    ProjDir = "sample6-netsdk-sparse"
    AssemblyName = ""
    ProjectFile = "sample6-netsdk-sparse.sln"
    TargetFrameworks = Map.empty
    ProjectReferences = []
}

/// old sdk, a net461 console MultiProject1
/// reference:
/// - net461 lib Project1A (F#)
/// - net461 lib Project1B (F#)
let ``sample7 legacy framework multi-project`` = {
    ProjDir = "sample7-oldsdk-projs"
    AssemblyName = "MultiProject1"
    ProjectFile =
        "m1"
        / "MultiProject1.fsproj"
    TargetFrameworks = Map.ofList [ "net45", sourceFiles [ "MultiProject1.fs" ] ]
    ProjectReferences = [
        {
            ProjDir =
                "sample7-oldsdk-projs"
                / "a"
            AssemblyName = "Project1A"
            ProjectFile =
                "a"
                / "Project1A.fsproj"
            TargetFrameworks = Map.ofList [ "net45", sourceFiles [ "Project1A.fs" ] ]
            ProjectReferences = []
        }
        {
            ProjDir =
                "sample7-oldsdk-projs"
                / "b"
            AssemblyName = "Project1B"
            ProjectFile =
                "b"
                / "Project1B.fsproj"
            TargetFrameworks = Map.ofList [ "net45", sourceFiles [ "Project1B.fs" ] ]
            ProjectReferences = []
        }
    ]
}

/// legacy framework net461 console project
/// reference:
/// - net461 lib Project1A (F#)
let ``sample7 legacy framework project`` = {
    ProjDir = "sample7-oldsdk-projs"
    AssemblyName = "Project1A"
    ProjectFile =
        "a"
        / "Project1A.fsproj"
    TargetFrameworks = Map.ofList [ "net45", sourceFiles [ "Project1A.fs" ] ]
    ProjectReferences = []
}

/// dotnet sdk, one netstandard2.0 lib n1 with advanced solution explorer configurations
let ``sample8 NetSdk Explorer`` = {
    ProjDir = "sample8-netsdk-explorer"
    AssemblyName = "n1"
    ProjectFile =
        "n1"
        / "n1.fsproj"
    TargetFrameworks =
        Map.ofList [
            "netstandard2.0",
            sourceFiles [
                "LibraryA.fs"
                "LibraryC.fs"
                "LibraryB.fs"
            ]
        ]
    ProjectReferences = []
}

/// dotnet sdk, one netstandard2.0 lib n1, nonstandard obj
let ``sample9 NetSdk library`` = {
    ProjDir = "sample9-netsdk-changed-obj"
    AssemblyName = "n1"
    ProjectFile =
        "n1"
        / "n1.fsproj"
    TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Library.fs" ] ]
    ProjectReferences = []
}

/// dotnet sdk library with ProduceReferenceAssembly=true
let ``NetSDK library with ProduceReferenceAssembly`` = {
    ProjDir = "sample-netsdk-prodref"
    AssemblyName = "l1"
    ProjectFile =
        "l1"
        / "l1.fsproj"
    TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Library.fs" ] ]
    ProjectReferences = []
}


let ``NetSDK library referencing ProduceReferenceAssembly library`` = {
    ProjDir = "sample-netsdk-prodref"
    AssemblyName = "l2"
    ProjectFile =
        "l2"
        / "l2.fsproj"
    TargetFrameworks = Map.ofList [ "netstandard2.0", sourceFiles [ "Library.fs" ] ]
    ProjectReferences = [ ``NetSDK library with ProduceReferenceAssembly`` ]
}

let ``Console app with missing direct Import`` = {
    ProjDir = "missing-import"
    AssemblyName = "missing-import"
    ProjectFile = "missing-import.fsproj"
    TargetFrameworks = Map.ofList [ "net6.0", sourceFiles [ "Program.fs" ] ]
    ProjectReferences = []
}

let ``traversal project`` = {
    ProjDir = "traversal-project"
    AssemblyName = ""
    ProjectFile = "dirs.proj"
    TargetFrameworks = Map.empty
    ProjectReferences = [
        yield ``sample3 Netsdk projs``
        yield! ``sample3 Netsdk projs``.ProjectReferences
    ]
}

let ``sample 11 sln with other project types`` = {
    ProjDir = "sample11-solution-with-other-projects"
    AssemblyName = ""
    ProjectFile = "sample11-solution-with-other-projects.sln"
    TargetFrameworks = Map.empty
    ProjectReferences = []
}

let ``sample 12 slnf with one project`` = {
    ProjDir = "sample12-solution-filter-with-one-project"
    AssemblyName = ""
    ProjectFile = "sample12-solution-filter-with-one-project.slnf"
    TargetFrameworks = Map.empty
    ProjectReferences = [
        {
            ProjDir =
                "sample12-solution-filter-with-one-project"
                / "classlibf2"
            AssemblyName = "lclasslibf2"
            ProjectFile =
                "classlibf2"
                / "classlibf2.fsproj"
            TargetFrameworks = Map.ofList [ "netstandard8.0", sourceFiles [ "Library.fs" ] ]
            ProjectReferences = []
        }
    ]
}

let ``sample 13 sln with solution files`` = {
    ProjDir = "sample13-solution-with-solution-files"
    AssemblyName = ""
    ProjectFile = "sample13-solution-with-solution-files.sln"
    TargetFrameworks = Map.empty
    ProjectReferences = []
}

// A test with a .slnx format solution file
let ``sample 14 slnx solution`` = {
    ProjDir = "sample-14-slnx-solution"
    AssemblyName = ""
    ProjectFile = "sample-14-slnx-solution.slnx"
    TargetFrameworks = Map.empty
    ProjectReferences = []
}
