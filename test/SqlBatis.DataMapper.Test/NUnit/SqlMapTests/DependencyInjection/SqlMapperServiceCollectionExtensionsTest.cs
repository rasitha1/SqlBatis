using System;
using System.Collections.Generic;
using System.Text;
using IBatisNet.DataMapper.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace IBatisNet.DataMapper.Test.NUnit.SqlMapTests.DependencyInjection
{
    [TestFixture]
    public class SqlMapperServiceCollectionExtensionsTest : BaseTest
    {
        private string _fileName = string.Empty;

        #region SetUp

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            _fileName = "sqlmap" + "_" + Configuration["database"] + "_" + Configuration["providerType"] + ".config";

        }
        #endregion

        [Test]
        public void CanInitializeDefaultSqlMapper()
        {
            var services = new ServiceCollection();
            services.AddSqlMapper(options =>
            {
                options.Resource = _fileName;
                options.Parameters = new Dictionary<string, string>
                {
                    {
                        "collection2Namespace",
                        "IBatisNet.DataMapper.Test.Domain.LineItemCollection, IBatisNet.DataMapper.Test"
                    },
                    {"nullableInt", "int"}
                };

            });

            var sp = services.BuildServiceProvider();

            var mapper = sp.GetRequiredService<ISqlMapper>();

            Assert.IsNotNull(mapper);
        }
    }
}
