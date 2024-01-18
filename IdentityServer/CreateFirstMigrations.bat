@ECHO OFF

dotnet ef migrations add InitialIdentityServerApplicationDbMigration -c ApplicationDbContext -o Data/Migrations/IdentityServer/ApplicationDb
IF %ERRORLEVEL% NEQ 0 GOTO fail
dotnet ef database update --context ApplicationDbContext
IF %ERRORLEVEL% NEQ 0 GOTO fail


dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
IF %ERRORLEVEL% NEQ 0 GOTO fail
dotnet ef database update --context ConfigurationDbContext
IF %ERRORLEVEL% NEQ 0 GOTO fail



dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
IF %ERRORLEVEL% NEQ 0 GOTO fail
dotnet ef database update --context PersistedGrantDbContext
IF %ERRORLEVEL% NEQ 0 GOTO fail

GOTO :getout

:fail
ECHO Action failed!

:getout
