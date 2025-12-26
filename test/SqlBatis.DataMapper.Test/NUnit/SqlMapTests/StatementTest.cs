using System;
using System.Collections;
using SqlBatis.DataMapper;
using SqlBatis.DataMapper.Exceptions;
using SqlBatis.DataMapper.Utilities;
using SqlBatis.DataMapper.MappedStatements;
using SqlBatis.DataMapper.Test.Domain;
using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests
{
    /// <summary>
    /// Summary description for ParameterMapTest.
    /// </summary>
    [TestFixture]
    public class StatementTest : BaseTest
    {
        #region SetUp & TearDown

        /// <summary>
        /// SetUp
        /// </summary>
        [SetUp]
        public void Init()
        {
            InitScript(sqlMap.DataSource, ScriptDirectory + "account-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "order-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "line-item-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "enumeration-init.sql");
            InitScript(sqlMap.DataSource, ScriptDirectory + "other-init.sql");
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void Dispose()
        { /* ... */
        }

        #endregion

        #region Object Query tests

        /// <summary>
        /// Interface mapping
        /// </summary>
        [Test]
        [Category("JIRA")]
        [Description("JIRA-283")]
        public void TestInterface()
        {
            BaseAccount account = new BaseAccount();

            sqlMap.QueryForObject<IAccount>("GetInterfaceAccount", 1, account);

            Assert.That(account.Id, Is.EqualTo(1), "account.Id");
            Assert.That(account.FirstName, Is.EqualTo("Joe"), "account.FirstName");
            Assert.That(account.LastName, Is.EqualTo("Dalton"), "account.LastName");
            Assert.That(account.EmailAddress, Is.EqualTo("Joe.Dalton@somewhere.com"), "account.EmailAddress");
        }

        /// <summary>
        /// Test Open connection with a connection string
        /// </summary>
        [Test]
        public void TestOpenConnection()
        {
            sqlMap.OpenConnection(sqlMap.DataSource.ConnectionString);
            Account account = sqlMap.QueryForObject("SelectWithProperty", null) as Account;
            sqlMap.CloseConnection();

            AssertAccount1(account);
        }

        /// <summary>
        /// Test use a statement with property subtitution
        /// (JIRA 22)
        /// </summary>
        [Test]
        public void TestSelectWithProperty()
        {
            Account account = sqlMap.QueryForObject("SelectWithProperty", null) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ColumnName
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaColumnName()
        {
            Account account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ColumnIndex
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaColumnIndex()
        {
            Account account = sqlMap.QueryForObject("GetAccountViaColumnIndex", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ResultClass
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaResultClass()
        {
            Account account = sqlMap.QueryForObject("GetAccountViaResultClass", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject With simple ResultClass : string
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithSimpleResultClass()
        {
            string email = sqlMap.QueryForObject("GetEmailAddressViaResultClass", 1) as string;
            Assert.That(email, Is.EqualTo("Joe.Dalton@somewhere.com"));
        }

        /// <summary>
        /// Test ExecuteQueryForObject With simple ResultMap : string
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithSimpleResultMap()
        {
            string email = sqlMap.QueryForObject("GetEmailAddressViaResultMap", 1) as string;
            Assert.That(email, Is.EqualTo("Joe.Dalton@somewhere.com"));
        }

        /// <summary>
        /// Test Primitive ReturnValue : System.DateTime
        /// </summary>
        [Test]
        public void TestPrimitiveReturnValue()
        {
            DateTime CardExpiry = (DateTime)sqlMap.QueryForObject("GetOrderCardExpiryViaResultClass", 1);
            Assert.That(CardExpiry, Is.EqualTo(new DateTime(2003, 02, 15, 8, 15, 00)));
        }

        /// <summary>
        /// Test ExecuteQueryForObject with result object : Account
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithResultObject()
        {
            Account account = new Account();
            Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnName", 1, account) as Account;
            AssertAccount1(account);
            Assert.That(account == testAccount, Is.True);
        }

        /// <summary>
        /// Test ExecuteQueryForObject as Hashtable
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectAsHashtable()
        {
            Hashtable account = (Hashtable)sqlMap.QueryForObject("GetAccountAsHashtable", 1);
            AssertAccount1AsHashtable(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject as Hashtable ResultClass
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectAsHashtableResultClass()
        {
            Hashtable account = (Hashtable)sqlMap.QueryForObject("GetAccountAsHashtableResultClass", 1);
            AssertAccount1AsHashtableForResultClass(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject via Hashtable
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaHashtable()
        {
            Hashtable param = new Hashtable();
            param.Add("LineItem_ID", 4);
            param.Add("Order_ID", 9);

            LineItem testItem = sqlMap.QueryForObject("GetSpecificLineItem", param) as LineItem;

            Assert.That(testItem, Is.Not.Null);
            Assert.That(testItem.Code, Is.EqualTo("TSM-12"));
        }

        /// <summary>
        /// Test Query Dynamic Sql Element
        /// </summary>
        [Test]
        public void TestQueryDynamicSqlElement()
        {
            IList list = sqlMap.QueryForList("GetDynamicOrderedEmailAddressesViaResultMap", "Account_ID");

            Assert.That((string)list[0], Is.EqualTo("Joe.Dalton@somewhere.com"));

            list = sqlMap.QueryForList("GetDynamicOrderedEmailAddressesViaResultMap", "Account_FirstName");

            Assert.That((string)list[0], Is.EqualTo("Averel.Dalton@somewhere.com"));

        }

        /// <summary>
        /// Test Execute QueryForList With ResultMap With Dynamic Element
        /// </summary>
        [Test]
        public void TestExecuteQueryForListWithResultMapWithDynamicElement()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaResultMapWithDynamicElement", "LIKE");

            AssertAccount1((Account)list[0]);
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(((Account)list[0]).Id, Is.EqualTo(1));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(4));

            list = sqlMap.QueryForList("GetAllAccountsViaResultMapWithDynamicElement", "=");

            Assert.That(list.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test Simple Dynamic Substitution
        /// </summary>
        [Test]
        [Ignore("No longer supported.")]
        public void TestSimpleDynamicSubstitution()
        {
            string statement = "select" + "    Account_ID          as Id," + "    Account_FirstName   as FirstName," + "    Account_LastName    as LastName," + "    Account_Email       as EmailAddress" + "  from Accounts" + "  WHERE Account_ID = #id#";

            Hashtable param = new Hashtable();
            param.Add("id", 1);
            param.Add("statement", statement);


            IList list = sqlMap.QueryForList("SimpleDynamicSubstitution", param);
            AssertAccount1((Account)list[0]);
            Assert.That(list.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Test Get Account Via Inline Parameters
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaInlineParameters()
        {
            Account account = new Account();
            account.Id = 1;

            Account testAccount = sqlMap.QueryForObject("GetAccountViaInlineParameters", account) as Account;

            AssertAccount1(testAccount);
        }

        /// <summary>
        /// Test ExecuteQuery For Object With Enum property
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithEnum()
        {
            Enumeration enumClass = sqlMap.QueryForObject("GetEnumeration", 1) as Enumeration;

            Assert.That(enumClass.Day, Is.EqualTo(Days.Sat));
            Assert.That(enumClass.Color, Is.EqualTo(Colors.Red));
            Assert.That(enumClass.Month, Is.EqualTo(Months.August));

            enumClass = sqlMap.QueryForObject("GetEnumeration", 3) as Enumeration;

            Assert.That(enumClass.Day, Is.EqualTo(Days.Mon));
            Assert.That(enumClass.Color, Is.EqualTo(Colors.Blue));
            Assert.That(enumClass.Month, Is.EqualTo(Months.September));
        }

        #endregion

        #region  List Query tests

        /// <summary>
        /// Test QueryForList with Hashtable ResultMap
        /// </summary>
        [Test]
        public void TestQueryForListWithHashtableResultMap()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsAsHashMapViaResultMap", null);

            AssertAccount1AsHashtable((Hashtable)list[0]);
            Assert.That(list.Count, Is.EqualTo(5));

            Assert.That(((Hashtable)list[0])["Id"], Is.EqualTo(1));
            Assert.That(((Hashtable)list[1])["Id"], Is.EqualTo(2));
            Assert.That(((Hashtable)list[2])["Id"], Is.EqualTo(3));
            Assert.That(((Hashtable)list[3])["Id"], Is.EqualTo(4));
            Assert.That(((Hashtable)list[4])["Id"], Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with Hashtable ResultClass
        /// </summary>
        [Test]
        public void TestQueryForListWithHashtableResultClass()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsAsHashtableViaResultClass", null);

            AssertAccount1AsHashtableForResultClass((Hashtable)list[0]);
            Assert.That(list.Count, Is.EqualTo(5));

            Assert.That(((Hashtable)list[0])[BaseTest.ConvertKey("Id")], Is.EqualTo(1));
            Assert.That(((Hashtable)list[1])[BaseTest.ConvertKey("Id")], Is.EqualTo(2));
            Assert.That(((Hashtable)list[2])[BaseTest.ConvertKey("Id")], Is.EqualTo(3));
            Assert.That(((Hashtable)list[3])[BaseTest.ConvertKey("Id")], Is.EqualTo(4));
            Assert.That(((Hashtable)list[4])[BaseTest.ConvertKey("Id")], Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with IList ResultClass
        /// </summary>
        [Test]
        public void TestQueryForListWithIListResultClass()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsAsArrayListViaResultClass", null);

            IList listAccount = (IList)list[0];
            Assert.That(listAccount[0], Is.EqualTo(1));
            Assert.That(listAccount[1], Is.EqualTo("Joe"));
            Assert.That(listAccount[2], Is.EqualTo("Dalton"));
            Assert.That(listAccount[3], Is.EqualTo("Joe.Dalton@somewhere.com"));

            Assert.That(list.Count, Is.EqualTo(5));

            listAccount = (IList)list[0];
            Assert.That(listAccount[0], Is.EqualTo(1));
            listAccount = (IList)list[1];
            Assert.That(listAccount[0], Is.EqualTo(2));
            listAccount = (IList)list[2];
            Assert.That(listAccount[0], Is.EqualTo(3));
            listAccount = (IList)list[3];
            Assert.That(listAccount[0], Is.EqualTo(4));
            listAccount = (IList)list[4];
            Assert.That(listAccount[0], Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList With ResultMap, result collection as ArrayList
        /// </summary>
        [Test]
        public void TestQueryForListWithResultMap()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaResultMap", null);

            AssertAccount1((Account)list[0]);
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(((Account)list[0]).Id, Is.EqualTo(1));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(3));
            Assert.That(((Account)list[3]).Id, Is.EqualTo(4));
            Assert.That(((Account)list[4]).Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with ResultObject : 
        /// AccountCollection strongly typed collection
        /// </summary>
        [Test]
        public void TestQueryForListWithResultObject()
        {
            AccountCollection accounts = new AccountCollection();

            sqlMap.QueryForList("GetAllAccountsViaResultMap", null, accounts);

            AssertAccount1(accounts[0]);
            Assert.That(accounts.Count, Is.EqualTo(5));
            Assert.That(accounts[0].Id, Is.EqualTo(1));
            Assert.That(accounts[1].Id, Is.EqualTo(2));
            Assert.That(accounts[2].Id, Is.EqualTo(3));
            Assert.That(accounts[3].Id, Is.EqualTo(4));
            Assert.That(accounts[4].Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with ListClass : LineItemCollection
        /// </summary>
        [Test]
        public void TestQueryForListWithListClass()
        {
            LineItemCollection linesItem = sqlMap.QueryForList("GetLineItemsForOrderWithListClass", 6) as LineItemCollection;

            Assert.That(linesItem, Is.Not.Null);
            Assert.That(linesItem.Count, Is.EqualTo(2));
            Assert.That(linesItem[0].Code, Is.EqualTo("ASM-45"));
            Assert.That(linesItem[1].Code, Is.EqualTo("QSM-39"));
        }

        /// <summary>
        /// Test QueryForList with no result.
        /// </summary>
        [Test]
        public void TestQueryForListWithNoResult()
        {
            IList list = sqlMap.QueryForList("GetNoAccountsViaResultMap", null);

            Assert.That(list.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test QueryForList with ResultClass : Account.
        /// </summary>
        [Test]
        public void TestQueryForListResultClass()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaResultClass", null);

            AssertAccount1((Account)list[0]);
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(((Account)list[0]).Id, Is.EqualTo(1));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(3));
            Assert.That(((Account)list[3]).Id, Is.EqualTo(4));
            Assert.That(((Account)list[4]).Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with simple resultClass : string
        /// </summary>
        [Test]
        public void TestQueryForListWithSimpleResultClass()
        {
            IList list = sqlMap.QueryForList("GetAllEmailAddressesViaResultClass", null);

            Assert.That((string)list[0], Is.EqualTo("Joe.Dalton@somewhere.com"));
            Assert.That((string)list[1], Is.EqualTo("Averel.Dalton@somewhere.com"));
            Assert.That(list[2], Is.Null);
            Assert.That((string)list[3], Is.EqualTo("Jack.Dalton@somewhere.com"));
            Assert.That((string)list[4], Is.EqualTo("gilles.bayon@nospam.org"));
        }

        /// <summary>
        /// Test  QueryForList with simple ResultMap : string
        /// </summary>
        [Test]
        public void TestQueryForListWithSimpleResultMap()
        {
            IList list = sqlMap.QueryForList("GetAllEmailAddressesViaResultMap", null);

            Assert.That((string)list[0], Is.EqualTo("Joe.Dalton@somewhere.com"));
            Assert.That((string)list[1], Is.EqualTo("Averel.Dalton@somewhere.com"));
            Assert.That(list[2], Is.Null);
            Assert.That((string)list[3], Is.EqualTo("Jack.Dalton@somewhere.com"));
            Assert.That((string)list[4], Is.EqualTo("gilles.bayon@nospam.org"));
        }

        /// <summary>
        /// Test QueryForListWithSkipAndMax
        /// </summary>
        [Test]
        public void TestQueryForListWithSkipAndMax()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaResultMap", null, 2, 2);

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(((Account)list[0]).Id, Is.EqualTo(3));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(4));
        }


        [Test]
        public void TestQueryWithRowDelegate()
        {
            RowDelegate handler = new RowDelegate(this.RowHandler);

            IList list = sqlMap.QueryWithRowDelegate("GetAllAccountsViaResultMap", null, handler);

            Assert.That(_index, Is.EqualTo(5));
            Assert.That(list.Count, Is.EqualTo(5));
            AssertAccount1((Account)list[0]);
            Assert.That(((Account)list[0]).Id, Is.EqualTo(1));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(3));
            Assert.That(((Account)list[3]).Id, Is.EqualTo(4));
            Assert.That(((Account)list[4]).Id, Is.EqualTo(5));

        }

        #endregion

        #region  Map Tests

        /// <summary>
        /// Test ExecuteQueryForMap : Hashtable.
        /// </summary>
        [Test]
        public void TestExecuteQueryForMap()
        {
            IDictionary map = sqlMap.QueryForMap("GetAllAccountsViaResultClass", null, "FirstName");

            Assert.That(map.Count, Is.EqualTo(5));
            AssertAccount1(((Account)map["Joe"]));

            Assert.That(((Account)map["Joe"]).Id, Is.EqualTo(1));
            Assert.That(((Account)map["Averel"]).Id, Is.EqualTo(2));
            Assert.That(((Account)map["William"]).Id, Is.EqualTo(3));
            Assert.That(((Account)map["Jack"]).Id, Is.EqualTo(4));
            Assert.That(((Account)map["Gilles"]).Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test ExecuteQueryForMap : Hashtable.
        /// </summary>
        /// <remarks>
        /// If the keyProperty is an integer, you must acces the map
        /// by map[integer] and not by map["integer"]
        /// </remarks>
        [Test]
        public void TestExecuteQueryForMap2()
        {
            IDictionary map = sqlMap.QueryForMap("GetAllOrderWithLineItems", null, "PostalCode");

            Assert.That(map.Count, Is.EqualTo(11));
            Order order = ((Order)map["T4H 9G4"]);

            Assert.That(order.LineItemsIList.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test ExecuteQueryForMap with value property :
        /// "FirstName" as key, "EmailAddress" as value
        /// </summary>
        [Test]
        public void TestExecuteQueryForMapWithValueProperty()
        {
            IDictionary map = sqlMap.QueryForMap("GetAllAccountsViaResultClass", null, "FirstName", "EmailAddress");

            Assert.That(map.Count, Is.EqualTo(5));

            Assert.That(map["Joe"], Is.EqualTo("Joe.Dalton@somewhere.com"));
            Assert.That(map["Averel"], Is.EqualTo("Averel.Dalton@somewhere.com"));
            Assert.That(map["William"], Is.Null);
            Assert.That(map["Jack"], Is.EqualTo("Jack.Dalton@somewhere.com"));
            Assert.That(map["Gilles"], Is.EqualTo("gilles.bayon@nospam.org"));
        }

        /// <summary>
        /// Test ExecuteQueryForWithJoined
        /// </remarks>
        [Test]
        public void TestExecuteQueryForWithJoined()
        {
            Order order = sqlMap.QueryForObject("GetOrderJoinWithAccount", 10) as Order;

            Assert.That(order.Account, Is.Not.Null);

            order = sqlMap.QueryForObject("GetOrderJoinWithAccount", 11) as Order;

            Assert.That(order.Account, Is.Null);
        }

        /// <summary>
        ///  Better support for nested result maps when using dictionary
        /// </remarks>
        [Test]
        [Category("JIRA-254")]
        public void Better_Support_For_Nested_Result_Maps_When_Using_Dictionary()
        {
            IDictionary order = (IDictionary)sqlMap.QueryForObject("JIRA-254", 10);

            Assert.That(order["Account"], Is.Not.Null);

            order = (IDictionary)sqlMap.QueryForObject("JIRA-254", 11);

            Assert.That(order["Account"], Is.Null);
        }

        /// <summary>
        /// Test ExecuteQueryFor With Complex Joined
        /// </summary>
        /// <remarks>
        /// A->B->C
        ///  ->E
        ///  ->F
        /// </remarks>
        [Test]
        public void TestExecuteQueryForWithComplexJoined()
        {
            A a = sqlMap.QueryForObject("SelectComplexJoined", null) as A;

            Assert.That(a, Is.Not.Null);
            Assert.That(a.B, Is.Not.Null);
            Assert.That(a.B.C, Is.Not.Null);
            Assert.That(a.B.D, Is.Null);
            Assert.That(a.E, Is.Not.Null);
            Assert.That(a.F, Is.Null);
        }
        #endregion

        #region Extends statement

        /// <summary>
        /// Test base Extends statement
        /// </summary>
        [Test]
        public void TestExtendsGetAllAccounts()
        {
            IList list = sqlMap.QueryForList("GetAllAccounts", null);

            AssertAccount1((Account)list[0]);
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(((Account)list[0]).Id, Is.EqualTo(1));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(3));
            Assert.That(((Account)list[3]).Id, Is.EqualTo(4));
            Assert.That(((Account)list[4]).Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test Extends statement GetAllAccountsOrderByName extends GetAllAccounts
        /// </summary>
        [Test]
        public void TestExtendsGetAllAccountsOrderByName()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsOrderByName", null);

            AssertAccount1((Account)list[3]);
            Assert.That(list.Count, Is.EqualTo(5));

            Assert.That(((Account)list[0]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(5));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(4));
            Assert.That(((Account)list[3]).Id, Is.EqualTo(1));
            Assert.That(((Account)list[4]).Id, Is.EqualTo(3));
        }

        /// <summary>
        /// Test Extends statement GetOneAccount extends GetAllAccounts
        /// </summary>
        [Test]
        public void TestExtendsGetOneAccount()
        {
            Account account = sqlMap.QueryForObject("GetOneAccount", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test Extends statement GetSomeAccount extends GetAllAccounts
        /// </summary>
        [Test]
        public void TestExtendsGetSomeAccount()
        {
            Hashtable param = new Hashtable();
            param.Add("lowID", 2);
            param.Add("hightID", 4);

            IList list = sqlMap.QueryForList("GetSomeAccount", param);

            Assert.That(list.Count, Is.EqualTo(3));

            Assert.That(((Account)list[0]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(3));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(4));
        }

        [Test]
        public void TestDummy()
        {
            Hashtable param = new Hashtable();
            param.Add("?lowID", 2);
            param.Add("?hightID", 4);

            IList list = sqlMap.QueryForList("GetDummy", param);

            Assert.That(list.Count, Is.EqualTo(3));

            Assert.That(((Account)list[0]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(3));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(4));
        }

        #endregion

        #region Update tests

        /// <summary>
        /// Test Insert with post GeneratedKey
        /// </summary>
        [Test]
        public void TestInsertPostKey()
        {
            LineItem item = new LineItem();

            item.Id = 350;
            item.Code = "blah";
            item.Order = new Order();
            item.Order.Id = 9;
            item.Price = 44.00m;
            item.Quantity = 1;

            object key = sqlMap.Insert("InsertLineItemPostKey", item);

            Assert.That(key, Is.EqualTo(99));
            Assert.That(item.Id, Is.EqualTo(99));

            Hashtable param = new Hashtable();
            param.Add("Order_ID", 9);
            param.Add("LineItem_ID", 350);
            LineItem testItem = (LineItem)sqlMap.QueryForObject("GetSpecificLineItem", param);
            Assert.That(testItem, Is.Not.Null);
            Assert.That(testItem.Id, Is.EqualTo(350));
        }

        /// <summary>
        /// Test Insert pre GeneratedKey
        /// </summary>
        [Test]
        public void TestInsertPreKey()
        {
            LineItem item = new LineItem();

            item.Id = 10;
            item.Code = "blah";
            item.Order = new Order();
            item.Order.Id = 9;
            item.Price = 44.00m;
            item.Quantity = 1;

            object key = sqlMap.Insert("InsertLineItemPreKey", item);

            Assert.That(key, Is.EqualTo(99));
            Assert.That(item.Id, Is.EqualTo(99));

            Hashtable param = new Hashtable();
            param.Add("Order_ID", 9);
            param.Add("LineItem_ID", 99);

            LineItem testItem = (LineItem)sqlMap.QueryForObject("GetSpecificLineItem", param);

            Assert.That(testItem, Is.Not.Null);
            Assert.That(testItem.Id, Is.EqualTo(99));
        }

        /// <summary>
        /// Test Test Insert No Key
        /// </summary>
        [Test]
        public void TestInsertNoKey()
        {
            LineItem item = new LineItem();

            item.Id = 100;
            item.Code = "blah";
            item.Order = new Order();
            item.Order.Id = 9;
            item.Price = 44.00m;
            item.Quantity = 1;

            object key = sqlMap.Insert("InsertLineItemNoKey", item);

            Assert.That(key, Is.Null);
            Assert.That(item.Id, Is.EqualTo(100));

            Hashtable param = new Hashtable();
            param.Add("Order_ID", 9);
            param.Add("LineItem_ID", 100);

            LineItem testItem = (LineItem)sqlMap.QueryForObject("GetSpecificLineItem", param);

            Assert.That(testItem, Is.Not.Null);
            Assert.That(testItem.Id, Is.EqualTo(100));
        }

        /// <summary>
        /// Test Insert account via inline parameters
        /// </summary>
        [Test]
        public void TestInsertAccountViaInlineParameters()
        {
            Account account = new Account();

            account.Id = 10;
            account.FirstName = "Luky";
            account.LastName = "Luke";
            account.EmailAddress = "luly.luke@somewhere.com";

            sqlMap.Insert("InsertAccountViaInlineParameters", account);

            Account testAccount = sqlMap.QueryForObject("GetAccountViaColumnIndex", 10) as Account;

            Assert.That(testAccount, Is.Not.Null);
            Assert.That(testAccount.Id, Is.EqualTo(10));
        }

        /// <summary>
        /// Test Insert account via parameterMap
        /// </summary>
        [Test]
        public void TestInsertAccountViaParameterMap()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            account = sqlMap.QueryForObject("GetAccountNullableEmail", 6) as Account;

            AssertAccount6(account);
        }

        /// <summary>
        /// Test Insert account via parameterMap
        /// </summary>
        [Test]
        public void TestInsertEnumViaParameterMap()
        {
            Enumeration enumClass = new Enumeration();
            enumClass.Id = 99;
            enumClass.Day = Days.Thu;
            enumClass.Color = Colors.Blue;
            enumClass.Month = Months.May;

            sqlMap.Insert("InsertEnumViaParameterMap", enumClass);

            enumClass = null;
            enumClass = sqlMap.QueryForObject("GetEnumeration", 99) as Enumeration;

            Assert.That(enumClass.Day, Is.EqualTo(Days.Thu));
            Assert.That(enumClass.Color, Is.EqualTo(Colors.Blue));
            Assert.That(enumClass.Month, Is.EqualTo(Months.May));
        }

        /// <summary>
        /// Test Update via parameterMap
        /// </summary>
        [Test]
        public void TestUpdateViaParameterMap()
        {
            Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

            account.EmailAddress = "new@somewhere.com";
            sqlMap.Update("UpdateAccountViaParameterMap", account);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

            Assert.That(account.EmailAddress, Is.EqualTo("new@somewhere.com"));
        }

        /// <summary>
        /// Test Update via parameterMap V2
        /// </summary>
        [Test]
        public void TestUpdateViaParameterMap2()
        {
            Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

            account.EmailAddress = "new@somewhere.com";
            sqlMap.Update("UpdateAccountViaParameterMap2", account);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

            Assert.That(account.EmailAddress, Is.EqualTo("new@somewhere.com"));
        }

        /// <summary>
        /// Test Update with inline parameters
        /// </summary>
        [Test]
        public void TestUpdateWithInlineParameters()
        {
            Account account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

            account.EmailAddress = "new@somewhere.com";
            sqlMap.Update("UpdateAccountViaInlineParameters", account);

            account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

            Assert.That(account.EmailAddress, Is.EqualTo("new@somewhere.com"));
        }

        /// <summary>
        /// Test Execute Update With Parameter Class
        /// </summary>
        [Test]
        public void TestExecuteUpdateWithParameterClass()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            bool checkForInvalidTypeFailedAppropriately = false;

            try
            {
                sqlMap.Update("DeleteAccount", new object());
            }
            catch (IBatisNetException e)
            {
                Console.WriteLine("TestExecuteUpdateWithParameterClass :" + e.Message);
                checkForInvalidTypeFailedAppropriately = true;
            }

            sqlMap.Update("DeleteAccount", account);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 6) as Account;

            Assert.That(account, Is.Null);
            Assert.That(checkForInvalidTypeFailedAppropriately, Is.True);
        }

        /// <summary>
        /// Test Execute Delete
        /// </summary>
        [Test]
        public void TestExecuteDelete()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            account = null;
            account = sqlMap.QueryForObject("GetAccountViaColumnName", 6) as Account;

            Assert.That(account.Id, Is.EqualTo(6));

            int rowNumber = sqlMap.Delete("DeleteAccount", account);
            Assert.That(rowNumber, Is.EqualTo(1));

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 6) as Account;

            Assert.That(account, Is.Null);
        }

        /// <summary>
        /// Test Execute Delete
        /// </summary>
        [Test]
        public void TestDeleteWithComments()
        {
            int rowNumber = sqlMap.Delete("DeleteWithComments", null);

            Assert.That(rowNumber, Is.EqualTo(3));
        }

        /// <summary>
        /// Test Execute delete Via Inline Parameters
        /// </summary>
        [Test]
        public void TestDeleteViaInlineParameters()
        {
            Account account = NewAccount6();

            sqlMap.Insert("InsertAccountViaParameterMap", account);

            int rowNumber = sqlMap.Delete("DeleteAccountViaInlineParameters", 6);

            Assert.That(rowNumber, Is.EqualTo(1));
        }

        #endregion

        #region Row delegate

        private int _index = 0;

        public void RowHandler(object obj, object paramterObject, IList list)
        {
            _index++;
            Assert.That(((Account)obj).Id, Is.EqualTo(_index));
            list.Add(obj);
        }

        #endregion

        #region Tests using syntax

        /// <summary>
        /// Test Test Using syntax on sqlMap.OpenConnection
        /// </summary>
        [Test]
        public void TestUsingConnection()
        {
            using (IDalSession session = sqlMap.OpenConnection())
            {
                Account account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;
                AssertAccount1(account);
            } // compiler will call Dispose on SqlMapSession
        }

        /// <summary>
        /// Test Using syntax on sqlMap.BeginTransaction
        /// </summary>
        [Test]
        public void TestUsingTransaction()
        {
            using (IDalSession session = sqlMap.BeginTransaction())
            {
                Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

                account.EmailAddress = "new@somewhere.com";
                sqlMap.Update("UpdateAccountViaParameterMap", account);

                account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

                Assert.That(account.EmailAddress, Is.EqualTo("new@somewhere.com"));

                session.Complete(); // Commit
            } // compiler will call Dispose on SqlMapSession
        }

        /// <summary>
        /// Test Using syntax on sqlMap.BeginTransaction
        /// </summary>
        [Test]
        public void TestUsing()
        {
            sqlMap.OpenConnection();
            sqlMap.BeginTransaction(false);
            Account account = (Account)sqlMap.QueryForObject("GetAccountViaColumnName", 1);

            account.EmailAddress = "new@somewhere.com";
            sqlMap.Update("UpdateAccountViaParameterMap", account);

            account = sqlMap.QueryForObject("GetAccountViaColumnName", 1) as Account;

            Assert.That(account.EmailAddress, Is.EqualTo("new@somewhere.com"));

            sqlMap.CommitTransaction(false);
            sqlMap.CloseConnection();
        }

        #endregion

        #region JIRA Tests

        /// <summary>
        /// Test a constructor argument with select tag.
        /// </remarks>
        [Test]
        public void TestJIRA182()
        {
            Order order = sqlMap.QueryForObject("JIRA182", 5) as Order;

            Assert.That(order.Id, Is.EqualTo(5));
            Assert.That(order.Account, Is.Not.Null);
            Assert.That(order.Account.Id, Is.EqualTo(5));
        }

        /// <summary>
        /// QueryForDictionary does not process select property
        /// </summary>
        [Test]
        public void TestJIRA220()
        {
            IDictionary map = sqlMap.QueryForMap("JIAR220", null, "PostalCode");

            Assert.That(map.Count, Is.EqualTo(11));
            Order order = ((Order)map["T4H 9G4"]);

            Assert.That(order.LineItemsIList.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test JIRA 30 (repeating property)
        /// </summary>
        [Test]
        public void TestJIRA30()
        {
            Account account = new Account();
            account.Id = 1;
            account.FirstName = "Joe";
            account.LastName = "Dalton";
            account.EmailAddress = "Joe.Dalton@somewhere.com";

            Account result = sqlMap.QueryForObject("GetAccountWithRepeatingProperty", account) as Account;

            AssertAccount1(result);
        }

        /// <summary>
        /// Test Bit column 
        /// </summary>
        [Test]
        public void TestJIRA42()
        {
            Other other = new Other();

            other.Int = 100;
            other.Bool = true;
            other.Long = 789456321;

            sqlMap.Insert("InsertBool", other);
        }

        /// <summary>
        /// Test for access a result map in a different namespace 
        /// </summary>
        [Test]
        public void TestJIRA45()
        {
            Account account = sqlMap.QueryForObject("GetAccountJIRA45", 1) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test : Whitespace is not maintained properly when CDATA tags are used
        /// </summary>
        [Test]
        public void TestJIRA110()
        {
            Account account = sqlMap.QueryForObject("Get1Account", null) as Account;
            AssertAccount1(account);
        }

        /// <summary>
        /// Test : Whitespace is not maintained properly when CDATA tags are used
        /// </summary>
        [Test]
        public void TestJIRA110Bis()
        {
            IList list = sqlMap.QueryForList("GetAccounts", null);

            AssertAccount1((Account)list[0]);
            Assert.That(list.Count, Is.EqualTo(5));
        }

        #endregion

        #region CustomTypeHandler tests

        /// <summary>
        /// Test CustomTypeHandler 
        /// </summary>
        [Test]
        public void TestExecuteQueryWithCustomTypeHandler()
        {
            IList list = sqlMap.QueryForList("GetAllAccountsViaCustomTypeHandler", null);

            AssertAccount1((Account)list[0]);
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(((Account)list[0]).Id, Is.EqualTo(1));
            Assert.That(((Account)list[1]).Id, Is.EqualTo(2));
            Assert.That(((Account)list[2]).Id, Is.EqualTo(3));
            Assert.That(((Account)list[3]).Id, Is.EqualTo(4));
            Assert.That(((Account)list[4]).Id, Is.EqualTo(5));

            Assert.That(((Account)list[0]).CartOption, Is.False);
            Assert.That(((Account)list[1]).CartOption, Is.False);
            Assert.That(((Account)list[2]).CartOption, Is.True);
            Assert.That(((Account)list[3]).CartOption, Is.True);
            Assert.That(((Account)list[4]).CartOption, Is.True);

            Assert.That(((Account)list[0]).BannerOption, Is.True);
            Assert.That(((Account)list[1]).BannerOption, Is.True);
            Assert.That(((Account)list[2]).BannerOption, Is.False);
            Assert.That(((Account)list[3]).BannerOption, Is.False);
            Assert.That(((Account)list[4]).BannerOption, Is.True);
        }

        /// <summary>
        /// Test CustomTypeHandler Oui/Non
        /// </summary>
        [Test]
        public void TestCustomTypeHandler()
        {
            Other other = new Other();
            other.Int = 99;
            other.Long = 1966;
            other.Bool = true;
            other.Bool2 = false;

            sqlMap.Insert("InsertCustomTypeHandler", other);

            Other anOther = sqlMap.QueryForObject("SelectByInt", 99) as Other;

            Assert.That(anOther, Is.Not.Null);
            Assert.That(anOther.Int, Is.EqualTo(99));
            Assert.That(anOther.Long, Is.EqualTo(1966));
            Assert.That(anOther.Bool, Is.EqualTo(true));
            Assert.That(anOther.Bool2, Is.EqualTo(false));
        }

        /// <summary>
        /// Test CustomTypeHandler Oui/Non
        /// </summary>
        [Test]
        public void TestInsertInlineCustomTypeHandlerV1()
        {
            Other other = new Other();
            other.Int = 99;
            other.Long = 1966;
            other.Bool = true;
            other.Bool2 = false;

            sqlMap.Insert("InsertInlineCustomTypeHandlerV1", other);

            Other anOther = sqlMap.QueryForObject("SelectByInt", 99) as Other;

            Assert.That(anOther, Is.Not.Null);
            Assert.That(anOther.Int, Is.EqualTo(99));
            Assert.That(anOther.Long, Is.EqualTo(1966));
            Assert.That(anOther.Bool, Is.EqualTo(true));
            Assert.That(anOther.Bool2, Is.EqualTo(false));
        }

        /// <summary>
        /// Test CustomTypeHandler Oui/Non
        /// </summary>
        [Test]
        public void TestInsertInlineCustomTypeHandlerV2()
        {
            Other other = new Other();
            other.Int = 99;
            other.Long = 1966;
            other.Bool = true;
            other.Bool2 = false;

            sqlMap.Insert("InsertInlineCustomTypeHandlerV2", other);

            Other anOther = sqlMap.QueryForObject("SelectByInt", 99) as Other;

            Assert.That(anOther, Is.Not.Null);
            Assert.That(anOther.Int, Is.EqualTo(99));
            Assert.That(anOther.Long, Is.EqualTo(1966));
            Assert.That(anOther.Bool, Is.EqualTo(true));
            Assert.That(anOther.Bool2, Is.EqualTo(false));
        }
        #endregion
    }
}
