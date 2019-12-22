using SqlBatis.DataMapper.Configuration.Cache;
using SqlBatis.DataMapper.Configuration.Cache.Fifo;
using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.CacheController
{
	/// <summary>
	/// Description résumée de FifoCacheControllerTest.
	/// </summary>
	[TestFixture]
	public class FifoCacheControllerTest : LruCacheControllerTest
	{

		protected override ICacheController GetController() 
		{
			return new FifoCacheController();
		}
	}
}
