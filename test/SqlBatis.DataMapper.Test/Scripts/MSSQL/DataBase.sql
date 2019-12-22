-- MSQL DATABASE 'IBatisNet'

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'IBatisNet')
	DROP DATABASE [IBatisNet]
GO

CREATE DATABASE [IBatisNet] 
 COLLATE Latin1_General_CI_AS
GO

