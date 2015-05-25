// include Fake lib
#r @"packages/FAKE.3.33.0/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = "./build/"
let apps =
     ["Dommy.Console"
      "Dommy.Console.x86"]
let packages = 
     ["Dommy.Extensions.Kinect.Sdk1" 
      "Dommy.Extensions.Kinect.Sdk2"
      "Dommy.Extensions.UsbUirt"
      "Dommy.Extensions.UsbUirt.x86"]

// Clean target
Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "BuildApp" (fun _ ->
    !! "Dommy.sln"
      |> MSBuildRelease null "Build"
      |> Log "AppBuild-Output: "
)

Target "Apps" (fun _ ->
    for app in apps do
        !! ("./" + app + "/bin/release/*.*")
        |> Copy (buildDir + "/apps/" + app) 
)

Target "Package" (fun _ ->
    for package in packages do
        !! ("./" + package + "/bin/release/*.*")
        -- ("./" + package + "/bin/release/Dommy.Business.*")
        -- ("./" + package + "/bin/release/Ninject.*")
        -- ("./" + package + "/bin/release/CassiniDev4*")
        -- ("./" + package + "/bin/release/Microsoft.AspNet*")
        -- ("./" + package + "/bin/release/Microsoft.Owin*")
        -- ("./" + package + "/bin/release/log4net*")
        -- ("./" + package + "/bin/release/newtonsoft*")
        -- ("./" + package + "/bin/release/roslyn*")
        -- ("./" + package + "/bin/release/owin*")
        -- ("./" + package + "/bin/release/castle*")
        -- ("./" + package + "/bin/release/jetbrain*")
        |> Copy (buildDir + package) 
)

// Default target
Target "Default" (fun _ ->
    trace "Build is done"
)

"Clean"
==> "BuildApp"
==> "Apps"
==> "Package"
==> "Default"

// start build
RunTargetOrDefault "Default"