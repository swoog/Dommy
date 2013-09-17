set version=1.5.0.44
set deployDirectory=c:\temp\dommydeploy\
set urlApplicationDirectory=\\destination\folder
set urlApplication=%urlApplicationDirectory%Dommy.application

msbuild /p:Configuration=Release /target:clean
msbuild /p:Configuration=Release /p:DeployOnBuild=true /p:TargetZone=LocalIntranet
xcopy /R /Y /E "Dommy.Console\bin\release\*.*" "Dommy.Web\obj\release\Package\PackageTmp"
move Dommy.Web\obj\release\Package\PackageTmp\bin\*.dll Dommy.Web\obj\release\Package\PackageTmp
REM xcopy /R /Y /E "Dommy.Bootstrap\bin\release\*.*" "Dommy.Web\obj\release\Package\PackageTmp"
REM xcopy /R /Y /E "Dommy.Console\bin\release\*.*" "Dommy.Web\obj\release\Package\PackageTmp"
rmdir %deployDirectory%%version%\ /s /q
xcopy /R /Y /E "Dommy.Web\obj\release\Package\PackageTmp" %deployDirectory%%version%\
REM move %deployDirectory%%version%\bin\*.dll %deployDirectory%%version%\
REM xcopy /R /Y /E "Dommy.Console\bin\release\*.*" %deployDirectory%%version%\bin\
REM xcopy /R /Y /E %deployDirectory%%version%\bin\Dommy.Console.exe %deployDirectory%%version%\Dommy.exe
mage -New Application -Processor x86 -ToFile "%deployDirectory%%version%\Dommy.Console.exe.manifest" -name "Dommy.Console" -Version %version% -TrustLevel FullTrust -FromDirectory %deployDirectory%%version%
REM mage -Update "%deployDirectory%%version%\Dommy.Console.exe.manifest" -TrustLevel LocalIntranet
mage -Sign "%deployDirectory%%version%\Dommy.Console.exe.manifest" -CertFile Dommy.Console\Dommy.Console.pfx 
REM mage -Update "%deployDirectory%%version%\Dommy.Console.exe.manifest" -CertFile Dommy.Console\Dommy.Console.pfx 
REM mage -New Deployment -Processor x86 -name "Dommy.Console" -Version %version% -Install true -Publisher "TrollCorp" -AppManifest %deployDirectory%%version%\Dommy.Console.exe.manifest -ToFile %deployDirectory%%version%\Dommy.Console.application
REM mage -Sign "%deployDirectory%%version%\Dommy.Console.application" -CertFile Dommy.Console\Dommy.Console.pfx 
mage -New Deployment -Processor x86 -name "Dommy.Console" -Version %version% -Install true -Publisher "TrollCorp" -ProviderUrl %urlApplication% -AppManifest %deployDirectory%%version%\Dommy.Console.exe.manifest -ToFile %deployDirectory%Dommy.application
mage -Sign "%deployDirectory%Dommy.application" -CertFile Dommy.Console\Dommy.Console.pfx 
REM xcopy /R /Y /E %deployDirectory%Dommy.application %deployDirectory%%version%\
robocopy /E %deployDirectory% %urlApplicationDirectory%

@rem xcopy /R /Y /E "$(SolutionDir)\Dommy.Web\*.*" "$(TargetDir)"

REM mage -Update "c:\temp\dommydeploy\1.5.0.29\Dommy.Console.exe.manifest" -TrustLevel LocalIntranet
