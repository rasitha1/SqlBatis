using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using SqlBatis.DataMapper.Test.NUnit;
using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.MSSQL.Generics
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
        public void GenericTestInsertAccountViaStoreProcedure() 
		{
			Account account = new Account();

			account.Id = 99;
			account.FirstName = "Achille";
			account.LastName = "Talon";
			account.EmailAddress = "Achille.Talon@somewhere.com";

			sqlMap.Insert("InsertAccountViaStoreProcedure", account);

            Account testAccount = sqlMap.QueryForObject<Account>("GetAccountViaColumnName", 99);

			Assert.That(testAccount, Is.Not.Null);
			Assert.That(testAccount.Id, Is.EqualTo(99));
		}

		/// <summary>
		/// Test statement with properties subtitutions
		/// (Test for IBATISNET-21 : Property substitutions do not occur inside selectKey statement)
		/// </summary>
		[Test] 
		public void GenericTestInsertCategoryWithProperties()
		{
			Category category = new Category();
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryWithProperties", category);

            Category categoryTest = sqlMap.QueryForObject<Category>("GetCategory", key);
			Assert.That(categoryTest.Id, Is.EqualTo(key));
			Assert.That(categoryTest.Name, Is.EqualTo("Film"));
			Assert.That(categoryTest.Guid, Is.EqualTo(category.Guid));
		}

		/// <summary>
		/// Test guid column/field.
		/// </summary>
		[Test]
        public void GenericTestGuidColumn()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategory", category);

			Category categoryTest = sqlMap.QueryForObject<Category>("GetCategory", key);
			Assert.That(categoryTest.Id, Is.EqualTo(key));
			Assert.That(categoryTest.Name, Is.EqualTo(category.Name));
			Assert.That(categoryTest.Guid, Is.EqualTo(category.Guid));
		}

		/// <summary>
		/// Test guid column/field through parameterClass.
		/// </summary>
		[Test]
        public void GenericTestGuidColumnParameterClass()
        {
			Guid newGuid = Guid.NewGuid();
			int key = (int)sqlMap.Insert("InsertCategoryGuidParameterClass", newGuid);

			Category categoryTest = sqlMap.QueryForObject<Category>("GetCategory", key);
			Assert.That(categoryTest.Id, Is.EqualTo(key));
			Assert.That(categoryTest.Name, Is.EqualTo("toto"));
			Assert.That(categoryTest.Guid, Is.EqualTo(newGuid));
		}

		/// <summary>
		/// Test guid column/field through parameterClass without specifiyng dbType
		/// </summary>
		[Test]
        public void GenericTestGuidColumnParameterClassJIRA20() 
		{
			Guid newGuid = Guid.NewGuid();
			int key = (int)sqlMap.Insert("InsertCategoryGuidParameterClassJIRA20", newGuid);

			Category categoryTest = sqlMap.QueryForObject<Category>("GetCategory", key);
			Assert.That(categoryTest.Id, Is.EqualTo(key));
			Assert.That(categoryTest.Name, Is.EqualTo("toto"));
			Assert.That(categoryTest.Guid, Is.EqualTo(newGuid));
		}

		/// <summary>
		/// Test Update Category with Extended ParameterMap
		/// </summary>
		[Test]
        public void GenericTestUpdateCategoryWithExtendParameterMap()
		{
			Category category = new Category();
			category.Name = "Cat";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryViaParameterMap", category);
			category.Id = key;

			category.Name = "Dog";
			category.Guid = Guid.NewGuid();

			sqlMap.Update("UpdateCategoryViaParameterMap", category);

			Category categoryRead = null;
			categoryRead = sqlMap.QueryForObject<Category>("GetCategory", key);

			Assert.That(categoryRead.Id, Is.EqualTo(category.Id));
			Assert.That(categoryRead.Name, Is.EqualTo(category.Name));
			Assert.That(categoryRead.Guid.ToString(), Is.EqualTo(category.Guid.ToString()));
		}

		/// <summary>
		/// Test select via store procedure
		/// </summary>
		[Test]
        public void GenericTestSelect()
		{
			Order order = sqlMap.QueryForObject<Order>("GetOrderWithAccountViaSP", 1);
			AssertOrder1(order);
			AssertAccount1(order.Account);
		}

        /// <summary>
        /// Test generic Collection via store procedure
        /// </summary>
        [Test]
        public void TestGenericCollectionMappingViaSP()
        {
            Order order = sqlMap.QueryForObject<Order>("GetOrderWithGenericViaSP", 1);

            AssertOrder1(order);

            // Check generic collection
            Assert.That(order.LineItemsCollection, Is.Not.Null);
            Assert.That(order.LineItemsCollection.Count, Is.EqualTo(3));
        }
		#endregion


	}
}

