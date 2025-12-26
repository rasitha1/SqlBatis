using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SqlBatis.DataMapper.Configuration;

using NUnit.Framework;

// DataSource definition
using SqlBatis.DataMapper.Utilities; // ScriptRunner definition
// SqlMap API
using SqlBatis.DataMapper.Test.Domain;
using System.Collections.Specialized;
using SqlBatis.DataMapper.Logging;
using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;


namespace SqlBatis.DataMapper.Test.NUnit.SqlMapTests
{
    public delegate string KeyConvert(string key);

    /// <summary>
    /// Summary description for BaseTest.
    /// </summary>
    [TestFixture]
    public abstract class BaseTest
    {
        /// <summary>
        /// The sqlMap
        /// </summary>
        protected static ISqlMapper sqlMap = null;
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static string ScriptDirectory { get; }

        protected static KeyConvert ConvertKey = null;

        static BaseTest()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(DefaultConfigurationStrings);
            Configuration = builder.Build();

            //LogManager.Adapter = new ConsoleOutLoggerFA(new NameValueCollection());

            ScriptDirectory = Path.Combine(Path.Combine(Path.Combine(Path.Combine(Path.Combine(TestContext.CurrentContext.TestDirectory, ".."), ".."), ".."), "Scripts"),
                                  Configuration["database"]) + Path.DirectorySeparatorChar;

        }

        protected void InitSqlMap()
        {
            //DateTime start = DateTime.Now;


            ConfigureHandler handler = new ConfigureHandler(Configure);
            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "SqlBatis.DataMapper.Test.Domain.LineItemCollection2, SqlBatis.DataMapper.Test");
            properties.Add("nullableInt", "int?");
            ChildSetupProperties(properties);
            builder.Properties = properties;

            sqlMap = builder.ConfigureAndWatch("sqlmap" + "_" + Configuration["database"] + "_"
                                               + Configuration["providerType"] + ".config", handler);

