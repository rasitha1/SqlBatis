using System;
using System.Collections;

using SqlBatis.DataMapper.Pagination;

using SqlBatis.DataMapper.Test.NUnit;

using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.CommonTests.Pagination
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
			Assert.That(_oddPageList.IsFirstPage, Is.True);
			Assert.That(_oddPageList.IsPreviousPageAvailable, Is.False);

			Assert.That(_oddPageList.Count, Is.EqualTo(5));

			_oddPageList.NextPage();

			Assert.That(_oddPageList.Count, Is.EqualTo(5));

			_oddPageList.NextPage();

			Assert.That(_oddPageList.IsMiddlePage, Is.True);
			Assert.That(_oddPageList.Count, Is.EqualTo(5));

			_oddPageList.NextPage();

			Assert.That(_oddPageList.Count, Is.EqualTo(3));

			Assert.That(_oddPageList.IsLastPage, Is.True);
			Assert.That(_oddPageList.IsNextPageAvailable, Is.False);

			_oddPageList.NextPage();

			Assert.That(_oddPageList.IsLastPage, Is.True);
			Assert.That(_oddPageList.IsNextPageAvailable, Is.False);

			_oddPageList.PreviousPage();

			Assert.That(_oddPageList[0], Is.EqualTo(10));
			Assert.That(_oddPageList[2], Is.EqualTo(12));

			_oddPageList.GotoPage(500);

			Assert.That(_oddPageList[0], Is.EqualTo(0));
			Assert.That(_oddPageList[4], Is.EqualTo(4));

			_oddPageList.GotoPage(-500);

			Assert.That(_oddPageList[0], Is.EqualTo(15));
			Assert.That(_oddPageList[2], Is.EqualTo(17));
		}

		/// <summary>
		/// Test Even Paginated IEnumerator
		/// </summary>
		[Test] 
		public void TestEvenPaginatedEnumerator() 
		{
			Assert.That(_evenPageList.IsFirstPage, Is.EqualTo(true));
			Assert.That(_evenPageList.IsPreviousPageAvailable, Is.EqualTo(false));

			Assert.That(_evenPageList.Count, Is.EqualTo(5));

			_evenPageList.NextPage();

			Assert.That(_evenPageList.IsMiddlePage, Is.EqualTo(true));
			Assert.That(_evenPageList.Count, Is.EqualTo(5));

			_evenPageList.NextPage();

			Assert.That(_evenPageList.Count, Is.EqualTo(5));

			Assert.That(_evenPageList.IsLastPage, Is.EqualTo(true));
			Assert.That(_evenPageList.IsNextPageAvailable, Is.EqualTo(false));

			_evenPageList.NextPage();

			Assert.That(_evenPageList[0], Is.EqualTo(10));
			Assert.That(_evenPageList[4], Is.EqualTo(14));

			_evenPageList.PreviousPage();

			Assert.That(_evenPageList[0], Is.EqualTo(5));
			Assert.That(_evenPageList[4], Is.EqualTo(9));

			_evenPageList.GotoPage(500);

			Assert.That(_evenPageList[0], Is.EqualTo(0));
			Assert.That(_evenPageList[4], Is.EqualTo(4));

			_evenPageList.GotoPage(-500);

			Assert.That(_evenPageList[0], Is.EqualTo(10));
			Assert.That(_evenPageList[4], Is.EqualTo(14));		
		}


		/// <summary>
		/// Test Small Paginated IEnumerator
		/// </summary>
		[Test]
		public void TestSmallPaginatedEnumerator() 
		{
			Assert.That(_smallPageList.IsFirstPage, Is.EqualTo(true));
			Assert.That(_smallPageList.IsLastPage, Is.EqualTo(true));
			Assert.That(_smallPageList.IsMiddlePage, Is.EqualTo(false));
			Assert.That(_smallPageList.IsPreviousPageAvailable, Is.EqualTo(false));
			Assert.That(_smallPageList.IsNextPageAvailable, Is.EqualTo(false));

			Assert.That(_smallPageList.Count, Is.EqualTo(3));

			_smallPageList.NextPage();

			Assert.That(_smallPageList.Count, Is.EqualTo(3));
			Assert.That(_smallPageList.IsFirstPage, Is.EqualTo(true));
			Assert.That(_smallPageList.IsLastPage, Is.EqualTo(true));
			Assert.That(_smallPageList.IsMiddlePage, Is.EqualTo(false));
			Assert.That(_smallPageList.IsPreviousPageAvailable, Is.EqualTo(false));
			Assert.That(_smallPageList.IsNextPageAvailable, Is.EqualTo(false));

			_smallPageList.NextPage();

			Assert.That(_smallPageList.Count, Is.EqualTo(3));

			_smallPageList.NextPage();

			Assert.That(_smallPageList[0], Is.EqualTo(0));
			Assert.That(_smallPageList[2], Is.EqualTo(2));

			_smallPageList.PreviousPage();

			Assert.That(_smallPageList[0], Is.EqualTo(0));
			Assert.That(_smallPageList[2], Is.EqualTo(2));

			_smallPageList.GotoPage(500);

			Assert.That(_smallPageList[0], Is.EqualTo(0));
			Assert.That(_smallPageList[2], Is.EqualTo(2));

			_smallPageList.GotoPage(-500);

			Assert.That(_smallPageList[0], Is.EqualTo(0));
			Assert.That(_smallPageList[2], Is.EqualTo(2));

			Assert.That(_smallPageList.IsFirstPage, Is.EqualTo(true));
			Assert.That(_smallPageList.IsLastPage, Is.EqualTo(true));
			Assert.That(_smallPageList.IsMiddlePage, Is.EqualTo(false));
			Assert.That(_smallPageList.IsPreviousPageAvailable, Is.EqualTo(false));
			Assert.That(_smallPageList.IsNextPageAvailable, Is.EqualTo(false));
		}


		#endregion

	}
}
