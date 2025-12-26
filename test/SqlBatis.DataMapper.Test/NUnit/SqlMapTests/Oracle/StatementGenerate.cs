using System;
using System.Collections;

using NUnit.Framework;

using SqlBatis.DataMapper.Test.NUnit;
using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.Oracle
{
	/// <summary>
	/// Summary description for StatementGenerate.
	/// </summary>
	[TestFixture] 
	[Category("Oracle")]
	public class StatementGenerate : BaseTest
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

		#region Specific statement test for Oracle

		/// <summary>
		/// Test a select by key via generate statement.
		/// </summary>
		[Test] 
		public void TestSelectByPK()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			category.Id = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			Assert.That(category.Id, Is.EqualTo(1));

			// Workaround!
			// Null out unneeded properties, otherwise those property values will be added
			// as command parameters for the auto-generated SELECT query even if
			// only 1 parameter for Id is needed.
            var query = new Category(){ Id = category.Id};

			Category categoryRead = null;
			categoryRead = (Category) sqlMap.QueryForObject("SelectByPKCategoryGenerate", query);

			Assert.That(categoryRead.Id, Is.EqualTo(category.Id));
			Assert.That(categoryRead.Name, Is.EqualTo(category.Name));
			Assert.That(categoryRead.Guid.ToString(), Is.EqualTo(category.Guid.ToString()));
		}

		/// <summary>
		/// Test an select all via generate statement.
		/// </summary>
		[Test] 
		public void TestSelectAll()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			Assert.That(key, Is.EqualTo(1));

			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			Assert.That(key, Is.EqualTo(2));

			IList categorieList = sqlMap.QueryForList("SelectAllCategoryGenerate",null) as IList;
			Assert.That(categorieList.Count, Is.EqualTo(2));

		}

		/// <summary>
		/// Test an insert via generate statement.
		/// </summary>
		[Test] 
		public void TestInsert()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.Empty;

			int key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			Assert.That(key, Is.EqualTo(1));
		}

		/// <summary>
		/// Test Update Category with Extended ParameterMap
		/// </summary>
		[Test] 
		public void TestUpdate()
		{
			Category category = new Category();
			category.Name = "Cat";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			category.Id = key;

			category.Name = "Dog";
			category.Guid = Guid.NewGuid();

			sqlMap.Update("UpdateCategoryGenerate", category);

			Category categoryRead = null;
			categoryRead = (Category) sqlMap.QueryForObject("GetCategory", key);

			Assert.That(categoryRead.Id, Is.EqualTo(category.Id));
			Assert.That(categoryRead.Name, Is.EqualTo(category.Name));
			Assert.That(categoryRead.Guid.ToString(), Is.EqualTo(category.Guid.ToString()));
		}
		
		/// <summary>
		/// Test an insert via generate statement.
		/// </summary>
		[Test] 
		public void TestDelete()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.NewGuid();

			int key = (int)sqlMap.Insert("InsertCategoryGenerate", category);
			category.Id = key;
			Assert.That(category.Id, Is.EqualTo(1));
			
			sqlMap.Delete("DeleteCategoryGenerate", category);

			Category categoryRead = null;
			categoryRead = sqlMap.QueryForObject("GetCategory", key) as Category;

			Assert.That(categoryRead, Is.Null);
		}
		#endregion
	}
}
