using System.Configuration;
using IBatisNet.DataAccess.Configuration;
using NUnit.Framework;

namespace IBatisNet.DataAccess.Test.NUnit.DaoTests.Ado.MySql
{
	/// <summary>
	/// Summary description for AdoDaoTest.
	/// </summary>
	[Category("MySql")]
	[Ignore("Need a MySQL server")]
    public class AdoDaoTest : BaseDaoTest
	{
		/// <summary>
		/// Initialisation
		/// </summary>
		[OneTimeSetUp] 
		public void FixtureSetUp() 
		{
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
			builder.Configure( "dao_MySql_"
                 + Configuration["providerType"] + ".config");
            daoManager = DaoManager.GetInstance();

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
