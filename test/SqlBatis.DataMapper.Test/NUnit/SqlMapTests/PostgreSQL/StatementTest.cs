using System;
using System.Collections;

using NUnit.Framework;

using SqlBatis.DataMapper.Test.NUnit;
using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.PostgreSQL
{
	/// <summary>
	/// Summary description for StatementTest.
	/// </summary>
	[TestFixture] 
	[Category("PostgreSQL")]
	public class StatementTest : BaseTest
	{
		
		#region SetUp & TearDown
	
		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
			InitScript( sqlMap.DataSource, ScriptDirectory + "category-init.sql" );
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific tests for PostgreSQL

		/// <summary>
		/// Test an insert with an autonumber key.
		/// </summary>
		[Test] 
		public void TestInsertAutonumberViaInsertQuery()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategory", category);
			Assert.That(key, Is.EqualTo(1));
		}

		/// <summary>
		/// Test Insert Via Insert Statement.
		/// </summary>
		[Test] 
		public void TestInsertViaInsertStatement()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryViaInsertStatement", category);
			Assert.That(key, Is.EqualTo(1));
		}

		/// <summary>
		/// Test guid column/field.
		/// </summary>
		[Test] 
		public void TestGuidColumn()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryViaInsertStatement", category);

			Category categoryTest = (Category)sqlMap.QueryForObject("GetCategory", key);
			Assert.That(categoryTest.Id, Is.EqualTo(key));
			Assert.That(categoryTest.Name, Is.EqualTo(category.Name));
			Assert.That(categoryTest.Guid, Is.EqualTo(category.Guid));
		}

		/// <summary>
		/// Test Insert Category Via ParameterMap.
		/// </summary>
		[Test] 
		public void TestInsertCategoryViaParameterMap()
		{
			Category category = new Category();
			category.Name = "Cat";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryViaParameterMap", category);
			Assert.That(key, Is.EqualTo(1));
		}

		/// <summary>
		/// Test Update Category with Extended ParameterMap
		/// </summary>
		[Test] 
		public void TestUpdateCategoryWithExtendParameterMap()
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
			categoryRead = (Category) sqlMap.QueryForObject("GetCategory", key);

			Assert.That(categoryRead.Id, Is.EqualTo(category.Id));
			Assert.That(categoryRead.Name, Is.EqualTo(category.Name));
			Assert.That(categoryRead.Guid.ToString(), Is.EqualTo(category.Guid.ToString()));
		}
		#endregion


	}
}
