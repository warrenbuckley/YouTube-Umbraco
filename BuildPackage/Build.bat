cd YouTube.PropertyEditors\YouTube\
Call npm install
Call node -e "require('grunt').tasks(['default']);"
Call ..\..\.nuget\nuget.exe restore ..\..\YouTube.sln
cd ..\..\BuildPackage\
Call C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\msbuild.exe Package.build.xml