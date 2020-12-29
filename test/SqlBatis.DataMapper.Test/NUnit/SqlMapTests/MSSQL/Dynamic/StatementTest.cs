using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NUnit.Framework;

using SqlBatis.DataMapper.Test.NUnit;
using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.MSSQL.Dynamic
{
	/// <summary>
	/// Summary description for StatementTest.
	/// </summary>
	[TestFixture] 
	[Category("MSSQL")]
	public class StatementTest : BaseTest
	{
		
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false );
			InitScript( sqlMap.DataSource, ScriptDirectory + "ps_SelectAccount.sql", false );

			InitScript( sqlMap.DataSource, ScriptDirectory + "category-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory + "order-init.sql" );
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "ps_SelectLineItem.sql", false);
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement test for sql server

		/// <summary>
		/// Test Insert Account via store procedure
		/// </summary>
		[Test]
        public void DynamicTestInsertAccountViaStoreProcedure()
        {
            dynamic account = new ExpandoObject();

			account.Id = 99;
			account.FirstName = "Achille";
			account.LastName = "Talon";
			account.EmailAddress = "Achille.Talon@somewhere.com";
            account.BannerOption = true;
            account.CartOption = false;

			sqlMap.Insert("InsertAccountViaStoreProcedureUsingDynamic", account);

            Account testAccount = sqlMap.QueryForObject<Account>("GetAccountViaColumnName", 99);

			Assert.IsNotNull(testAccount);
			Assert.AreEqual(99, testAccount.Id);
		}


        [Test]
        public void DynamicTestGetAccountWithResultsClass()
        {
            var existing = sqlMap.QueryForList<Account>("GetLruCachedAccountsViaResultMap", null).FirstOrDefault();
            Assert.IsNotNull(existing);

            var results = sqlMap.QueryForObject<dynamic>("GetDynamicAccountById", existing.Id);

			Assert.That(results.Id, Is.EqualTo(existing.Id));
            Assert.That(results.FirstName, Is.EqualTo(existing.FirstName));
            Assert.That(results.LastName, Is.EqualTo(existing.LastName));
            Assert.That(results.EmailAddress, Is.EqualTo(existing.EmailAddress));
        }

        [Test]
        public void DynamicTestGetAccountWithResultsMap()
        {
            var existing = sqlMap.QueryForList<Account>("GetLruCachedAccountsViaResultMap", null).FirstOrDefault();
            Assert.IsNotNull(existing);

            var results = sqlMap.QueryForObject<dynamic>("GetDynamicAccountByIdUsingResultsMap", existing.Id);

            Assert.That(results.Id, Is.EqualTo(existing.Id));
            Assert.That(results.FirstName, Is.EqualTo(existing.FirstName));
            Assert.That(results.LastName, Is.EqualTo(existing.LastName));
            Assert.That(results.EmailAddress, Is.EqualTo(existing.EmailAddress));
        }
        #endregion


	}
}

