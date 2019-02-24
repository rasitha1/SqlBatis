using System;
using System.Reflection;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
#if dotnet2

#if DEBUG
#else
    #if dotnet2
    [assembly: AssemblyConfiguration("net-2.0.win32; Release")]
    #else
    [assembly: AssemblyConfiguration("net-1.1.win32; Release")]
    #endif
    [assembly: AssemblyDelaySign(false)]
    [assembly: AssemblyKeyFile("..\\..\\..\\AssemblyKey.snk")]
    #endif
#endif

[assembly: AssemblyTitle("iBATIS.Common")]
[assembly: AssemblyDescription("Common object used by DataAccess and DataMapper component in iBATIS.Net")]
	
[assembly: AssemblyVersion("1.6.1")]

