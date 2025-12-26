#region Apache Notice
/*****************************************************************************
 * $Revision: 470514 $
 * $LastChangedDate: 2006-11-02 21:46:13 +0100 (jeu., 02 nov. 2006) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2005, 2006 The Apache Software Foundation
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion


using System;
using System.Collections;
using SqlBatis.DataMapper;
using SqlBatis.DataMapper.Exceptions;
using SqlBatis.DataMapper.Utilities;
using SqlBatis.DataMapper.MappedStatements;
using SqlBatis.DataMapper.Test.Domain;
using System.Collections.Generic;
using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests.Generics
{
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
        /// Test Open connection with a connection string
        /// </summary>
        [Test]
        public void TestOpenConnection()
        {
            sqlMap.OpenConnection(sqlMap.DataSource.ConnectionString);
            Account account = sqlMap.QueryForObject<Account>("SelectWithProperty", null);
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
            Account account = sqlMap.QueryForObject<Account>("SelectWithProperty", null);
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ColumnName
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaColumnName()
        {
            Account account = sqlMap.QueryForObject<Account>("GetAccountViaColumnName", 1);
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ColumnIndex
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaColumnIndex()
        {
            Account account = sqlMap.QueryForObject<Account>("GetAccountViaColumnIndex", 1);
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject Via ResultClass
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectViaResultClass()
        {
            Account account = sqlMap.QueryForObject<Account>("GetAccountViaResultClass", 1);
            AssertAccount1(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject With simple ResultClass : string
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithSimpleResultClass()
        {
            string email = sqlMap.QueryForObject<string>("GetEmailAddressViaResultClass", 1);
            Assert.That(email, Is.EqualTo("Joe.Dalton@somewhere.com"));
        }

        /// <summary>
        /// Test ExecuteQueryForObject With simple ResultMap : string
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithSimpleResultMap()
        {
            string email = sqlMap.QueryForObject<string>("GetEmailAddressViaResultMap", 1);
            Assert.That(email, Is.EqualTo("Joe.Dalton@somewhere.com"));
        }

        /// <summary>
        /// Test Primitive ReturnValue : System.DateTime
        /// </summary>
        [Test]
        public void TestPrimitiveReturnValue()
        {
            DateTime CardExpiry = sqlMap.QueryForObject<DateTime>("GetOrderCardExpiryViaResultClass", 1);
            Assert.That(CardExpiry, Is.EqualTo(new DateTime(2003, 02, 15, 8, 15, 00)));
        }

        /// <summary>
        /// Test ExecuteQueryForObject with result object : Account
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithResultObject()
        {
            Account account = new Account();
            Account testAccount = sqlMap.QueryForObject<Account>("GetAccountViaColumnName", 1, account);
            AssertAccount1(account);
            Assert.That(account == testAccount, Is.True);
        }

        /// <summary>
        /// Test ExecuteQueryForObject as Hashtable
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectAsHashtable()
        {
            Hashtable account = sqlMap.QueryForObject<Hashtable>("GetAccountAsHashtable", 1);
            AssertAccount1AsHashtable(account);
        }

        /// <summary>
        /// Test ExecuteQueryForObject as Hashtable ResultClass
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectAsHashtableResultClass()
        {
            Hashtable account = sqlMap.QueryForObject<Hashtable>("GetAccountAsHashtableResultClass", 1);
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

            LineItem testItem = sqlMap.QueryForObject<LineItem>("GetSpecificLineItem", param);

            Assert.That(testItem, Is.Not.Null);
            Assert.That(testItem.Code, Is.EqualTo("TSM-12"));
        }

        /// <summary>
        /// Test Query Dynamic Sql Element
        /// </summary>
        [Test]
        public void TestQueryDynamicSqlElement()
        {
            //IList list = sqlMap.QueryForList("GetDynamicOrderedEmailAddressesViaResultMap", "Account_ID");
            IList<string> list = sqlMap.QueryForList<string>("GetDynamicOrderedEmailAddressesViaResultMap", "Account_ID");

            Assert.That(list[0], Is.EqualTo("Joe.Dalton@somewhere.com"));

            //list = sqlMap.QueryForList("GetDynamicOrderedEmailAddressesViaResultMap", "Account_FirstName");
            list = sqlMap.QueryForList<string>("GetDynamicOrderedEmailAddressesViaResultMap", "Account_FirstName");

            Assert.That(list[0], Is.EqualTo("Averel.Dalton@somewhere.com"));

        }

        /// <summary>
        /// Test Execute QueryForList With ResultMap With Dynamic Element
        /// </summary>
        [Test]
        public void TestExecuteQueryForListWithResultMapWithDynamicElement()
        {
            IList<Account> list = sqlMap.QueryForList<Account>("GetAllAccountsViaResultMapWithDynamicElement", "LIKE");

            AssertAccount1(list[0]);
            Assert.That(list.Count, Is.EqualTo(4));
            Assert.That(list[0].Id, Is.EqualTo(1));
            Assert.That(list[1].Id, Is.EqualTo(2));
            Assert.That(list[2].Id, Is.EqualTo(4));

            list = sqlMap.QueryForList<Account>("GetAllAccountsViaResultMapWithDynamicElement", "=");

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

            Account testAccount = sqlMap.QueryForObject<Account>("GetAccountViaInlineParameters", account);

            AssertAccount1(testAccount);
        }

        /// <summary>
        /// Test ExecuteQuery For Object With Enum property
        /// </summary>
        [Test]
        public void TestExecuteQueryForObjectWithEnum()
        {
            Enumeration enumClass = sqlMap.QueryForObject<Enumeration>("GetEnumeration", 1);

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
        public void TestQueryForListWithGeneric()
        {
            List<Account> accounts = new List<Account>();

            sqlMap.QueryForList("GetAllAccountsViaResultMap", null, (System.Collections.IList)accounts);

            AssertAccount1(accounts[0]);
            Assert.That(accounts.Count, Is.EqualTo(5));
            Assert.That(accounts[0].Id, Is.EqualTo(1));
            Assert.That(accounts[1].Id, Is.EqualTo(2));
            Assert.That(accounts[2].Id, Is.EqualTo(3));
            Assert.That(accounts[3].Id, Is.EqualTo(4));
            Assert.That(accounts[4].Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with Hashtable ResultMap
        /// </summary>
        [Test]
        public void TestQueryForListWithHashtableResultMap()
        {
            IList<Hashtable> list = sqlMap.QueryForList<Hashtable>("GetAllAccountsAsHashMapViaResultMap", null);

            AssertAccount1AsHashtable(list[0]);
            Assert.That(list.Count, Is.EqualTo(5));

            Assert.That(list[0]["Id"], Is.EqualTo(1));
            Assert.That(list[1]["Id"], Is.EqualTo(2));
            Assert.That(list[2]["Id"], Is.EqualTo(3));
            Assert.That(list[3]["Id"], Is.EqualTo(4));
            Assert.That(list[4]["Id"], Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with Hashtable ResultClass
        /// </summary>
        [Test]
        public void TestQueryForListWithHashtableResultClass()
        {
            IList<Hashtable> list = sqlMap.QueryForList<Hashtable>("GetAllAccountsAsHashtableViaResultClass", null);

            AssertAccount1AsHashtableForResultClass(list[0]);
            Assert.That(list.Count, Is.EqualTo(5));

            Assert.That(list[0][BaseTest.ConvertKey("Id")], Is.EqualTo(1));
            Assert.That(list[1][BaseTest.ConvertKey("Id")], Is.EqualTo(2));
            Assert.That(list[2][BaseTest.ConvertKey("Id")], Is.EqualTo(3));
            Assert.That(list[3][BaseTest.ConvertKey("Id")], Is.EqualTo(4));
            Assert.That(list[4][BaseTest.ConvertKey("Id")], Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with IList ResultClass
        /// </summary>
        [Test]
        public void TestQueryForListWithIListResultClass()
        {
            IList<IList> list = sqlMap.QueryForList<IList>("GetAllAccountsAsArrayListViaResultClass", null);

            IList listAccount = list[0];
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
            IList<Account> list = sqlMap.QueryForList<Account>("GetAllAccountsViaResultMap", null);

            AssertAccount1(list[0]);
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0].Id, Is.EqualTo(1));
            Assert.That(list[1].Id, Is.EqualTo(2));
            Assert.That(list[2].Id, Is.EqualTo(3));
            Assert.That(list[3].Id, Is.EqualTo(4));
            Assert.That(list[4].Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with ResultObject : 
        /// AccountCollection strongly typed collection
        /// </summary>
        [Test]
        public void TestQueryForListWithResultObject()
        {
            IList<Account> accounts = new List<Account>();

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
        /// Test QueryForList with no result.
        /// </summary>
        [Test]
        public void TestQueryForListWithNoResult()
        {
            IList<Account> list = sqlMap.QueryForList<Account>("GetNoAccountsViaResultMap", null);

            Assert.That(list.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test QueryForList with ResultClass : Account.
        /// </summary>
        [Test]
        public void TestQueryForListResultClass()
        {
            IList<Account> list = sqlMap.QueryForList<Account>("GetAllAccountsViaResultClass", null);

            AssertAccount1(list[0]);
            Assert.That(list.Count, Is.EqualTo(5));
            Assert.That(list[0].Id, Is.EqualTo(1));
            Assert.That(list[1].Id, Is.EqualTo(2));
            Assert.That(list[2].Id, Is.EqualTo(3));
            Assert.That(list[3].Id, Is.EqualTo(4));
            Assert.That(list[4].Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test QueryForList with simple resultClass : string
        /// </summary>
        [Test]
        public void TestQueryForListWithSimpleResultClass()
        {
            IList<string> list = sqlMap.QueryForList<string>("GetAllEmailAddressesViaResultClass", null);

            Assert.That(list[0], Is.EqualTo("Joe.Dalton@somewhere.com"));
            Assert.That(list[1], Is.EqualTo("Averel.Dalton@somewhere.com"));
            Assert.That(list[2], Is.Null);
            Assert.That(list[3], Is.EqualTo("Jack.Dalton@somewhere.com"));
            Assert.That(list[4], Is.EqualTo("gilles.bayon@nospam.org"));
        }

        /// <summary>
        /// Test  QueryForList with simple ResultMap : string
        /// </summary>
        [Test]
        public void TestQueryForListWithSimpleResultMap()
        {
            IList<string> list = sqlMap.QueryForList<string>("GetAllEmailAddressesViaResultMap", null);

            Assert.That(list[0], Is.EqualTo("Joe.Dalton@somewhere.com"));
            Assert.That(list[1], Is.EqualTo("Averel.Dalton@somewhere.com"));
            Assert.That(list[2], Is.Null);
            Assert.That(list[3], Is.EqualTo("Jack.Dalton@somewhere.com"));
            Assert.That(list[4], Is.EqualTo("gilles.bayon@nospam.org"));
        }

        /// <summary>
        /// Test QueryForListWithSkipAndMax
        /// </summary>
        [Test]
        public void TestQueryForListWithSkipAndMax()
        {
            IList<Account> list = sqlMap.QueryForList<Account>("GetAllAccountsViaResultMap", null, 2, 2);

            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0].Id, Is.EqualTo(3));
            Assert.That(list[1].Id, Is.EqualTo(4));
        }


        [Test]
        public void TestQueryWithRowDelegate()
        {
            _index = 0;
            RowDelegate<Account> handler = new RowDelegate<Account>(this.RowHandler);

            IList<Account> list = sqlMap.QueryWithRowDelegate<Account>("GetAllAccountsViaResultMap", null, handler);

            Assert.That(_index, Is.EqualTo(5));
            Assert.That(list.Count, Is.EqualTo(5));
            AssertAccount1(list[0]);
            Assert.That(list[0].Id, Is.EqualTo(1));
            Assert.That(list[1].Id, Is.EqualTo(2));
            Assert.That(list[2].Id, Is.EqualTo(3));
            Assert.That(list[3].Id, Is.EqualTo(4));
            Assert.That(list[4].Id, Is.EqualTo(5));

        }

        /// <summary>
        /// Test  QueryForList with constructor use on result object
        /// </summary>
        [Test]
        public void TestJIRA172()
        {
            IList<Order> list = sqlMap.QueryForList<Order>("GetManyOrderWithConstructor", null);

            Assert.That(list.Count > 0, Is.True);
        }

        #endregion

        #region Row delegate

        private int _index = 0;

        public void RowHandler(object obj, object paramterObject, IList<Account> list)
        {
            _index++;

            Assert.That(((Account)obj).Id, Is.EqualTo(_index));
            list.Add(((Account)obj));
        }

        #endregion

        #region QueryForDictionary
        /// <summary>
        /// Test ExecuteQueryForDictionary 
        /// </summary>
        [Test]
        public void TestExecuteQueryForDictionary()
        {
            IDictionary<string, Account> map = sqlMap.QueryForDictionary<string, Account>("GetAllAccountsViaResultClass", null, "FirstName");

            Assert.That(map.Count, Is.EqualTo(5));
            AssertAccount1(map["Joe"]);

            Assert.That(map["Joe"].Id, Is.EqualTo(1));
            Assert.That(map["Averel"].Id, Is.EqualTo(2));
            Assert.That(map["William"].Id, Is.EqualTo(3));
            Assert.That(map["Jack"].Id, Is.EqualTo(4));
            Assert.That(map["Gilles"].Id, Is.EqualTo(5));
        }

        /// <summary>
        /// Test ExecuteQueryForMap : Hashtable.
        /// </summary>
        /// <remarks>
        /// If the keyProperty is an integer, you must acces the map
        /// by map[integer] and not by map["integer"]
        /// </remarks>
        [Test]
        public void TestExecuteQueryForDictionary2()
        {
            IDictionary<string, Order> map = sqlMap.QueryForDictionary<string, Order>("GetAllOrderWithLineItems", null, "PostalCode");

            Assert.That(map.Count, Is.EqualTo(11));
            Order order = map["T4H 9G4"];

            Assert.That(order.LineItemsIList.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test ExecuteQueryForMap with value property :
        /// "FirstName" as key, "EmailAddress" as value
        /// </summary>
        [Test]
        public void TestExecuteQueryForDictionaryWithValueProperty()
        {
            IDictionary<string, string> map = sqlMap.QueryForDictionary<string, string>("GetAllAccountsViaResultClass", null, "FirstName", "EmailAddress");

            Assert.That(map.Count, Is.EqualTo(5));

            Assert.That(map["Joe"], Is.EqualTo("Joe.Dalton@somewhere.com"));
            Assert.That(map["Averel"], Is.EqualTo("Averel.Dalton@somewhere.com"));
            Assert.That(map["William"], Is.Null);
            Assert.That(map["Jack"], Is.EqualTo("Jack.Dalton@somewhere.com"));
            Assert.That(map["Gilles"], Is.EqualTo("gilles.bayon@nospam.org"));
        }

        #endregion
    }
}
