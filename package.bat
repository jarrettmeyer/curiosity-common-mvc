@echo off
msbuild /t:BuildRelease .\build.msbuild
del *.nupkg
nuget pack .\Curiosity.Common.Mvc\Curiosity.Common.Mvc.csproj -Prop Configuration=Release
