using System;
using System.Collections;
using SqlBatis.DataMapper;
using SqlBatis.DataMapper.Exceptions;
using SqlBatis.DataMapper.Utilities;
using SqlBatis.DataMapper.MappedStatements;
using SqlBatis.DataMapper.Test.Domain;
using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ConstructorTest.
	/// </summary>
	[TestFixture]
	public class ConstructorTest : BaseTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp]
		public void Init()
		{
			InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
			InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
			InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
			InitScript(sqlMap.DataSource, ScriptDirectory + "enumeration-init.sql");
			InitScript(sqlMap.DataSource, ScriptDirectory + "other-init.sql");
			InitScript(sqlMap.DataSource, ScriptDirectory + "Nullable-init.sql");
			InitScript(sqlMap.DataSource, ScriptDirectory + "category-init.sql");
			InitScript(sqlMap.DataSource, ScriptDirectory + "documents-init.sql");
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown]
		public void Dispose()
		{ /* ... */
		}

		#endregion

		#region Tests

		/// <summary>
		/// Test account constructor mapping
		/// </summary>
		[Test]
		public void TestPrimitiveArgument()
		{
			Account account = sqlMap.QueryForObject("SelectAccountConstructor", 1) as Account;
			AssertAccount1(account);
		}

		[Test]
		[Category("JIRA")]
		[Category("JIRA-260")]
		public void TestExtendsConstructor()
		{
			Account account = sqlMap.QueryForObject("JIRA260", 1) as Account;
			AssertAccount1(account);
			Assert.That(account.BannerOption, Is.True);
			Assert.That(account.CartOption, Is.False);
		}

		/// <summary>
		/// Test argument nullable constructor mapping
		/// </summary>
		[Test]
		public void TestNullableArgument()
		{
			NullableClass clazz = new NullableClass();
			clazz.TestBool = true;
			clazz.TestByte = 155;
			clazz.TestChar = 'a';
			DateTime? date = new DateTime?(DateTime.Now);
			clazz.TestDateTime = date;
			clazz.TestDecimal = 99.53M;
			clazz.TestDouble = 99.5125;
			Guid? guid = new Guid?(Guid.NewGuid());
			clazz.TestGuid = guid;
			clazz.TestInt16 = 45;
			clazz.TestInt32 = null;
			clazz.TestInt64 = 1234567890123456789;
			clazz.TestSingle = 4578.46445454112f;

			sqlMap.Insert("InsertNullable", clazz);

			clazz = null;
			clazz = sqlMap.QueryForObject<NullableClass>("GetNullableConstructor", 1);

			Assert.That(clazz, Is.Not.Null);
			Assert.That(clazz.Id, Is.EqualTo(1));
			Assert.That(clazz.TestBool.Value, Is.True);
			Assert.That(clazz.TestByte, Is.EqualTo(155));
			Assert.That(clazz.TestChar, Is.EqualTo('a'));
			Assert.That(clazz.TestDateTime.Value.ToString(), Is.EqualTo(date.Value.ToString()));
			Assert.That(clazz.TestDecimal, Is.EqualTo(99.53M));
			Assert.That(clazz.TestDouble, Is.EqualTo(99.5125));
			Assert.That(clazz.TestGuid, Is.EqualTo(guid));
			Assert.That(clazz.TestInt16, Is.EqualTo(45));
			Assert.That(clazz.TestInt32, Is.Null);
			Assert.That(clazz.TestInt64, Is.EqualTo(1234567890123456789));
			Assert.That(clazz.TestSingle, Is.EqualTo(4578.46445454112f));
		}

		/// <summary>
		/// Test constructor injection using a resultMapping where
		/// - the resultmapping object performs *only* constructor injection.
		/// </remarks>
		[Test]
		public void TestJIRA176()
		{
			Category category = new Category();
			category.Name = "toto";
			category.Guid = Guid.Empty;

			int key = (int)sqlMap.Insert("InsertCategory", category);

			ImmutableCategoryPropertyContainer categoryContainerFromDB = (ImmutableCategoryPropertyContainer)sqlMap.QueryForObject("GetImmutableCategoryInContainer", key);
			Assert.That(categoryContainerFromDB, Is.Not.Null);
			Assert.That(categoryContainerFromDB.ImmutableCategory, Is.Not.Null);
			Assert.That(categoryContainerFromDB.ImmutableCategory.Name, Is.EqualTo(category.Name));
			Assert.That(categoryContainerFromDB.ImmutableCategory.Id, Is.EqualTo(key));
			Assert.That(categoryContainerFromDB.ImmutableCategory.Guid, Is.EqualTo(category.Guid));
		}

		/// <summary>
		/// Test constructor with resultMapping attribute on argument
		/// </remarks>
		[Test]
		public void TestArgumentResultMapping()
		{
			Order order = sqlMap.QueryForObject("GetOrderConstructor1", 10) as Order;

			Assert.That(order.Account, Is.Not.Null);
			AssertAccount1(order.Account);

			order = sqlMap.QueryForObject("GetOrderConstructor1", 11) as Order;

			Assert.That(order.Account, Is.Null);
		}

		/// <summary>
		/// Test constructor with an argument using a resultMapping where
		/// - the resulMap argument use another constructor
		/// - all second constructor arguments are null.
		/// </remarks>
		[Test]
		public void TestJIRA173()
		{
			Order order = sqlMap.QueryForObject("GetOrderConstructor8", 11) as Order;

			Assert.That(order.Id == 11, Is.True);
			Assert.That(order.Account, Is.Null);
		}

		/// <summary>
		/// Test resultMap with a result property using another resultMap and where
		/// - the result property resultMap use a constructor
		/// - all the constructor arguments are null.
		/// </remarks>
		[Test]
		public void TestJIRA174()
		{
			Order order = sqlMap.QueryForObject("GetOrderConstructor9", 11) as Order;

			Assert.That(order.Id == 11, Is.True);
			Assert.That(order.Account, Is.Null);
		}

		/// <summary>
		/// Test a constructor argument with select tag.
		/// </remarks>
		[Test]
		public void TestJIRA186()
		{
			Order order = sqlMap.QueryForObject("GetOrderConstructor10", 5) as Order;

			Assert.That(order.Id == 5, Is.True);
			Assert.That(order.Account, Is.Not.Null);
			Assert.That(order.Account.Document, Is.Not.Null);
		}

		/// <summary>
		/// Test constructor with select attribute on argument
		/// </remarks>
		[Test]
		public void TestArgumentSelectObject()
		{
			Order order = (Order)sqlMap.QueryForObject("GetOrderConstructor2", 1);
			AssertOrder1(order);
			AssertAccount1(order.Account);
		}

		/// <summary>
		/// Test constructor with select attribute on IList argument
		/// </remarks>
		[Test]
		public void TestArgumentSelectIList()
		{
			Order order = (Order)sqlMap.QueryForObject("GetOrderConstructor3", 1);
			AssertOrder1(order);
			AssertAccount1(order.Account);

			Assert.That(order.LineItemsIList, Is.Not.Null);
			Assert.That(order.LineItemsIList.Count, Is.EqualTo(3));
		}

		/// <summary>
		/// Test constructor with select attribute on array argument
		/// </remarks>
		[Test]
		public void TestArgumentSelectArray()
		{
			Order order = (Order)sqlMap.QueryForObject("GetOrderConstructor4", 1);
			AssertOrder1(order);
			AssertAccount1(order.Account);

			Assert.That(order.LineItemsArray, Is.Not.Null);
			Assert.That(order.LineItemsArray.Length, Is.EqualTo(3));
		}

		/// <summary>
		/// Test constructor with select attribute on stronly typed collection argument
		/// </remarks>
		[Test]
		public void TestArgumentSelectCollection()
		{
			Order order = (Order)sqlMap.QueryForObject("GetOrderConstructor5", 1);
			AssertOrder1(order);
			AssertAccount1(order.Account);

			Assert.That(order.LineItemsCollection, Is.Not.Null);
			Assert.That(order.LineItemsCollection.Count, Is.EqualTo(3));
		}

		/// <summary>
		/// Test constructor with select attribute on generic list argument
		/// </remarks>
		[Test]
		public void TestArgumentSelectGenericIList()
		{
			Order order = (Order)sqlMap.QueryForObject("GetOrderConstructor6", 1);
			AssertOrder1(order);

			Assert.That(order.LineItemsGenericList, Is.Not.Null);
			Assert.That(order.LineItemsGenericList.Count, Is.EqualTo(3));
		}

		/// <summary>
		/// Test constructor with select attribute on stronly typed generic collection argument
		/// </remarks>
		[Test]
		public void TestArgumentSelectGenericCollection()
		{
			Order order = (Order)sqlMap.QueryForObject("GetOrderConstructor7", 1);
			AssertOrder1(order);

			Assert.That(order.LineItemsCollection2, Is.Not.Null);
			Assert.That(order.LineItemsCollection2.Count, Is.EqualTo(3));
		}
		#endregion
	}
}