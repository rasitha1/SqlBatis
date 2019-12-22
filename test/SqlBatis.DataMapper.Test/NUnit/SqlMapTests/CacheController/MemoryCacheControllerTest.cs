using SqlBatis.DataMapper.Configuration.Cache;
using SqlBatis.DataMapper.Configuration.Cache.Memory;
using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.CacheController
{
	/// <summary>
	/// Description résumée de MemoryCacheControllerTest.
	/// </summary>
	[TestFixture]
	public class MemoryCacheControllerTest: LruCacheControllerTest
	{

		protected override ICacheController GetController() 
		{
			return new MemoryCacheController();
		}

		[Test]
		public override void TestSizeOne() 
		{
			// This is not relevant for this model
		}
	}
}
