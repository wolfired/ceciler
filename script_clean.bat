@call .\script_const.bat

if exist %SLN_BIN% rd /Q /S %SLN_BIN%
if exist %SLN_OBJ% rd /Q /S %SLN_OBJ%
if exist .\%SLN_NAME%.sln del .\%SLN_NAME%.sln

if exist .\injecter_cmd\bin rd /Q /S .\injecter_cmd\bin
if exist .\injecter_cmd\obj rd /Q /S .\injecter_cmd\obj
if exist .\injecter_cmd\*.csproj del .\injecter_cmd\*.csproj

if exist .\injecter\bin rd /Q /S .\injecter\bin
if exist .\injecter\obj rd /Q /S .\injecter\obj
if exist .\injecter\*.csproj del .\injecter\*.csproj

if exist .\unity3p\bin rd /Q /S .\unity3p\bin
if exist .\unity3p\obj rd /Q /S .\unity3p\obj
if exist .\unity3p\*.csproj del .\unity3p\*.csproj

if exist .\unity3d\bin rd /Q /S .\unity3d\bin
if exist .\unity3d\obj rd /Q /S .\unity3d\obj
if exist .\unity3d\*.csproj del .\unity3d\*.csproj

if exist .\hooker\bin rd /Q /S .\hooker\bin
if exist .\hooker\obj rd /Q /S .\hooker\obj
if exist .\hooker\*.csproj del .\hooker\*.csproj
