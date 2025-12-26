using System.Collections;
using System.Collections.Generic;
using SqlBatis.DataMapper.Test.Domain;
using NUnit.Framework;

using Category=SqlBatis.DataMapper.Test.Domain.Petshop.Category;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests
{
    /// <summary>
    /// Summary description for GroupByTest.
    /// </summary>
    [TestFixture]
    public class GroupByTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [OneTimeSetUp]
        protected override void SetUpFixture()
        {
            base.SetUpFixture();

            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-drop.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-schema.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "child-parent-init.sql");

        }


        /// <summary>
        /// Dispose the SqlMap
        /// </summary>
        [OneTimeTearDown]
        protected override void TearDownFixture()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-drop.sql");
            base.TearDownFixture();
        }
        #endregion

        [Test]
        public void TestBobHanson ()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "groupby-schema.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "groupby-init.sql");

            IList<Application> list = sqlMap.QueryForList<Application>("GroupByBobHanson", null);
            Assert.That(list.Count, Is.EqualTo(1));
            Application application = list[0];

            Assert.That(application.DefaultRole.Name, Is.EqualTo("Admin"));
            Assert.That(application.Users.Count, Is.EqualTo(2));
            Assert.That(application.Users[0].UserName, Is.EqualTo("user1"));
            Assert.That(application.Users[0].Address, Is.Null);
            
            Assert.That(application.Users[0].Roles.Count, Is.EqualTo(1));
            Assert.That(application.Users[0].Roles[0].Name, Is.EqualTo("User"));

            Assert.That(application.Users[1].Roles.Count, Is.EqualTo(2));
            Assert.That(application.Users[1].Roles[1].Name, Is.EqualTo("User"));
            Assert.That(application.Users[1].Roles[0].Name, Is.EqualTo("Admin"));

        }

        [Test]
        [Category("JIRA-253")]
        public void Issue_When_Using_Sql_Timestamp_Data_Type()
        {
            IList<Parent> parents = sqlMap.QueryForList<Parent>("GetAllParentsNPlus1", null);

            Assert.That(parents[0].Children.Count, Is.EqualTo(2));
            Assert.That(parents[1].Children.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestGroupByWithNullSon() 
        {
            IList list = sqlMap.QueryForList("GetCategories", null);
            Assert.That(list.Count, Is.EqualTo(6));
        }

        [Test]
        public void TestGroupBy()
        {
            IList list = sqlMap.QueryForList("GetAllCategories", null);
            Assert.That(list.Count, Is.EqualTo(5));
        }
        
        [Test]
        public void TestGroupByExtended()  
        {
            IList list = sqlMap.QueryForList("GetAllCategoriesExtended", null);
            Assert.That(list.Count, Is.EqualTo(5));
        }

        [Test]
        public void TestNestedProperties()
        {
            IList list = sqlMap.QueryForList("GetFish", null);
            Assert.That(list.Count, Is.EqualTo(1));

            Domain.Petshop.Category cat = (Domain.Petshop.Category)list[0];
            Assert.That(cat.Id, Is.EqualTo("FISH"));
            Assert.That(cat.Name, Is.EqualTo("Fish"));
            Assert.That(cat.Products, Is.Not.Null.And.With.Property("Count").EqualTo(4), "Expected product list.");
            Assert.That(cat.Products.Count, Is.EqualTo(4));

            Domain.Petshop.Product product = (Domain.Petshop.Product)cat.Products[0];
            Assert.That(product.Items.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestForQueryForObject()
        {
            Domain.Petshop.Category cat = (Domain.Petshop.Category)sqlMap.QueryForObject("GetFish", null);
            Assert.That(cat, Is.Not.Null);

            Assert.That(cat.Id, Is.EqualTo("FISH"));
            Assert.That(cat.Name, Is.EqualTo("Fish"));
            Assert.That(cat.Products, Is.Not.Null.And.With.Property("Count").EqualTo(4), "Expected product list.");
            Assert.That(cat.Products.Count, Is.EqualTo(4));

            Domain.Petshop.Product product = (Domain.Petshop.Product)cat.Products[0];
            Assert.That(product.Items.Count, Is.EqualTo(2));
        }


        [Test]
        public void TestGenericFish()
        {
            IList list = sqlMap.QueryForList("GetFishGeneric", null);
            Assert.That(list.Count, Is.EqualTo(1));

            Domain.Petshop.Category cat = (Domain.Petshop.Category)list[0];
            Assert.That(cat.Id, Is.EqualTo("FISH"));
            Assert.That(cat.Name, Is.EqualTo("Fish"));
            Assert.That(cat.GenericProducts, Is.Not.Null.And.With.Property("Count").EqualTo(4), "Expected product list.");
            Assert.That(cat.GenericProducts.Count, Is.EqualTo(4));

            Domain.Petshop.Product product = cat.GenericProducts[0];
            Assert.That(product.GenericItems.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestForQueryForObjectGeneric()
        {
            Domain.Petshop.Category cat = sqlMap.QueryForObject<Domain.Petshop.Category>("GetFishGeneric", null);
            Assert.That(cat, Is.Not.Null);

            Assert.That(cat.Id, Is.EqualTo("FISH"));
            Assert.That(cat.Name, Is.EqualTo("Fish"));
            Assert.That(cat.GenericProducts, Is.Not.Null.And.With.Property("Count").EqualTo(4), "Expected product list.");
            Assert.That(cat.GenericProducts.Count, Is.EqualTo(4));

            Domain.Petshop.Product product = cat.GenericProducts[0];
            Assert.That(product.GenericItems.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestJira241()
        {
            Category myCategory = new Category();

            sqlMap.QueryForObject<Category>("GetFishGeneric", null, myCategory);
            Assert.That(myCategory, Is.Not.Null);

            Assert.That(myCategory.Id, Is.EqualTo("FISH"));
            Assert.That(myCategory.Name, Is.EqualTo("Fish"));
            Assert.That(myCategory.GenericProducts, Is.Not.Null.And.With.Property("Count").EqualTo(4), "Expected product list.");
            Assert.That(myCategory.GenericProducts.Count, Is.EqualTo(4));

            Domain.Petshop.Product product = myCategory.GenericProducts[0];
            Assert.That(product.GenericItems.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestGenericList()
        {
            IList<Domain.Petshop.Category> list = sqlMap.QueryForList<Domain.Petshop.Category>("GetFishGeneric", null);
            Assert.That(list.Count, Is.EqualTo(1));

            Domain.Petshop.Category cat = list[0];
            Assert.That(cat.Id, Is.EqualTo("FISH"));
            Assert.That(cat.Name, Is.EqualTo("Fish"));
            Assert.That(cat.GenericProducts, Is.Not.Null.And.With.Property("Count").EqualTo(4), "Expected product list.");
            Assert.That(cat.GenericProducts.Count, Is.EqualTo(4));

            Domain.Petshop.Product product = cat.GenericProducts[0];
            Assert.That(product.GenericItems.Count, Is.EqualTo(2));
        }
        
        [Test]
        public void TestGroupByNull()
        {
            IList list = sqlMap.QueryForList("GetAllProductCategoriesJIRA250", null);
            Domain.Petshop.Category cat = (Domain.Petshop.Category)list[0];
            Assert.That(cat.Products.Count, Is.EqualTo(0));
        }
        
        /// <summary>
        /// Test Select N+1 on Order/LineItem
        /// </summary>
        [Test]
        public void TestOrderLineItemGroupBy()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-drop.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");

            Order order = new Order();
            order.Id = 11;
            LineItem item = new LineItem();
            item.Id = 10;
            item.Code = "blah";
            item.Price = 44.00m;
            item.Quantity = 1;
            item.Order = order;

            sqlMap.Insert("InsertLineItemPostKey", item);

            
            IList list = sqlMap.QueryForList("GetOrderLineItem", null);

            Assert.That(list.Count, Is.EqualTo(11));
            
            order = (Order)list[0];
            Assert.That(order.LineItemsIList.Count, Is.EqualTo(3));
            Assert.That(order.Account, Is.Not.Null);
            AssertAccount1(order.Account);

            order = (Order)list[10];
            Assert.That(order.LineItemsIList.Count, Is.EqualTo(1));
            Assert.That(order.Account, Is.Null);
        }

        /// <summary>
        /// Test GroupBy With use of Inheritance
        /// </summary>
        [Test]
        public void GroupByWithInheritance()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-drop.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "documents-init.sql");

            IList<Account> list = sqlMap.QueryForList<Account>("JIRA206", null);
            
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0].Documents.Count, Is.EqualTo(0));
            Assert.That(list[1].Documents.Count, Is.EqualTo(2));
            Assert.That(list[2].Documents.Count, Is.EqualTo(1));
            Assert.That(list[3].Documents.Count, Is.EqualTo(0));
            Assert.That(list[4].Documents.Count, Is.EqualTo(2));

            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-drop.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-schema.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "petstore-init.sql");
        }
    }
}
