@call .\script_const.bat

dotnet build -c %SLN_CONFIGURATION% -o %SLN_BIN%

set unity3d=unity3d
set unity3d_dll=.\bin\%unity3d%.dll
set unity3d_pdb=.\bin\%unity3d%.pdb
if "true" == "%NEED_PDB2MDB%" (
    if exist %unity3d_dll% (
        if exist %unity3d_pdb% (
            echo do pdb2mdb @ %unity3d_dll%
            %EXE_MONO% %EXE_PDB2MDB% %unity3d_dll%
        )
    )
)


set Assembly_CSharp=Assembly-CSharp
set Assembly_CSharp_dll=.\bin\%Assembly_CSharp%.dll
set Assembly_CSharp_pdb=.\bin\%Assembly_CSharp%.pdb
if "true" == "%NEED_PDB2MDB%" (
    if exist %Assembly_CSharp_dll% (
        if exist %Assembly_CSharp_pdb% (
            echo do pdb2mdb @ %Assembly_CSharp_dll%
            %EXE_MONO% %EXE_PDB2MDB% %Assembly_CSharp_dll%
        )
    )
)

set Assembly_CSharp_firstpass=Assembly-CSharp-firstpass
set Assembly_CSharp_firstpass_dll=.\bin\%Assembly-CSharp-firstpass%.dll
set Assembly_CSharp_firstpass_pdb=.\bin\%Assembly-CSharp-firstpass%.pdb
if "true" == "%NEED_PDB2MDB%" (
    if exist %Assembly_CSharp_firstpass_dll% (
        if exist %Assembly_CSharp_firstpass_pdb% (
            echo do pdb2mdb @ %Assembly_CSharp_firstpass_dll%
            %EXE_MONO% %EXE_PDB2MDB% %Assembly_CSharp_firstpass_dll%
        )
    )
)

set utils=utils
set utils_dll=.\bin\%utils%.dll
set utils_pdb=.\bin\%utils%.pdb
if "true" == "%NEED_PDB2MDB%" (
    if exist %utils_dll% (
        if exist %utils_pdb% (
            echo do pdb2mdb @ %utils_dll%
            %EXE_MONO% %EXE_PDB2MDB% %utils_dll%
        )
    )
)

.\bin\injecter_cmd.exe -t %unity3d_dll% -a %unity3d_dll% --nameof_hooker wolfired.com.unity3d.Hooker --method_name_b HookBegin --method_name_e HookEnd
REM .\bin\injecter_cmd.exe -h -i %Assembly_CSharp_dll% -a %Assembly_CSharp_dll
REM .\bin\injecter_cmd.exe -h -i %Assembly_CSharp_firstpass_dll%
REM .\bin\injecter_cmd.exe -h -i %utils_dll%

REM set Assembly_CSharp_firstpass_dll=.\bin\hooker.dll
REM .\bin\injecter_cmd.exe -t %Assembly_CSharp_dll% -a %Assembly_CSharp_firstpass_dll% --nameof_hooker wolfired.com.hooker.Hooker --method_name_b HookBegin --method_name_e HookEnd --method_whitelist Click

if exist .\bin\unity3d.dll.ddd del .\bin\unity3d.dll

if exist .\bin\unity3d.dll.ddd rename .\bin\unity3d.dll.ddd unity3d.dll

.\bin\unity3p.exe
