set version=1.5.0.21
set deployDirectory=c:\temp\dommydeploy\

msbuild /target:clean
msbuild /p:Configuration=Release /p:DeployOnBuild=true
xcopy /R /Y /E "Dommy.Console\bin\release\*.*" "Dommy.Web\obj\release\Package\PackageTmp"
move Dommy.Web\obj\release\Package\PackageTmp\bin\*.dll Dommy.Web\obj\release\Package\PackageTmp
REM xcopy /R /Y /E "Dommy.Bootstrap\bin\release\*.*" "Dommy.Web\obj\release\Package\PackageTmp"
REM xcopy /R /Y /E "Dommy.Console\bin\release\*.*" "Dommy.Web\obj\release\Package\PackageTmp"
xcopy /R /Y /E "Dommy.Web\obj\release\Package\PackageTmp" %deployDirectory%%version%\
REM move %deployDirectory%%version%\bin\*.dll %deployDirectory%%version%\
REM xcopy /R /Y /E "Dommy.Console\bin\release\*.*" %deployDirectory%%version%\bin\
REM xcopy /R /Y /E %deployDirectory%%version%\bin\Dommy.Console.exe %deployDirectory%%version%\Dommy.exe
mage -New Application -Processor x86 -ToFile "%deployDirectory%%version%\Dommy.Console.exe.manifest" -name "Dommy.Console" -Version %version% -FromDirectory %deployDirectory%%version%
mage -Sign "%deployDirectory%%version%\Dommy.Console.exe.manifest" -CertFile Dommy.Console\Dommy.Console_TemporaryKey.pfx 
mage -New Deployment -Processor x86 -Install true -Publisher "TrollCorp" -ProviderUrl "\\AG-MINIPC\Dommy\test\Dommy.application" -AppManifest %deployDirectory%%version%\Dommy.Console.exe.manifest -ToFile %deployDirectory%Dommy.application
mage -Sign "%deployDirectory%Dommy.application" -CertFile Dommy.Console\Dommy.Console_TemporaryKey.pfx 

rem xcopy /R /Y /E "$(SolutionDir)\Dommy.Web\*.*" "$(TargetDir)"