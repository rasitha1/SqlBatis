using System;
using System.Collections;

using NUnit.Framework;

using SqlBatis.DataMapper.Test.NUnit;
using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.Oracle
{
	/// <summary>
	/// Summary description for ProcedureTest.
	/// </summary>
	[TestFixture] 
	[Category("Oracle")]
    [Ignore("Need an Oracle local server")]
	public class ProcedureTest : BaseTest
	{
		
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-init.sql" );
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-procedure.sql", false );		
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-init.sql" );	
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false );
			InitScript( sqlMap.DataSource, ScriptDirectory + "swap-procedure.sql", false );	
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-refcursor-package-spec.sql", false );	
			InitScript( sqlMap.DataSource, ScriptDirectory + "account-refcursor-package-body.sql", false );	
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement store procedure tests for oracle

		/// <summary>
		/// Test an insert with sequence key via a store procedure.
		/// </summary>
		[Test] 
		public void InsertTestSequenceViaProcedure()
		{
			Category category = new Category();
			category.Name = "Mapping object relational";

			sqlMap.Insert("InsertCategoryViaStoreProcedure", category);
			Assert.That(category.Id, Is.EqualTo(1));

			category = new Category();
			category.Name = "Nausicaa";

			sqlMap.Insert("InsertCategoryViaStoreProcedure", category);
			Assert.That(category.Id, Is.EqualTo(2));
		}

		/// <summary>
		/// Test store procedure with output parameters
		/// </summary>
		[Test]
		public void TestProcedureWithOutputParameters() 
		{
			string first = "Joe.Dalton@somewhere.com";
			string second = "Averel.Dalton@somewhere.com";

			Hashtable map = new Hashtable();
			map.Add("email1", first);
			map.Add("email2", second);

			sqlMap.QueryForObject("SwapEmailAddresses", map);

			Assert.That(map["email2"].ToString(), Is.EqualTo(first));
			Assert.That(map["email1"].ToString(), Is.EqualTo(second));
		}

		/// <summary>
		/// Test store procedure with input parameters
		/// passe via Hashtable
		/// </summary>
		[Test]
		public void TestProcedureWithInputParametersViaHashtable() 
		{
			Hashtable map = new Hashtable();
			map.Add("Id", 0);
			map.Add("Name", "Toto");
			map.Add("GuidString", Guid.NewGuid().ToString());

			sqlMap.Insert("InsertCategoryViaStoreProcedure", map);
			Assert.That(map["Id"], Is.EqualTo(1));

		}

		/// <summary>
		/// Test Insert Account via store procedure
		/// </summary>
		[Test] 
		public void TestInsertAccountViaStoreProcedure() {
			Account account = new Account();

			account.Id = 99;
			account.FirstName = "Achille";
			account.LastName = "Talon";
			account.EmailAddress = "Achille.Talon@somewhere.com";

			sqlMap.Insert("InsertAccountViaStoreProcedure", account);

			Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnName", 99) as Account;

			Assert.That(testAccount, Is.Not.Null);
			Assert.That(testAccount.Id, Is.EqualTo(99));
		}

		/// <summary>
		/// Test QueryForList with Ref Cursor.
		/// </summary>
		[Test]
		public void QueryForListWithRefCursor()
		{
			Hashtable param = new Hashtable();
			param.Add("P_ACCOUNTS",null);

			IList list = sqlMap.QueryForList("GetAllAccountsViaStoredProcRefCursor", param);

			Assert.That(list.Count, Is.EqualTo(5));
			AssertAccount1((Account) list[0]);
			Assert.That(((Account) list[1]).Id, Is.EqualTo(2));
			Assert.That(((Account) list[1]).FirstName, Is.EqualTo("Averel"));
			Assert.That(((Account) list[2]).Id, Is.EqualTo(3));
			Assert.That(((Account) list[2]).FirstName, Is.EqualTo("William"));
			Assert.That(((Account) list[3]).Id, Is.EqualTo(4));
			Assert.That(((Account) list[3]).FirstName, Is.EqualTo("Jack"));
			Assert.That(((Account) list[4]).Id, Is.EqualTo(5));
			Assert.That(((Account) list[4]).FirstName, Is.EqualTo("Gilles"));
		}

		/// <summary>
		/// Test QueryForList with Ref Cursor and Input.
		/// </summary>
		[Test]
		public void QueryForListWithRefCursorAndInput()
		{
			Hashtable param = new Hashtable();
			param.Add("P_ACCOUNTS",null);
			param.Add("P_ACCOUNT_ID",1);

			IList list = sqlMap.QueryForList("GetAccountViaStoredProcRefCursor", param);

			Assert.That(list.Count, Is.EqualTo(1));
			AssertAccount1((Account) list[0]);
		}		
		#endregion
	}
}