            //string loadTime = DateTime.Now.Subtract(start).ToString();
            //Console.WriteLine("Loading configuration time :"+loadTime);
        }

        static IReadOnlyDictionary<string, string> DefaultConfigurationStrings { get; } =
            new Dictionary<string, string>()
            {
                ["database"] = "MSSQL",
                ["providerType"] = "SqlClient"
            };

        protected static IConfiguration Configuration { get; }


        protected virtual void ChildSetupProperties(NameValueCollection nvc)
        {
            nvc["useStatementNamespaces"] = "false";
        }
        /// <summary>
        /// Initialize an sqlMap
        /// </summary>
        [OneTimeSetUp]
        protected virtual void SetUpFixture()
        {
            //DateTime start = DateTime.Now;

            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            NameValueCollection properties = new NameValueCollection();
            properties.Add("collection2Namespace", "SqlBatis.DataMapper.Test.Domain.LineItemCollection2, SqlBatis.DataMapper.Test");
            properties.Add("nullableInt", "int?");
            ChildSetupProperties(properties);
            builder.Properties = properties;

            string fileName = "sqlmap" + "_" + Configuration["database"] + "_" + Configuration["providerType"] + ".config";
            try
            {
                sqlMap = builder.Configure(fileName);
            }
            catch (Exception ex)

            {
                Exception e = ex;
                while (e != null)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace.ToString());
                    e = e.InnerException;

                }
                throw;
            }

            if (sqlMap.DataSource.DbProvider.Name.IndexOf("PostgreSql") >= 0)
            {
                BaseTest.ConvertKey = new KeyConvert(Lower);
            }
            else if (sqlMap.DataSource.DbProvider.Name.IndexOf("oracle") >= 0)
            {
                BaseTest.ConvertKey = new KeyConvert(Upper);
            }
            else
            {
                BaseTest.ConvertKey = new KeyConvert(Normal);
            }

            //			string loadTime = DateTime.Now.Subtract(start).ToString();
            //			Console.WriteLine("Loading configuration time :"+loadTime);
        }

        /// <summary>
        /// Dispose the SqlMap
        /// </summary>
        [OneTimeTearDown]
        protected virtual void TearDownFixture()
        {
            sqlMap = null;
        }

        protected static string Normal(string key)
        {
            return key;
        }

        protected static string Upper(string key)
        {
            return key.ToUpper();
        }

        protected static string Lower(string key)
        {
            return key.ToLower();
        }

        /// <summary>
        /// Configure the SqlMap
        /// </summary>
        /// <remarks>
        /// Must verify ConfigureHandler signature.
        /// </remarks>
        /// <param name="obj">
        /// The reconfigured sqlMap.
        /// </param>
        protected static void Configure(object obj)
        {
            sqlMap = null;//(SqlMapper) obj;
        }

        /// <summary>
        /// Run a sql batch for the datasource.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="script">The sql batch</param>
        protected static void InitScript(IDataSource datasource, string script)
        {
            InitScript(datasource, script, true);
        }

        /// <summary>
        /// Run a sql batch for the datasource.
        /// </summary>
        /// <param name="datasource">The datasource.</param>
        /// <param name="script">The sql batch</param>
        /// <param name="doParse">parse out the statements in the sql script file.</param>
        protected static void InitScript(IDataSource datasource, string script, bool doParse)
        {
            ScriptRunner runner = new ScriptRunner();

            runner.RunScript(datasource, script, doParse);
        }

        /// <summary>
        /// Create a new account with id = 6
        /// </summary>
        /// <returns>An account</returns>
        protected Account NewAccount6()
        {
            Account account = new Account();
            account.Id = 6;
            account.FirstName = "Calamity";
            account.LastName = "Jane";
            account.EmailAddress = "no_email@provided.com";
            return account;
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertGilles(Account account)
        {
            Assert.That(account.Id, Is.EqualTo(5), "account.Id");
            Assert.That(account.FirstName, Is.EqualTo("Gilles"), "account.FirstName");
            Assert.That(account.LastName, Is.EqualTo("Bayon"), "account.LastName");
            Assert.That(account.EmailAddress, Is.EqualTo("gilles.bayon@nospam.org"), "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertAccount1(Account account)
        {
            Assert.That(account.Id, Is.EqualTo(1), "account.Id");
            Assert.That(account.FirstName, Is.EqualTo("Joe"), "account.FirstName");
            Assert.That(account.LastName, Is.EqualTo("Dalton"), "account.LastName");
            Assert.That(account.EmailAddress, Is.EqualTo("Joe.Dalton@somewhere.com"), "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account as hashtable</param>
        protected void AssertAccount1AsHashtable(Hashtable account)
        {
            Assert.That((int)account["Id"], Is.EqualTo(1), "account.Id");
            Assert.That((string)account["FirstName"], Is.EqualTo("Joe"), "account.FirstName");
            Assert.That((string)account["LastName"], Is.EqualTo("Dalton"), "account.LastName");
            Assert.That((string)account["EmailAddress"], Is.EqualTo("Joe.Dalton@somewhere.com"), "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=1).
        /// </summary>
        /// <param name="account">An account as hashtable</param>
        protected void AssertAccount1AsHashtableForResultClass(Hashtable account)
        {
            Assert.That((int)account[BaseTest.ConvertKey("Id")], Is.EqualTo(1), "account.Id");
            Assert.That((string)account[BaseTest.ConvertKey("FirstName")], Is.EqualTo("Joe"), "account.FirstName");
            Assert.That((string)account[BaseTest.ConvertKey("LastName")], Is.EqualTo("Dalton"), "account.LastName");
            Assert.That((string)account[BaseTest.ConvertKey("EmailAddress")], Is.EqualTo("Joe.Dalton@somewhere.com"), "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input account is equal to the account(id=6).
        /// </summary>
        /// <param name="account">An account object</param>
        protected void AssertAccount6(Account account)
        {
            Assert.That(account.Id, Is.EqualTo(6), "account.Id");
            Assert.That(account.FirstName, Is.EqualTo("Calamity"), "account.FirstName");
            Assert.That(account.LastName, Is.EqualTo("Jane"), "account.LastName");
            Assert.That(account.EmailAddress, Is.Null, "account.EmailAddress");
        }

        /// <summary>
        /// Verify that the input order is equal to the order(id=1).
        /// </summary>
        /// <param name="order">An order object.</param>
        protected void AssertOrder1(Order order)
        {
            DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

            Assert.That(order.Id, Is.EqualTo(1), "order.Id");
            Assert.That(order.Date.ToString(), Is.EqualTo(date.ToString()), "order.Date");
            Assert.That(order.CardType, Is.EqualTo("VISA"), "order.CardType");
            Assert.That(order.CardNumber, Is.EqualTo("999999999999"), "order.CardNumber");
            Assert.That(order.CardExpiry, Is.EqualTo("05/03"), "order.CardExpiry");
            Assert.That(order.Street, Is.EqualTo("11 This Street"), "order.Street");
            Assert.That(order.City, Is.EqualTo("Victoria"), "order.City");
            Assert.That(order.Province, Is.EqualTo("BC"), "order.Id");
            Assert.That(order.PostalCode, Is.EqualTo("C4B 4F4"), "order.PostalCode");
        }

        /// <summary>
        /// Verify that the input order is equal to the order(id=1).
        /// </summary>
        /// <param name="order">An order as hashtable.</param>
        protected void AssertOrder1AsHashtable(Hashtable order)
        {
            DateTime date = new DateTime(2003, 2, 15, 8, 15, 00);

            Assert.That((int)order["Id"], Is.EqualTo(1), "order.Id");
            Assert.That(((DateTime)order["Date"]).ToString(), Is.EqualTo(date.ToString()), "order.Date");
            Assert.That((string)order["CardType"], Is.EqualTo("VISA"), "order.CardType");
            Assert.That((string)order["CardNumber"], Is.EqualTo("999999999999"), "order.CardNumber");
            Assert.That((string)order["CardExpiry"], Is.EqualTo("05/03"), "order.CardExpiry");
            Assert.That((string)order["Street"], Is.EqualTo("11 This Street"), "order.Street");
            Assert.That((string)order["City"], Is.EqualTo("Victoria"), "order.City");
            Assert.That((string)order["Province"], Is.EqualTo("BC"), "order.Id");
            Assert.That((string)order["PostalCode"], Is.EqualTo("C4B 4F4"), "order.PostalCode");
        }
    }
}
