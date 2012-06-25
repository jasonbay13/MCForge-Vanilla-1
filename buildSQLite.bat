cd SQLite
MSBuild.exe System.Data.SQLite\System.Data.SQLite.2010.csproj /t:Rebuild /p:Configuration=Release /p:UseInteropDll=false /p:UseSqliteStandard=true
cd ..
pause