using System;
using System.Collections.Generic;
using System.Text;
using SqlBatis.DataMapper.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.DependencyInjection
{
    [TestFixture]
    public class SqlMapperExtensionsTest : BaseTest
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
                    { "useStatementNamespaces", "false"},
                    {
                        "collection2Namespace",
                        "SqlBatis.DataMapper.Test.Domain.LineItemCollection, SqlBatis.DataMapper.Test"
                    },
                    {"nullableInt", "int"}
                };

            });

            var sp = services.BuildServiceProvider();

            var mapper = sp.GetRequiredService<ISqlMapper>();

            Assert.That(mapper, Is.Not.Null);
        }

        [Test]
        public void MissingResourceValueShouldThrow()
        {
            var services = new ServiceCollection();
            services.AddSqlMapper(options =>
            {

            });

            var sp = services.BuildServiceProvider();
            Assert.That(() => sp.GetRequiredService<ISqlMapper>(), Throws.TypeOf<OptionsValidationException>());
        }


        [Test]
        public void CanInitializeNamedSqlMappers()
        {
            var services = new ServiceCollection();
            services.AddSqlMapper("A", options =>
            {
                options.Resource = _fileName;
                options.Parameters = new Dictionary<string, string>
                {
                    { "useStatementNamespaces", "true"},
                    {
                        "collection2Namespace",
                        "SqlBatis.DataMapper.Test.Domain.LineItemCollection, SqlBatis.DataMapper.Test"
                    },
                    {"nullableInt", "int"}
                };

            });

            services.AddSqlMapper("B", options =>
            {
                options.Resource = _fileName;
                options.Parameters = new Dictionary<string, string>
                {
                    { "useStatementNamespaces", "true"},
                    {
                        "collection2Namespace",
                        "SqlBatis.DataMapper.Test.Domain.LineItemCollection, SqlBatis.DataMapper.Test"
                    },
                    {"nullableInt", "int"}
                };

            });

            var sp = services.BuildServiceProvider();

            var factory = sp.GetRequiredService<ISqlMapperFactory>();

            var mapperA = factory.GetMapper("A");
            var mapperB = factory.GetMapper("B");

            Assert.That(mapperA, Is.Not.Null);
            Assert.That(mapperB, Is.Not.Null);
            Assert.That(mapperA, Is.Not.SameAs(mapperB));

            var newMapperA = factory.GetMapper("A");
            var newMapperB = factory.GetMapper("B");

            Assert.That(mapperA, Is.SameAs(newMapperA));
            Assert.That(mapperB, Is.SameAs(newMapperB));
        }

        [Test]
        public void InvalidNamedInstanceShouldThrow()
        {
            var services = new ServiceCollection();
            services.AddSqlMapper("A", options =>
            {
                options.Resource = _fileName;
                options.Parameters = new Dictionary<string, string>
                {
                    { "useStatementNamespaces", "true"},
                    {
                        "collection2Namespace",
                        "SqlBatis.DataMapper.Test.Domain.LineItemCollection, SqlBatis.DataMapper.Test"
                    },
                    {"nullableInt", "int"}
                };

            });

            var sp = services.BuildServiceProvider();

            Assert.That(sp.GetRequiredService<ISqlMapperFactory>().GetMapper("A"), Is.Not.Null);
            Assert.That(() => sp.GetRequiredService<ISqlMapperFactory>().GetMapper("B"), Throws.TypeOf<OptionsValidationException>());
            
        }
    }
}
