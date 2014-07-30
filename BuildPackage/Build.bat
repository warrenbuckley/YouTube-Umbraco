ECHO APPVEYOR_REPO_BRANCH: %APPVEYOR_REPO_BRANCH%
cd ..\YouTube.PropertyEditors\YouTube\
Call npm install
Call node -e "require('grunt').tasks(['default']);"
Call ..\..\.nuget\nuget.exe restore ..\..\YouTube.sln
cd ..\..\BuildPackage\
Call "C:\Program Files (x86)\MSBuild\12.0\Bin\MsBuild.exe" Package.build.xml