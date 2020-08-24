@echo off

:: U3D安装目录
set ROOT_U3D=D:\Unity.2019.4.6f1
set EXE_MONO=%ROOT_U3D%\Editor\Data\MonoBleedingEdge\bin\mono.exe
set EXE_PDB2MDB=%ROOT_U3D%\Editor\Data\MonoBleedingEdge\lib\mono\4.5\pdb2mdb.exe

:: 解决方案输出目录
set SLN_BIN=%SLN_PATH%\bin
set SLN_OBJ=%SLN_PATH%\obj
:: 解决方案编译模式
set SLN_CONFIGURATION=Debug

set NEED_PDB2MDB=false

:: 解决方案名称
for /f %%q in ("%~dp0.") do set SLN_NAME=%%~nxq
:: 解决方案目录
for /f %%q in ("%~dp0.") do set SLN_PATH=%%~dpnxq
