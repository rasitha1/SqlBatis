set db=sqllocaldb info SqlBatis
if "%db%"=="" sqllocaldb create "SqlBatis" 17.0 -s
set db=sqllocaldb info SqlBatis | findstr "Running"
if "%db%"=="" sqllocaldb s "SqlBatis"

sqlcmd -S (localdb)\SqlBatis -E -i .\test\SqlBatis.DataMapper.Test\Scripts\MSSQL\DataBase.sql
