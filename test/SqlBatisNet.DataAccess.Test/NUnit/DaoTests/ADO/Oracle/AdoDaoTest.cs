using System.Configuration;
using IBatisNet.DataAccess.Configuration;
using NUnit.Framework;

namespace IBatisNet.DataAccess.Test.NUnit.DaoTests.Ado.Oracle
{
	/// <summary>
	/// Summary description for AdoDaoTest.
	/// </summary>
	[Category("Oracle")]
    [Ignore("Need an Oracle server")]
	public class AdoDaoTest : BaseDaoTest
	{
		/// <summary>
		/// Initialisation
		/// </summary>
		[OneTimeSetUp] 
		public void FixtureSetUp() 
		{
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
			builder.Configure( "dao_Oracle_"
				 + Configuration["providerType"] + ".config" );
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
