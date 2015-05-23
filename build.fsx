// include Fake lib
#r @"packages/FAKE.3.33.0/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = "./build/"
let packages = 
     ["Dommy.Extensions.Kinect.Sdk1" 
      "Dommy.Extensions.Kinect.Sdk2"]

// Clean target
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "BuildApp" (fun _ ->
    !! "Dommy.sln"
      |> MSBuildRelease null "Build"
      |> Log "AppBuild-Output: "
)

Target "Package" (fun _ ->
    for package in packages do
        !! ("./" + package + "/bin/release/*.*")
        |> Copy (buildDir + package) 
)

// Default target
Target "Default" (fun _ ->
    trace "Build is done"
)

"Clean"
==> "BuildApp"
==> "Package"
==> "Default"

// start build
RunTargetOrDefault "Default"