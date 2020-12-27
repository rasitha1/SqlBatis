@echo off
echo Executing MSSQL Database Teardown Script...
sqlcmd -S "(localdb)\SqlBatis" -E -i .\test\SqlBatis.DataMapper.Test\Scripts\MSSQL\Teardown.sql

echo Stopping sqllocaldb instance and deleting...
sqllocaldb p "SqlBatis"
sqllocaldb d "SqlBatis"

echo Deleting files... (IBatisNet.mdf and IBatisNet_log.ldf)
if exist %userprofile%\IBatisNet.mdf del %userprofile%\IBatisNet.mdf
if exist %userprofile%\IBatisNet_log.mdf del %userprofile%\IBatisNet_log.mdf

echo Done