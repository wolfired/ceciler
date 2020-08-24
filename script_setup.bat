@call .\script_const.bat

if exist .\%SLN_NAME%.sln (
    exit /B 0
)

dotnet new sln -o %SLN_PATH%

dotnet new classlib -o .\hooker
del .\hooker\*.cs
if exist .\hooker\hooker.csproj.bak xcopy /Y .\hooker\hooker.csproj.bak .\hooker\hooker.csproj
dotnet sln add .\hooker

dotnet new classlib -o .\unity3d
del .\unity3d\*.cs
dotnet sln add .\unity3d

dotnet new classlib -o .\injecter
del .\injecter\*.cs
dotnet add .\injecter package Mono.Cecil
dotnet add .\injecter package Mono.Options
dotnet sln add .\injecter

dotnet new console -o .\unity3p
del .\unity3p\*.cs
dotnet add .\unity3p reference .\unity3d
dotnet add .\unity3p reference .\hooker
dotnet sln add .\unity3p

dotnet new console -o .\injecter_cmd
del .\injecter_cmd\*.cs
dotnet add .\injecter_cmd reference .\injecter
REM dotnet add .\injecter_cmd reference .\unity3d
dotnet sln add .\injecter_cmd
