using System.Configuration;
using IBatisNet.DataAccess.Configuration;
using NUnit.Framework;

namespace IBatisNet.DataAccess.Test.NUnit.DaoTests.Ado.MSSQL
{
	/// <summary>
	/// Summary description for AdoDaoTest.
	/// </summary>
	[Category("MSSQL")]
	public class AdoDaoTest : BaseDaoTest
	{
		/// <summary>
		/// Initialisation
		/// </summary>
		[TestFixtureSetUp] 
		public void FixtureSetUp() 
		{
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
#if dotnet2		    
			builder.Configure( "dao_MSSQL_"
                 + ConfigurationManager.AppSettings["providerType"] + ".config");
			daoManager = DaoManager.GetInstance();
#else
			builder.Configure( "dao_MSSQL_"
				 + ConfigurationSettings.AppSettings["providerType"] + ".config" );
			daoManager = DaoManager.GetInstance();
#endif

        }

		/// <summary>
		/// Initialisation
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{			
			InitScript( daoManager.LocalDataSource, ScriptDirectory + "account-init.sql" );
		}
	}
}
