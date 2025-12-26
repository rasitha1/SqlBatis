using System;
using System.Collections;
using System.Collections.Generic;
using SqlBatis.DataMapper.Test.Domain;

using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests
{
    /// <summary>
    /// Summary description for ParameterMapTest.
    /// </summary>
    [TestFixture]
    public class MultipleResultTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
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


        /// <summary>
        /// Test Multiple ResultMaps
        /// </summary>
        [Test]
        public void TestMultipleResultMapsWithInList()
        {
            IList accounts = new ArrayList();
            sqlMap.QueryForList("GetMultipleResultMapAccount", null, accounts);
            Assert.That(accounts.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test Multiple ResultMaps
        /// </summary>
        [Test]
        public void TestMultipleAccountResultMap()
        {
            Assert.That(sqlMap.QueryForList("GetMultipleResultMapAccount", null).Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test Multiple ResultMaps
        /// </summary>
        [Test]
        public void TestMultipleResultClassWithInList()
        {
            IList accounts = new ArrayList();
            sqlMap.QueryForList("GetMultipleResultClassAccount", null, accounts);
            Assert.That(accounts.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test Multiple Result class
        /// </summary>
        [Test]
        public void TestMultipleAccountResultClass()
        {
            Assert.That(sqlMap.QueryForList("GetMultipleResultClassAccount", null).Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test Multiple ResultMaps
        /// </summary>
        [Test]
        public void TestMultipleResultMap()
        {
            Category category = new Category();
            category.Name = "toto";
            category.Guid = Guid.NewGuid();

            int key = (int)sqlMap.Insert("InsertCategory", category);
            IList list = sqlMap.QueryForList("GetMultipleResultMap", null);
            
            Assert.That(list.Count, Is.EqualTo(2));
            
            Account account = list[0] as Account;
            Category saveCategory = list[01] as Category;
            AssertAccount1(account);
            Assert.That(saveCategory.Id, Is.EqualTo(key));
            Assert.That(saveCategory.Name, Is.EqualTo(category.Name));
            Assert.That(saveCategory.Guid, Is.EqualTo(category.Guid));
        }

        /// <summary>
        /// Test Multiple Result class
        /// </summary>
        [Test]
        public void TestMultipleResultClass()
        {
            Category category = new Category();
            category.Name = "toto";
            category.Guid = Guid.NewGuid();

            int key = (int)sqlMap.Insert("InsertCategory", category);

            IList list = sqlMap.QueryForList("GetMultipleResultClass", null);
            Assert.That(list.Count, Is.EqualTo(2));
        }

        
        /// <summary>
        /// Test Multiple Document
        /// </summary>
        [Test]
        public void TestMultipleDocument()
        {
            IList<Document> list = sqlMap.QueryForList<Document>("GetMultipleDocument", null);

            Assert.That(list.Count, Is.EqualTo(3));
        }
    }
}
