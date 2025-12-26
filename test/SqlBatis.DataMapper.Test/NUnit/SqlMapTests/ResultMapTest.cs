using System;
using System.Collections;
using System.Configuration;

using NUnit.Framework;

using SqlBatis.DataMapper.Test.Domain;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests
{
    /// <summary>
    /// Summary description for ResultMapTest.
    /// </summary>
    [TestFixture]
    public class ResultMapTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-procedure.sql", false);
            InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "enumeration-init.sql");
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        { /* ... */ }

        #endregion

        #region Result Map test

        /// <summary>
        /// Test a Result Map property with map by column name
        /// </summary>
        [Test]
        public void TestColumnsByName()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderLiteByColumnName", 1);
            AssertOrder1(order);
        }

        /// <summary>
        /// Test a Result Map property with map by column index
        /// </summary>
        [Test]
        public void TestColumnsByIndex()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderLiteByColumnIndex", 1);
            AssertOrder1(order);
        }


        /// <summary>
        /// Test extends attribute in a Result Map
        /// </summary>
        [Test]
        public void TestExtendedResultMap()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithLineItems", 1);

            AssertOrder1(order);
            Assert.That(order.LineItemsIList, Is.Not.Null);
            Assert.That(order.LineItemsIList.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test lazyLoad attribute in a Result Map property
        /// </summary>
        [Test]
        public void TestLazyLoad()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithLineItems", 1);

            AssertOrder1(order);

            Assert.That(order.LineItemsIList, Is.Not.Null);
            Assert.That(typeof(IList).IsAssignableFrom(order.LineItemsIList.GetType()), Is.True);

            Assert.That(order.LineItemsIList.Count, Is.EqualTo(3));
            // After a call to a method from a proxy object,
            // the proxy object is replaced by his real object.
            Assert.That(order.LineItemsIList, Is.InstanceOf<ArrayList>());
        }

        /// <summary>
        /// Test lazyLoad attribute With an Open Connection
        /// </summary>
        [Test]
        public void TestLazyLoadWithOpenConnection()
        {
            sqlMap.OpenConnection();

            Order order = (Order)sqlMap.QueryForObject("GetOrderWithLineItems", 1);

            AssertOrder1(order);

            Assert.That(order.LineItemsIList, Is.Not.Null);
            Assert.That(typeof(IList).IsAssignableFrom(order.LineItemsIList.GetType()), Is.True);

            Assert.That(order.LineItemsIList.Count, Is.EqualTo(3));
            // After a call to a method from a proxy object,
            // the proxy object is replaced by his real object.
            Assert.That(order.LineItemsIList, Is.InstanceOf<ArrayList>());

            sqlMap.CloseConnection();
        }

        /// <summary>
        /// Test collection mapping
        /// order.LineItems
        /// </summary>
        [Test]
        public void TestLazyWithStronglyTypedCollectionMapping()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithLineItemCollection", 1);

            AssertOrder1(order);

            Assert.That(order.LineItemsCollection, Is.Not.Null);
            Assert.That(order.LineItemsCollection.Count, Is.EqualTo(3));

            IEnumerator e = ((IEnumerable)order.LineItemsCollection).GetEnumerator();
            while (e.MoveNext())
            {
                LineItem item = (LineItem)e.Current;
                Assert.That(item, Is.Not.Null);
            }
        }

        /// <summary>
        /// Test null value replacement(on string) in a Result property.
        /// </summary>
        [Test]
        public void TestNullValueReplacementOnString()
        {
            Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 3);
            Assert.That(account.EmailAddress, Is.EqualTo("no_email@provided.com"));
        }

        /// <summary>
        /// Test null value replacement(on enum class) in a Result property.
        /// </summary>
        [Test]
        public void TestNullValueReplacementOnEnum()
        {
            Enumeration enumClass = new Enumeration();
            enumClass.Id = 99;
            enumClass.Day = Days.Thu;
            enumClass.Color = Colors.Blue;
            enumClass.Month = Months.All;

            sqlMap.Insert("InsertEnumViaParameterMap", enumClass);

            enumClass = null;
            enumClass = sqlMap.QueryForObject("GetEnumerationNullValue", 99) as Enumeration;

            Assert.That(enumClass.Day, Is.EqualTo(Days.Thu));
            Assert.That(enumClass.Color, Is.EqualTo(Colors.Blue));
            Assert.That(enumClass.Month, Is.EqualTo(Months.All));
        }

        /// <summary>
        /// Test usage of dbType in a result map property.
        /// 
        /// </summary>
        [Test]
        public void TestTypeSpecified()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithTypes", 1);
            AssertOrder1(order);
        }


        /// <summary>
        /// Test a Complex Object Mapping. 
        /// Order + Account in Order.Account
        /// </summary>
        [Test]
        public void TestComplexObjectMapping()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithAccount", 1);
            AssertOrder1(order);
            AssertAccount1(order.Account);
        }


        /// <summary>
        /// Test collection mapping with extends attribute
        /// </summary>
        [Test]
        public void TestCollectionMappingAndExtends()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithLineItemsCollection", 1);

            AssertOrder1(order);

            // Check strongly typed collection
            Assert.That(order.LineItemsCollection, Is.Not.Null);
            Assert.That(order.LineItemsCollection.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test collection mapping: Ilist collection 
        /// order.LineItemsIList 
        /// </summary>
        [Test]
        public void TestListMapping()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithLineItems", 1);

            AssertOrder1(order);

            // Check IList collection
            Assert.That(order.LineItemsIList, Is.Not.Null);
            Assert.That(order.LineItemsIList.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test Array Mapping
        /// </summary>
        [Test]
        public void TestArrayMapping()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithLineItemArray", 1);

            AssertOrder1(order);
            Assert.That(order.LineItemsArray, Is.Not.Null);
            Assert.That(order.LineItemsArray.Length, Is.EqualTo(3));
        }

        /// <summary>
        /// Test collection mapping
        /// order.LineItems
        /// </summary>
        [Test]
        public void TestStronglyTypedCollectionMapping()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderWithLineItemCollection", 1);

            AssertOrder1(order);

            Assert.That(order.LineItemsCollection, Is.Not.Null);
            Assert.That(order.LineItemsCollection.Count, Is.EqualTo(3));

            IEnumerator e = ((IEnumerable)order.LineItemsCollection).GetEnumerator();
            while (e.MoveNext())
            {
                LineItem item = (LineItem)e.Current;
                Assert.That(item, Is.Not.Null);
            }
        }

        /// <summary>
        /// Test a ResultMap mapping as an Hastable.
        /// </summary>
        [Test]
        public void TestHashtableMapping()
        {
            Hashtable order = (Hashtable)sqlMap.QueryForObject("GetOrderAsHastable", 1);

            AssertOrder1AsHashtable(order);
        }

        /// <summary>
        /// Test nested object.
        /// Order + FavouriteLineItem in order.FavouriteLineItem
        /// </summary>
        [Test]
        public void TestNestedObjects()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderJoinedFavourite", 1);

            AssertOrder1(order);

            Assert.That(order.FavouriteLineItem, Is.Not.Null);
            Assert.That(order.FavouriteLineItem.Id, Is.EqualTo(1), "order.FavouriteLineItem.Id");
            Assert.That(order.FavouriteLineItem.Code, Is.EqualTo("ESM-34"));

        }

        /// <summary>
        /// Test nested object.
        /// Order + FavouriteLineItem in order.FavouriteLineItem
        /// </summary>
        [Test]
        public void TestNestedObjects2()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderJoinedFavourite2", 1);

            AssertOrder1(order);

            Assert.That(order.FavouriteLineItem, Is.Not.Null);
            Assert.That(order.FavouriteLineItem.Id, Is.EqualTo(1), "order.FavouriteLineItem.Id");
            Assert.That(order.FavouriteLineItem.Code, Is.EqualTo("ESM-34"));
        }

        /// <summary>
        /// Test Implicit Result Maps
        /// </summary>
        [Test]
        public void TestImplicitResultMaps()
        {
            Order order = (Order)sqlMap.QueryForObject("GetOrderJoinedFavourite3", 1);

            AssertOrder1(order);

            Assert.That(order.FavouriteLineItem, Is.Not.Null);
            Assert.That(order.FavouriteLineItem.Id, Is.EqualTo(1), "order.FavouriteLineItem.Id");
            Assert.That(order.FavouriteLineItem.Code, Is.EqualTo("ESM-34"));

        }

        /// <summary>
        /// Test a composite Key Mapping.
        /// It must be: property1=column1,property2=column2,...
        /// </summary>
        [Test]
        public void TestCompositeKeyMapping()
        {
            Order order1 = (Order)sqlMap.QueryForObject("GetOrderWithFavouriteLineItem", 1);
            Order order2 = (Order)sqlMap.QueryForObject("GetOrderWithFavouriteLineItem", 2);

            Assert.That(order1, Is.Not.Null);
            Assert.That(order1.FavouriteLineItem, Is.Not.Null);
            Assert.That(order1.FavouriteLineItem.Id, Is.EqualTo(1));

            Assert.That(order2, Is.Not.Null);
            Assert.That(order2.FavouriteLineItem, Is.Not.Null);
            Assert.That(order2.FavouriteLineItem.Id, Is.EqualTo(17));
        }

        /// <summary>
        /// Test a composite Key Mapping.
        /// It must be: key1,key2,... (old syntax)
        /// </summary>
        [Test]
        public void TestCompositeKeyMapping_JIRA_251()
        {
            Order order1 = (Order)sqlMap.QueryForObject("GetOrderWithFavouriteLineItem-JIRA-251", 1);
            Order order2 = (Order)sqlMap.QueryForObject("GetOrderWithFavouriteLineItem-JIRA-251", 2);

            Assert.That(order1, Is.Not.Null);
            Assert.That(order1.FavouriteLineItem, Is.Not.Null);
            Assert.That(order1.FavouriteLineItem.Id, Is.EqualTo(1));

            Assert.That(order2, Is.Not.Null);
            Assert.That(order2.FavouriteLineItem, Is.Not.Null);
            Assert.That(order2.FavouriteLineItem.Id, Is.EqualTo(17));

        }

        /// <summary>
        /// Test Dynamique Composite Key Mapping
        /// </summary>
        [Test]
        public void TestDynamiqueCompositeKeyMapping()
        {

            Order order1 = (Order)sqlMap.QueryForObject("GetOrderWithDynFavouriteLineItem", 1);

            Assert.That(order1, Is.Not.Null);
            Assert.That(order1.FavouriteLineItem, Is.Not.Null);
            Assert.That(order1.FavouriteLineItem.Id, Is.EqualTo(1));
        }

        /// <summary>
        /// Test a simple type mapping (string)
        /// </summary>
        [Test]
        public void TestSimpleTypeMapping()
        {
            IList list = sqlMap.QueryForList("GetAllCreditCardNumbersFromOrders", null);

            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0], Is.EqualTo("555555555555"));
        }

        /// <summary>
        /// Test a simple type mapping (decimal)
        /// </summary>
        [Test]
        public void TestDecimalTypeMapping()
        {
            Hashtable param = new Hashtable();
            param.Add("LineItem_ID", 1);
            param.Add("Order_ID", 1);
            decimal price = (decimal)sqlMap.QueryForObject("GetLineItemPrice", param);
            Assert.That(price, Is.EqualTo(45.43m));
        }

        /// <summary>
        /// Test Byte Array Mapping
        /// </summary>
        /// <remarks>Test for request support 1032436 ByteArrayTypeHandler misses the last byte</remarks>
        [Test]
        public void TestByteArrayMapping()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            Order order = new Order();
            order.Id = 99;
            order.CardExpiry = "09/11";
            order.Account = account;
            order.CardNumber = "154564656";
            order.CardType = "Visa";
            order.City = "Lyon";
            order.Date = System.DateTime.MinValue;
            order.PostalCode = "69004";
            order.Province = "Rhone";
            order.Street = "rue Durand";

            sqlMap.Insert("InsertOrderViaParameterMap", order);

            LineItem item = new LineItem();
            item.Id = 99;
            item.Code = "test";
            item.Price = -99.99m;
            item.Quantity = 99;
            item.Order = order;
            item.PictureData = new byte[] { 1, 2, 3 };

            // Check insert
            sqlMap.Insert("InsertLineItemWithPicture", item);

            // select
            LineItem loadItem = null;

            Hashtable param = new Hashtable();
            param.Add("LineItem_ID", 99);
            param.Add("Order_ID", 99);

            loadItem = sqlMap.QueryForObject("GetSpecificLineItemWithPicture", param) as LineItem;

            Assert.That(loadItem.Id, Is.Not.Null);
            Assert.That(loadItem.PictureData, Is.Not.Null);
            Assert.That(loadItem.PictureData, Is.EqualTo(item.PictureData));
        }

        /// <summary>
        /// Test null replacement (on decimal) in ResultMap property
        /// </summary>
        [Test]
        public void TestNullValueReplacementOnDecimal()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            Order order = new Order();
            order.Id = 99;
            order.CardExpiry = "09/11";
            order.Account = account;
            order.CardNumber = "154564656";
            order.CardType = "Visa";
            order.City = "Lyon";
            order.Date = System.DateTime.MinValue; //<-- null replacement for parameterMAp 
            order.PostalCode = "69004";
            order.Province = "Rhone";
            order.Street = "rue Durand";

            sqlMap.Insert("InsertOrderViaParameterMap", order);

            LineItem item = new LineItem();
            item.Id = 99;
            item.Code = "test";
            item.Price = -99.99m;//<-- null replacement for parameterMAp 
            item.Quantity = 99;
            item.Order = order;

            sqlMap.Insert("InsertLineItem", item);

            // Retrieve LineItem & test null replacement for resultMap 

            LineItem testItem = (LineItem)sqlMap.QueryForObject("GetSpecificLineItemWithNullReplacement", 99);

            Assert.That(testItem, Is.Not.Null);
            Assert.That(testItem.Price, Is.EqualTo(-77.77m));
            Assert.That(testItem.Code, Is.EqualTo("test"));
        }

        /// <summary>
        /// Test null replacement (on DateTime) in ResultMap property.
        /// </summary>
        [Test]
        public void TestNullValueReplacementOnDateTime()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            Order order = new Order();
            order.Id = 99;
            order.CardExpiry = "09/11";
            order.Account = account;
            order.CardNumber = "154564656";
            order.CardType = "Visa";
            order.City = "Lyon";
            order.Date = System.DateTime.MinValue; //<-- null replacement
            order.PostalCode = "69004";
            order.Province = "Rhone";
            order.Street = "rue Durand";

            sqlMap.Insert("InsertOrderViaParameterMap", order);

            Order orderTest = (Order)sqlMap.QueryForObject("GetOrderLiteByColumnName", 99);

            Assert.That(orderTest.Date, Is.EqualTo(System.DateTime.MinValue));
        }

        #endregion

    }
}
