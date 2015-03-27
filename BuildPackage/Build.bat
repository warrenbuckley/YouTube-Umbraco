ECHO APPVEYOR_REPO_BRANCH: %APPVEYOR_REPO_BRANCH%
ECHO APPVEYOR_BUILD_NUMBER : %APPVEYOR_BUILD_NUMBER%
ECHO APPVEYOR_BUILD_VERSION : %APPVEYOR_BUILD_VERSION%

cd ..\YouTube.PropertyEditors\YouTube\
Call npm install
Call grunt --buildversion %APPVEYOR_BUILD_VERSION% --buildbranch %APPVEYOR_REPO_BRANCH% --packagesuffix %UMBRACO_PACKAGE_PRERELEASE_SUFFIX%

@IF %ERRORLEVEL% NEQ 0 GOTO err

Call ..\..\.nuget\nuget.exe restore ..\..\YouTube.sln
cd ..\..\BuildPackage\
Call "C:\Program Files (x86)\MSBuild\12.0\Bin\MsBuild.exe" Package.build.xml

@IF %ERRORLEVEL% NEQ 0 GOTO err

@EXIT /B 0
:err
@EXIT /B 1