using System;
using System.Configuration;
using IBatisNet.Common.Transaction;
using IBatisNet.DataAccess.Configuration;
using IBatisNet.DataAccess.Test.Dao.Interfaces;
using IBatisNet.DataAccess.Test.Domain;
using NUnit.Framework;

namespace IBatisNet.DataAccess.Test.NUnit.DaoTests
{
	/// <summary>
	/// Summary description for MutipleDaoTest.
	/// </summary>
	[TestFixture] 
	public class MutipleDaoTest: BaseDaoTest
	{
		/// <summary>
		/// An other daoManager
		/// </summary>
		protected DaoManager daoManager2 = null;

		/// <summary>
		/// Initialisation
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
			DomDaoManagerBuilder builder = new DomDaoManagerBuilder();
			builder.Configure( "dao"+ "_" + ConfigurationSettings.AppSettings["database"] + "_"
				+ ConfigurationSettings.AppSettings["providerType"] + ".config" );

			daoManager = DaoManager.GetInstance("SqlMapDao");
			daoManager2 = DaoManager.GetInstance("NHibernateDao");
			
			InitScript( daoManager.LocalDataSource, ScriptDirectory + "account-init.sql" );
			InitScript( daoManager2.LocalDataSource, ScriptDirectory + "user-init.sql" );
		}


		/// <summary>
		/// Test Create user
		/// </summary>
		[Test] 
		[Category("NHibernate")]
		public void TestCreateUser () 
		{
			IUserDao userDao = (IUserDao)daoManager2[typeof(IUserDao)];

			User newUser = new User();
			newUser.Id = "joe_cool";
			newUser.UserName = "Joseph Cool";
			newUser.Password = "abc123";
			newUser.EmailAddress = "joe@cool.com";
			newUser.LastLogon = DateTime.Now;

			try
			{
				daoManager2.OpenConnection();
				userDao.Create(newUser);
			}
			catch(Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateUser, error cause : "+e.Message);
			}
			finally
			{
				daoManager2.CloseConnection();
			}

			DateTime stamp = DateTime.Now;
			User joeCool = null;
			try
			{
				// open another session to retrieve the just inserted user
				daoManager2.OpenConnection();

				//The User object you get back is live! 
				joeCool = userDao.Load("joe_cool");

				Assert.IsNotNull(joeCool);
				Assert.AreEqual("Joseph Cool", joeCool.UserName);
			
				//Change its properties and it will get persisted to the database on Close. 
				// set Joe Cool's Last Login property
				joeCool.LastLogon = stamp;			
			}
			catch(Exception e)
			{
				// Ignore
				Console.WriteLine("TestCreateUser, error cause : "+e.Message);
			}
			finally
			{
				// flush the changes from the Session to the Database
				daoManager2.CloseConnection();
			}

			daoManager2.OpenConnection();
			//The User object you get back is live! 
			joeCool = userDao.Load("joe_cool");
			daoManager2.CloseConnection();

			Assert.IsNotNull(joeCool);
			Assert.AreEqual("Joseph Cool", joeCool.UserName);
			Assert.AreEqual(stamp.ToString(), joeCool.LastLogon.ToString());
		}


		/// <summary>
		/// Test mutiple dao in TransactionScope
		/// </summary>
		[Test] 
		[Category("MTS")]
		[Category("NHibernate")]
		public void TestUsingTransactionScope () 
		{
			Account account = NewAccount();
			IAccountDao accountDao = daoManager[typeof(IAccountDao)] as IAccountDao;
			IUserDao userDao = daoManager2[typeof(IUserDao)] as IUserDao;
			DateTime stamp = DateTime.Now.AddDays(2);
			User joeCool = null;

			daoManager.OpenConnection();
			accountDao.Create(account);
			daoManager.CloseConnection();

			User newUser = new User();
			newUser.Id = "joe_cool";
			newUser.UserName = "Joseph Cool";
			newUser.Password = "abc123";
			newUser.EmailAddress = "joe@cool.com";
			newUser.LastLogon = DateTime.Now;

			daoManager2.OpenConnection();
			userDao.Create(newUser);
			daoManager2.CloseConnection();

			using (TransactionScope tx = new TransactionScope())
			{
				daoManager.OpenConnection();
				account = accountDao.GetAccountById(1001);
				account.FirstName = "TestTransactionScope";
				accountDao.Update(account);
				daoManager.CloseConnection();

				daoManager2.OpenConnection();
				joeCool = userDao.Load("joe_cool");
				joeCool.LastLogon = stamp;
				daoManager2.CloseConnection();

				//tx.Complete(); // not call complte --> RollBack
			}

			//----------------------------------------
			daoManager.OpenConnection();
			account = accountDao.GetAccountById(1001);
			daoManager.CloseConnection();			
			
			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);
			Assert.IsFalse( "TestTransactionScope"==account.FirstName );

			//----------------
			daoManager2.OpenConnection();
			joeCool = userDao.Load("joe_cool");
			daoManager2.CloseConnection();

			Assert.IsNotNull(joeCool);
			Assert.AreEqual("Joseph Cool", joeCool.UserName);
			Assert.IsFalse( stamp.ToString()==joeCool.LastLogon.ToString() );
		}



		/// <summary>
		/// Test Create user
		/// </summary>
		[Test] 
		[Category("NHibernate")]
		public void TestNestedDao()
		{
			IAccountDao accountDao = daoManager[typeof(IAccountDao)] as IAccountDao;
			IUserDao userDao = daoManager2[typeof(IUserDao)] as IUserDao;
			DateTime stamp = DateTime.Now.AddDays(2);
			User joeCool = null;
			User newUser = new User();
			Account account = NewAccount();

			newUser.Id = "joe_cool";
			newUser.UserName = "Joseph Cool";
			newUser.Password = "abc123";
			newUser.EmailAddress = "joe@cool.com";
			newUser.LastLogon = DateTime.Now;

			daoManager.OpenConnection();
			daoManager2.OpenConnection();
			accountDao.Create(account);
			userDao.Create(newUser);
			daoManager.CloseConnection();
			daoManager2.CloseConnection();

			account = null;
			daoManager.OpenConnection();
			account = accountDao.GetAccountById(1001);
			daoManager.CloseConnection();
			Assert.IsNotNull(account);
			Assert.AreEqual("Calamity.Jane@somewhere.com", account.EmailAddress);

			daoManager2.OpenConnection();
			joeCool = userDao.Load("joe_cool");
			daoManager2.CloseConnection();

			Assert.IsNotNull(joeCool);
			Assert.AreEqual("Joseph Cool", joeCool.UserName);
		}

	}
}
