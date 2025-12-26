using System;

using NUnit.Framework;

using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for ResultClassTest.
	/// </summary>
	[TestFixture] 
	public class ResultClassTest : BaseTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void Init() 
		{
		}

		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ /* ... */ } 

		#endregion

		#region Specific statement test

		/// <summary>
		///  Test a boolean resultClass
		/// </summary>
		[Test]
		public void TestBoolean() 
		{
			bool bit = (bool) sqlMap.QueryForObject("GetBoolean", 1);

			Assert.That(bit, Is.EqualTo(true));
		}

		/// <summary>
		///  Test a boolean implicit resultClass
		/// </summary>
		[Test]
		public void TestBooleanWithoutResultClass() 
		{
			bool bit = Convert.ToBoolean(sqlMap.QueryForObject("GetBooleanWithoutResultClass", 1));

			Assert.That(bit, Is.EqualTo(true));
		}

		/// <summary>
		///  Test a byte resultClass
		/// </summary>
		[Test] 
		public void TestByte() 
		{
			byte letter = (byte) sqlMap.QueryForObject("GetByte", 1);

			Assert.That(letter, Is.EqualTo(155));
		}

		/// <summary>
		///  Test a byte implicit resultClass
		/// </summary>
		[Test] 
		public void TestByteWithoutResultClass() 
		{
			byte letter = Convert.ToByte(sqlMap.QueryForObject("GetByteWithoutResultClass", 1));

			Assert.That(letter, Is.EqualTo(155));
		}

		/// <summary>
		///  Test a char resultClass
		/// </summary>
		[Test] 
		public void TestChar() 
		{
			char letter = (char) sqlMap.QueryForObject("GetChar", 1);

			Assert.That(letter, Is.EqualTo('a'));
		}

		/// <summary>
		///  Test a char implicit resultClass
		/// </summary>
		[Test] 
		public void TestCharWithoutResultClass() 
		{
			char letter = Convert.ToChar(sqlMap.QueryForObject("GetCharWithoutResultClass", 1));

			Assert.That(letter, Is.EqualTo('a'));
		}

		/// <summary>
		///  Test a DateTime resultClass
		/// </summary>
		[Test] 
		public void TestDateTime() 
		{
			DateTime orderDate = (DateTime) sqlMap.QueryForObject("GetDate", 1);

			System.DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

			Assert.That(orderDate.ToString(), Is.EqualTo(date.ToString()));
		}

		/// <summary>
		///  Test a DateTime implicit resultClass
		/// </summary>
		[Test] 
		public void TestDateTimeWithoutResultClass() 
		{
			DateTime orderDate = Convert.ToDateTime(sqlMap.QueryForObject("GetDateWithoutResultClass", 1));

			System.DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

			Assert.That(orderDate.ToString(), Is.EqualTo(date.ToString()));
		}

		/// <summary>
		///  Test a decimal resultClass
		/// </summary>
		[Test] 
		public void TestDecimal() 
		{
			decimal price = (decimal) sqlMap.QueryForObject("GetDecimal", 1);

			Assert.That(price, Is.EqualTo((decimal)1.56));
		}

		/// <summary>
		///  Test a decimal implicit resultClass
		/// </summary>
		[Test] 
		public void TestDecimalWithoutResultClass() 
		{
			decimal price = Convert.ToDecimal(sqlMap.QueryForObject("GetDecimalWithoutResultClass", 1));

			Assert.That(price, Is.EqualTo((decimal)1.56));
		}

		/// <summary>
		///  Test a double resultClass
		/// </summary>
		[Test] 
		public void TestDouble() 
		{
			double price = (double) sqlMap.QueryForObject("GetDouble", 1);

			Assert.That(price, Is.EqualTo(99.5f));
		}

		/// <summary>
		///  Test a double implicit resultClass
		/// </summary>
		[Test] 
		public void TestDoubleWithoutResultClass() 
		{
			double price = Convert.ToDouble(sqlMap.QueryForObject("GetDoubleWithoutResultClass", 1));

			Assert.That(price, Is.EqualTo(99.5f));
		}

		/// <summary>
		///  IBATISNET-25 Error applying ResultMap when using 'Guid' in resultClass
		/// </summary>
		[Test] 
		public void TestGuid() 
		{
			Guid newGuid = new Guid("CD5ABF17-4BBC-4C86-92F1-257735414CF4");

			Guid guid = (Guid) sqlMap.QueryForObject("GetGuid", 1);

			Assert.That(guid, Is.EqualTo(newGuid));
		}

		/// <summary>
		/// Test a Guid implicit resultClass
		/// </summary>
		[Test] 
		public void TestGuidWithoutResultClass()
		{
			Guid newGuid = new Guid("CD5ABF17-4BBC-4C86-92F1-257735414CF4");

			string guidString = Convert.ToString(sqlMap.QueryForObject("GetGuidWithoutResultClass", 1));

			Guid guid = new Guid(guidString);

			Assert.That(guid, Is.EqualTo(newGuid));
		}

		/// <summary>
		///  Test a int16 resultClass
		/// </summary>
		[Test] 
		public void TestInt16() 
		{
			short integer = (short) sqlMap.QueryForObject("GetInt16", 1);

			Assert.That(integer, Is.EqualTo(32111));
		}

		/// <summary>
		///  Test a int16 implicit resultClass
		/// </summary>
		[Test] 
		public void TestInt16WithoutResultClass() 
		{
			short integer = Convert.ToInt16(sqlMap.QueryForObject("GetInt16WithoutResultClass", 1));

			Assert.That(integer, Is.EqualTo(32111));
		}

		/// <summary>
		///  Test a int 32 resultClass
		/// </summary>
		[Test] 

		public void TestInt32() 
		{
			int integer = (int) sqlMap.QueryForObject("GetInt32", 1);

			Assert.That(integer, Is.EqualTo(999999));
		}

		/// <summary>
		///  Test a int 32 implicit resultClass
		/// </summary>
		[Test] 

		public void TestInt32WithoutResultClass() 
		{
			int integer = Convert.ToInt32(sqlMap.QueryForObject("GetInt32WithoutResultClass", 1));

			Assert.That(integer, Is.EqualTo(999999));
		}

		/// <summary>
		///  Test a int64 resultClass
		/// </summary>
		[Test] 
		public void TestInt64() 
		{
			long bigInt = (long) sqlMap.QueryForObject("GetInt64", 1);

			Assert.That(bigInt, Is.EqualTo(9223372036854775800));
		}

		/// <summary>
		///  Test a int64 implicit resultClass
		/// </summary>
		[Test] 
		public void TestInt64WithoutResultClass() 
		{
			long bigInt = Convert.ToInt64(sqlMap.QueryForObject("GetInt64WithoutResultClass", 1));

			Assert.That(bigInt, Is.EqualTo(9223372036854775800));
		}

		/// <summary>
		///  Test a single/float resultClass
		/// </summary>
		[Test] 
		public void TestSingle() 
		{
			float price = (float)sqlMap.QueryForObject("GetSingle", 1);

			Assert.That(price, Is.EqualTo(92233.5));
		}

		/// <summary>
		///  Test a single/float implicit resultClass
		/// </summary>
		[Test] 
		public void TestSingleWithoutResultClass() 
		{
			double price = Convert.ToDouble(sqlMap.QueryForObject("GetSingleWithoutResultClass", 1));

			Assert.That(price, Is.EqualTo(92233.5));
		}

		/// <summary>
		///  Test a string resultClass
		/// </summary>
		[Test] 
		public void TestString() 
		{
			string cardType = sqlMap.QueryForObject("GetString", 1) as string;

			Assert.That(cardType, Is.EqualTo("VISA"));
		}

		/// <summary>
		///  Test a string implicit resultClass
		/// </summary>
		[Test] 
		public void TestStringWithoutResultClass() 
		{
			string cardType = Convert.ToString(sqlMap.QueryForObject("GetStringWithoutResultClass", 1));

			Assert.That(cardType, Is.EqualTo("VISA"));
		}

		/// <summary>
		///  Test a TimeSpan resultClass
		/// </summary>
		[Test] 
		[Ignore("To do")]
		public void TestTimeSpan() 
		{
			Guid newGuid = Guid.NewGuid();;
			Category category = new Category();
			category.Name = "toto";
			category.Guid = newGuid;

			int key = (int)sqlMap.Insert("InsertCategory", category);

			Guid guid = (Guid)sqlMap.QueryForObject("GetGuid", key);

			Assert.That(guid, Is.EqualTo(newGuid));
		}

		/// <summary>
		///  Test a TimeSpan implicit resultClass
		/// </summary>
		[Test] 
		[Ignore("To do")]
		public void TestTimeSpanWithoutResultClass() 
		{

		}
		#endregion
	}
}
