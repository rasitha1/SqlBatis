// DataSource definition
	// ScriptRunner definition
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using IBatisNet.Common;
using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess.Configuration;
using IBatisNet.DataAccess.Test.Dao.Interfaces;
using IBatisNet.DataAccess.Test.Domain;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace IBatisNet.DataAccess.Test.NUnit.DaoTests
{
	/// <summary>
	/// Summary description for BaseDaoTest.
	/// </summary>
	[TestFixture]
	public abstract class BaseDaoTest
	{
		/// <summary>
		/// A daoManager
		/// </summary>
		protected static IDaoManager daoManager = null;

		protected static string ScriptDirectory = null;

		private ManualResetEvent _startEvent = new ManualResetEvent(false);
		private ManualResetEvent _stopEvent = new ManualResetEvent(false);

	    static IReadOnlyDictionary<string, string> DefaultConfigurationStrings { get; } =
	        new Dictionary<string, string>()
	        {
	            ["database"] = "MSSQL",
	            ["providerType"] = "SqlClient"
	        };

	    protected static IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        static BaseDaoTest()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(DefaultConfigurationStrings);
            Configuration = builder.Build();


            ScriptDirectory = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine(TestContext.CurrentContext.TestDirectory, ".."), ".."), ".."), "Scripts"),
                                  Configuration["database"]) + Path.DirectorySeparatorChar;

        }


        /// <summary>
        /// Run a sql batch for the datasource.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="script">The sql batch</param>
        protected static void InitScript(IDataSource datasource, string script)
		{
			ScriptRunner runner = new ScriptRunner();

			runner.RunScript(datasource, script);
		}

		[Test]
		public void MultipleContext() 
		{
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
            builder.Configure( "dao_Multiple_Context.config" );
			IDaoManager daoManager1 = DaoManager.GetInstance("Contex1");
			IDaoManager daoManager2 = DaoManager.GetInstance("Contex2");

			Assert.IsNotNull(daoManager1);
			Assert.IsNotNull(daoManager2);
			Assert.IsTrue(daoManager2.LocalDataSource.ConnectionString != daoManager1.LocalDataSource.ConnectionString);
			Assert.IsTrue(daoManager2.LocalDataSource.DbProvider.Name != daoManager1.LocalDataSource.DbProvider.Name);

            daoManager1.OpenConnection();
            daoManager2.OpenConnection();   

            daoManager1.CloseConnection();
            daoManager2.CloseConnection();
		}

		#region Dao statement tests



		/// <summary>
		/// Verify that DaoManager.GetDao("Account")
		/// return an object that implemetent the interface IAccountDao.
		/// </summary>
		[Test]
		public void TestGetDao()
		{
			Type type = typeof (IAccountDao);

			IAccountDao accountDao = (IAccountDao) daoManager[typeof (IAccountDao)];

			Assert.IsNotNull(accountDao);
			Assert.IsTrue(type.IsInstanceOfType(accountDao));
		}

		/// <summary>
		/// Test Open connection with a connection string
		/// </summary>
		[Test]
		public void TestOpenConnection()
		{
			IAccountDao accountDao = (IAccountDao) daoManager[typeof (IAccountDao)];


			Account account = NewAccount();

			try
			{
				daoManager.OpenConnection(daoManager.LocalDataSource.ConnectionString);
				accountDao.Create(account);

				account = accountDao.GetAccountById(1001);
			}
			catch (Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateAccount, error cause : " + e.Message);
			}
			finally
			{
				daoManager.CloseConnection();
			}

			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);
		}

		/// <summary>
		/// Test CreateAccount
		/// </summary>
		[Test]
		public void TestCreateAccount()
		{
			IAccountDao accountDao = (IAccountDao) daoManager[typeof (IAccountDao)];

			Account account = NewAccount();

			try
			{
				daoManager.OpenConnection();
				accountDao.Create(account);

				account = accountDao.GetAccountById(1001);
			}
			catch (Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateAccount, error cause : " + e.Message);
			}
			finally
			{
				daoManager.CloseConnection();
			}

			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);
		}

		/// <summary>
		/// Test CreateAccount
		/// </summary>
		[Test]
		public void TestCreateAccountExplicitOpenSession()
		{
			IAccountDao accountDao = daoManager[typeof (IAccountDao)] as IAccountDao;

			Account account = NewAccount();

			try
			{
				accountDao.Create(account);

				account = accountDao.GetAccountById(1001);
			}
			catch (Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateAccount, error cause : " + e.Message);
			}
			finally
			{
			}

			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);
		}

		/// <summary>
		/// Test Transaction Rollback
		/// </summary>
		[Test]
		public void TestTransactionRollback()
		{
			IAccountDao accountDao = (IAccountDao) daoManager[typeof (IAccountDao)];

			Account account = NewAccount();
			daoManager.OpenConnection();
			Account account2 = accountDao.GetAccountById(1);
			daoManager.CloseConnection();

			account2.EmailAddress = "someotherAddress@somewhere.com";

			try
			{
				daoManager.BeginTransaction();

				accountDao.Create(account);
				accountDao.Update(account2);
				throw new Exception("BOOM!");

				//daoManager.CommitTransaction();
			}
			catch
			{
				daoManager.RollBackTransaction();
			}
			finally
			{
			}

			daoManager.OpenConnection();
			account = accountDao.GetAccountById(account.Id);
			account2 = accountDao.GetAccountById(1);
			daoManager.CloseConnection();

			Assert.IsNull(account);
			Assert.AreEqual("Joe.Dalton@somewhere.com", account2.EmailAddress);
		}


		/// <summary>
		/// Test Transaction Commit
		/// </summary>
		[Test]
		public void TestTransactionCommit()
		{
			IAccountDao accountDao = (IAccountDao) daoManager[typeof (IAccountDao)];

			Account account = NewAccount();

			daoManager.OpenConnection();
			Account account2 = accountDao.GetAccountById(1);
			daoManager.CloseConnection();

			account2.EmailAddress = "someotherAddress@somewhere.com";

			try
			{
				daoManager.BeginTransaction();
				accountDao.Create(account);
				accountDao.Update(account2);
				daoManager.CommitTransaction();
			}
			finally
			{
				// Nothing
			}

			daoManager.OpenConnection();
			account = accountDao.GetAccountById(account.Id);
			account2 = accountDao.GetAccountById(1);
			daoManager.CloseConnection();

			Assert.IsNotNull(account);
			Assert.AreEqual("someotherAddress@somewhere.com", account2.EmailAddress);
		}

		/// <summary>
		/// Test Delete
		/// </summary>
		[Test]
		public void TestDeleteAccount()
		{
			IAccountDao accountDao = (IAccountDao) daoManager[typeof (IAccountDao)];

			Account account = NewAccount();

			daoManager.OpenConnection();

			accountDao.Create(account);
			account = accountDao.GetAccountById(1001);

			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);

			accountDao.Delete(account);

			account = accountDao.GetAccountById(1001);
			Assert.IsNull(account);

			daoManager.CloseConnection();
		}

		/// <summary>
		/// Test Using syntax on daoManager.OpenConnection
		/// </summary>
		[Test]
		public void TestUsingConnection()
		{
			IAccountDao accountDao = (IAccountDao) daoManager[typeof (IAccountDao)];

			using (IDalSession session = daoManager.OpenConnection())
			{
				Account account = NewAccount();
				accountDao.Create(account);
			} // compiler will call Dispose on DaoSession
		}

		/// <summary>
		/// Test Test Using syntax on daoManager.BeginTransaction
		/// </summary>
		[Test]
		public void TestUsingTransaction()
		{
			IAccountDao accountDao = (IAccountDao) daoManager[typeof (IAccountDao)];

			using (IDalSession session = daoManager.BeginTransaction())
			{
				Account account = NewAccount();
				Account account2 = accountDao.GetAccountById(1);
				account2.EmailAddress = "someotherAddress@somewhere.com";

				accountDao.Create(account);
				accountDao.Update(account2);

				session.Complete(); // Commit
			} // compiler will call Dispose on IDalSession
		}

		#endregion

		#region Thread test

		[Test]
		public void TestCommonUsageMultiThread()
		{
			const int threadCount = 10;

			Thread[] threads = new Thread[threadCount];

			for (int i = 0; i < threadCount; i++)
			{
				threads[i] = new Thread(new ThreadStart(ExecuteMethodUntilSignal));
				threads[i].Start();
			}

			_startEvent.Set();

			Thread.CurrentThread.Join(1*2000);

			_stopEvent.Set();
		}

		public void ExecuteMethodUntilSignal()
		{
			_startEvent.WaitOne(int.MaxValue, false);

			IAccountDao accountDao = daoManager[typeof (IAccountDao)] as IAccountDao;

			while (!_stopEvent.WaitOne(1, false))
			{
				Assert.IsFalse(daoManager.IsDaoSessionStarted());

				Account account = account = accountDao.GetAccountById(1);

				Assert.IsFalse(daoManager.IsDaoSessionStarted());

				Assert.AreEqual(1, account.Id, "account.Id");
				Assert.AreEqual("Joe", account.FirstName, "account.FirstName");
				Assert.AreEqual("Dalton", account.LastName, "account.LastName");
			}
		}

		#endregion

		/// <summary>
		/// Create a new account with id = 1001
		/// </summary>
		/// <returns>An account</returns>
		protected Account NewAccount()
		{
			Account account = new Account();
			account.Id = 1001;
			account.FirstName = "Calamity";
			account.LastName = "Jane";
			account.EmailAddress = "Calamity.Jane@somewhere.com";
			return account;
		}

	}
}
