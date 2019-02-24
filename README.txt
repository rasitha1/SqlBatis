Building the Assemblies
----------------------------------------------
The IBatisNet.DataMapper, IBatisNet.DataAccess, and IBatisNet.Common assemblies can be built by using Visual Studio or by using NAnt and the iBATIS.build file.



Project Dependencies
----------------------------------------------
If you get an error about a missing NHibernate.dll, it is due to a dependency on NHibernate by the IBatisNet.DataAccess.Extensions project. If you do not need NHibernate support, you can "remove" the DataAccess.Extensions and DataAccess.Test projects from the IBatisNet.sln and/or DataAccess.sln. Otherwise, download the required NHibernate assemblies and place them in the External-Bin\Net\1.1 directory.

The reason why we do not bundle NHibernate with the source is that it is licensed under the LGPL.