using System.Collections;
using NUnit.Framework;

//<-- To access the definition of the deleagte RowDelegate
//<-- To access the definition of the PageinatedList
using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests
{
	/// <summary>
	/// Summary description for InheritanceTest.
	/// </summary>
	[TestFixture]
	public class InheritanceTest : BaseTest
	{

		#region SetUp & TearDown


		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp]
		public void Init()
		{
			InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
			InitScript(sqlMap.DataSource, ScriptDirectory + "documents-init.sql");
		}


		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown]
		public void Dispose()
		{ /* ... */ }

		#endregion

		#region Tests

		/// <summary>
		/// Test All document with no formula
		/// </summary>
		[Test]
		public void GetAllDocument()
		{
			IList list = sqlMap.QueryForList("GetAllDocument", null);

			Assert.That(list.Count, Is.EqualTo(6));
			Book book = (Book)list[0];
			AssertBook(book, 1, "The World of Null-A", 55);

			book = (Book)list[1];
			AssertBook(book, 3, "Lord of the Rings", 3587);

			Document document = (Document)list[2];
			AssertDocument(document, 5, "Le Monde");

			document = (Document)list[3];
			AssertDocument(document, 6, "Foundation");

			Newspaper news = (Newspaper)list[4];
			AssertNewspaper(news, 2, "Le Progres de Lyon", "Lyon");

			document = (Document)list[5];
			AssertDocument(document, 4, "Le Canard enchaine");
		}

		/// <summary>
		/// Test All document in a typed collection
		/// </summary>
		[Test]
		public void GetTypedCollection()
		{
			DocumentCollection list = sqlMap.QueryForList("GetTypedCollection", null) as DocumentCollection;

			Assert.That(list.Count, Is.EqualTo(6));

			Book book = (Book)list[0];
			AssertBook(book, 1, "The World of Null-A", 55);

			book = (Book)list[1];
			AssertBook(book, 3, "Lord of the Rings", 3587);

			Document document = list[2];
			AssertDocument(document, 5, "Le Monde");

			document = list[3];
			AssertDocument(document, 6, "Foundation");

			Newspaper news = (Newspaper)list[4];
			AssertNewspaper(news, 2, "Le Progres de Lyon", "Lyon");

			document = list[5];
			AssertDocument(document, 4, "Le Canard enchaine");
		}

		/// <summary>
		/// Test All document with Custom Type Handler
		/// </summary>
		[Test]
		public void GetAllDocumentWithCustomTypeHandler()
		{
			IList list = sqlMap.QueryForList("GetAllDocumentWithCustomTypeHandler", null);

			Assert.That(list.Count, Is.EqualTo(6));
			Book book = (Book)list[0];
			AssertBook(book, 1, "The World of Null-A", 55);

			book = (Book)list[1];
			AssertBook(book, 3, "Lord of the Rings", 3587);

			Newspaper news = (Newspaper)list[2];
			AssertNewspaper(news, 5, "Le Monde", "Paris");

			book = (Book)list[3];
			AssertBook(book, 6, "Foundation", 557);

			news = (Newspaper)list[4];
			AssertNewspaper(news, 2, "Le Progres de Lyon", "Lyon");

			news = (Newspaper)list[5];
			AssertNewspaper(news, 4, "Le Canard enchaine", "Paris");
		}

		/// <summary>
		/// Test Inheritance On Result Property
		/// </summary>
		[Test]
		public void TestJIRA175()
		{
			Account account = sqlMap.QueryForObject("JIRA175", 3) as Account;
			Assert.That(account.Id, Is.EqualTo(3), "account.Id");
			Assert.That(account.FirstName, Is.EqualTo("William"), "account.FirstName");

			Book book = account.Document as Book;
			Assert.That(book, Is.Not.Null);
			AssertBook(book, 3, "Lord of the Rings", 3587);
		}

		#endregion

		void AssertDocument(Document document, int id, string title)
		{
			Assert.That(document.Id, Is.EqualTo(id));
			Assert.That(document.Title, Is.EqualTo(title));
		}

		void AssertBook(Book book, int id, string title, int pageNumber)
		{
			Assert.That(book.Id, Is.EqualTo(id));
			Assert.That(book.Title, Is.EqualTo(title));
			Assert.That(book.PageNumber, Is.EqualTo(pageNumber));
		}

		void AssertNewspaper(Newspaper news, int id, string title, string city)
		{
			Assert.That(news.Id, Is.EqualTo(id));
			Assert.That(news.Title, Is.EqualTo(title));
			Assert.That(news.City, Is.EqualTo(city));
		}
	}
}

