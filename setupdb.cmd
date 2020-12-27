@echo off
echo Setting up SqlLocalDB database instance...
set db=sqllocaldb info SqlBatis
if "%db%"=="" sqllocaldb create "SqlBatis" 15.0 -s
set db=sqllocaldb info SqlBatis | findstr "Running"
if "%db%"=="" sqllocaldb s "SqlBatis"

echo Executing MSSQL Database Init Script...
sqlcmd -S (localdb)\SqlBatis -E -i .\test\SqlBatis.DataMapper.Test\Scripts\MSSQL\DataBase.sql

echo Done