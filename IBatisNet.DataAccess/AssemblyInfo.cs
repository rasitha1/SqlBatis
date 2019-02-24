using System;
using System.Reflection;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//

#if DEBUG
#else
[assembly: AssemblyConfiguration("net-2.0.win32; Release")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFileAttribute("..\\..\\..\\AssemblyKey.snk")]
#endif

[assembly: AssemblyTitle("iBATIS.DataAccess")]
[assembly: AssemblyDescription("Data Access Object (DAO) design pattern implementation.")]


[assembly: AssemblyVersion("1.9.1")]
