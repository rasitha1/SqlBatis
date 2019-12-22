using System;
using System.Collections;

using IBatisNet.Common.Pagination;

using IBatisNet.Common.Test.NUnit;

using NUnit.Framework;

namespace IBatisNet.Common.Test.NUnit.CommonTests.Pagination
{
	/// <summary>
	/// Summary description for PaginatedArrayListTest.
	/// </summary>
	[TestFixture] 
	public class PaginatedArrayListTest
	{
		private PaginatedArrayList _smallPageList = null;
		private PaginatedArrayList _oddPageList = null;
		private PaginatedArrayList _evenPageList = null;

		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
			_smallPageList = new PaginatedArrayList(5);
			_smallPageList.Add(0);
			_smallPageList.Add(1);
			_smallPageList.Add(2);

			_oddPageList = new PaginatedArrayList(5);
			_oddPageList.Add(0);
			_oddPageList.Add(1);
			_oddPageList.Add(2);
			_oddPageList.Add(3);
			_oddPageList.Add(4);
			_oddPageList.Add(5);
			_oddPageList.Add(6);
			_oddPageList.Add(7);
			_oddPageList.Add(8);
			_oddPageList.Add(9);
			_oddPageList.Add(10);
			_oddPageList.Add(11);
			_oddPageList.Add(12);
			_oddPageList.Add(13);
			_oddPageList.Add(14);
			_oddPageList.Add(15);
			_oddPageList.Add(16);
			_oddPageList.Add(17);

			_evenPageList = new PaginatedArrayList(5);
			_evenPageList.Add(0);
			_evenPageList.Add(1);
			_evenPageList.Add(2);
			_evenPageList.Add(3);
			_evenPageList.Add(4);
			_evenPageList.Add(5);
			_evenPageList.Add(6);
			_evenPageList.Add(7);
			_evenPageList.Add(8);
			_evenPageList.Add(9);
			_evenPageList.Add(10);
			_evenPageList.Add(11);
			_evenPageList.Add(12);
			_evenPageList.Add(13);
			_evenPageList.Add(14);
		}


		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ 
		} 

		#endregion

		#region Test PaginatedList

		/// <summary>
		/// Test Odd Paginated Enumerator
		/// </summary>
		[Test] 
		public void TestOddPaginatedIterator() 
		{
			Assert.AreEqual(true, _oddPageList.IsFirstPage);
			Assert.AreEqual(false, _oddPageList.IsPreviousPageAvailable);

			Assert.AreEqual(5, _oddPageList.Count);

			_oddPageList.NextPage();

			Assert.AreEqual(5, _oddPageList.Count);

			_oddPageList.NextPage();

			Assert.AreEqual(true, _oddPageList.IsMiddlePage);
			Assert.AreEqual(5, _oddPageList.Count);

			_oddPageList.NextPage();

			Assert.AreEqual(3, _oddPageList.Count);

			Assert.AreEqual(true, _oddPageList.IsLastPage);
			Assert.AreEqual(false, _oddPageList.IsNextPageAvailable);

			_oddPageList.NextPage();

			Assert.AreEqual(true, _oddPageList.IsLastPage);
			Assert.AreEqual(false, _oddPageList.IsNextPageAvailable);

			_oddPageList.PreviousPage();

			Assert.AreEqual(10, _oddPageList[0]);
			Assert.AreEqual(12, _oddPageList[2]);

			_oddPageList.GotoPage(500);

			Assert.AreEqual(0, _oddPageList[0]);
			Assert.AreEqual(4, _oddPageList[4]);

			_oddPageList.GotoPage(-500);

			Assert.AreEqual(15, _oddPageList[0]);
			Assert.AreEqual(17, _oddPageList[2]);
		}

		/// <summary>
		/// Test Even Paginated IEnumerator
		/// </summary>
		[Test] 
		public void TestEvenPaginatedEnumerator() 
		{
			Assert.AreEqual(true, _evenPageList.IsFirstPage);
			Assert.AreEqual(false, _evenPageList.IsPreviousPageAvailable);

			Assert.AreEqual(5, _evenPageList.Count);

			_evenPageList.NextPage();

			Assert.AreEqual(true, _evenPageList.IsMiddlePage);
			Assert.AreEqual(5, _evenPageList.Count);

			_evenPageList.NextPage();

			Assert.AreEqual(5, _evenPageList.Count);

			Assert.AreEqual(true, _evenPageList.IsLastPage);
			Assert.AreEqual(false, _evenPageList.IsNextPageAvailable);

			_evenPageList.NextPage();

			Assert.AreEqual(10, _evenPageList[0]);
			Assert.AreEqual(14, _evenPageList[4]);

			_evenPageList.PreviousPage();

			Assert.AreEqual(5, _evenPageList[0]);
			Assert.AreEqual(9, _evenPageList[4]);

			_evenPageList.GotoPage(500);

			Assert.AreEqual(0, _evenPageList[0]);
			Assert.AreEqual(4, _evenPageList[4]);

			_evenPageList.GotoPage(-500);

			Assert.AreEqual(10, _evenPageList[0]);
			Assert.AreEqual(14, _evenPageList[4]);		
		}


		/// <summary>
		/// Test Small Paginated IEnumerator
		/// </summary>
		[Test]
		public void TestSmallPaginatedEnumerator() 
		{
			Assert.AreEqual(true, _smallPageList.IsFirstPage);
			Assert.AreEqual(true, _smallPageList.IsLastPage);
			Assert.AreEqual(false, _smallPageList.IsMiddlePage);
			Assert.AreEqual(false, _smallPageList.IsPreviousPageAvailable);
			Assert.AreEqual(false, _smallPageList.IsNextPageAvailable);

			Assert.AreEqual(3, _smallPageList.Count);

			_smallPageList.NextPage();

			Assert.AreEqual(3, _smallPageList.Count);
			Assert.AreEqual(true, _smallPageList.IsFirstPage);
			Assert.AreEqual(true, _smallPageList.IsLastPage);
			Assert.AreEqual(false, _smallPageList.IsMiddlePage);
			Assert.AreEqual(false, _smallPageList.IsPreviousPageAvailable);
			Assert.AreEqual(false, _smallPageList.IsNextPageAvailable);

			_smallPageList.NextPage();

			Assert.AreEqual(3, _smallPageList.Count);

			_smallPageList.NextPage();

			Assert.AreEqual(0, _smallPageList[0]);
			Assert.AreEqual(2, _smallPageList[2]);

			_smallPageList.PreviousPage();

			Assert.AreEqual(0, _smallPageList[0]);
			Assert.AreEqual(2, _smallPageList[2]);

			_smallPageList.GotoPage(500);

			Assert.AreEqual(0, _smallPageList[0]);
			Assert.AreEqual(2, _smallPageList[2]);

			_smallPageList.GotoPage(-500);

			Assert.AreEqual(0, _smallPageList[0]);
			Assert.AreEqual(2, _smallPageList[2]);

			Assert.AreEqual(true, _smallPageList.IsFirstPage);
			Assert.AreEqual(true, _smallPageList.IsLastPage);
			Assert.AreEqual(false, _smallPageList.IsMiddlePage);
			Assert.AreEqual(false, _smallPageList.IsPreviousPageAvailable);
			Assert.AreEqual(false, _smallPageList.IsNextPageAvailable);
		}


		#endregion

	}
}
