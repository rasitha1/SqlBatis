using System.Collections;
using System.IO;
using SqlBatis.DataMapper.Configuration;
//using log4net;

using NUnit.Framework;

// DataSource definition
using SqlBatis.DataMapper.Utilities; // ScriptRunner definition
// SqlMap API

using SqlBatis.DataMapper.Test.Domain;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using ConfigurationBuilder= Microsoft.Extensions.Configuration.ConfigurationBuilder;

//[assembly:log4net.Config.XmlConfigurator(Watch=true)]

namespace SqlBatis.DataMapper.Test.NUnit.CommonTests.Transaction
{
	/// <summary>
	/// Summary description for BaseTest.
	/// </summary>
	public abstract class BaseTest
	{
		/// <summary>
		/// The sqlMap
		/// </summary>
		protected static ISqlMapper sqlMap;


		//private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );
		
		protected static string ScriptDirectory = null;

		/// <summary>
		/// Constructor
		/// </summary>
		static BaseTest()
		{
		    ConfigurationBuilder builder = new ConfigurationBuilder();
		    builder.AddInMemoryCollection(DefaultConfigurationStrings);
		    Configuration = builder.Build();
		    //LogManager.Adapter = new ConsoleOutLoggerFA(new NameValueCollection());

            ScriptDirectory = Path.Combine(
				Path.Combine(
				Path.Combine(
                Path.Combine(Resources.ApplicationBase, ".."), ".."), "Scripts"), Configuration["database"]) + Path.DirectorySeparatorChar;
        }

	    static IReadOnlyDictionary<string, string> DefaultConfigurationStrings { get; } =
	        new Dictionary<string, string>()
	        {
	            ["database"] = "MSSQL",
	            ["providerType"] = "SqlClient"
	        };

	    protected static IConfiguration Configuration { get; }
	    
        /// <summary>
        /// Initialize an sqlMap
        /// </summary>
        protected static void InitSqlMap()
		{
            //DateTime start = DateTime.Now;


            ConfigureHandler handler = new ConfigureHandler( Configure );
			DomSqlMapBuilder builder = new DomSqlMapBuilder();
            sqlMap = builder.ConfigureAndWatch("sqlmap" + "_" + Configuration["database"] + "_"
                + Configuration["providerType"] + ".config", handler);

            //string loadTime = DateTime.Now.Subtract(start).ToString();
            //Console.WriteLine("Loading configuration time :"+loadTime);
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
			sqlMap = (ISqlMapper)obj;
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



	}
}
