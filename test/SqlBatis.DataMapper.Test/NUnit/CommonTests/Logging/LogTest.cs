using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using SqlBatis.DataMapper.Logging;
using SqlBatis.DataMapper.Logging.Impl;
using NUnit.Framework;


namespace SqlBatis.DataMapper.Test.NUnit.CommonTests.Logging
{
	/// <summary>
	/// Summary description for LogTest.
	/// </summary>
	[TestFixture]
	public class LogTest
	{
		private ILog _log = null;
		private StringWriter outWriter = new StringWriter();
		private StringWriter errorWriter = new StringWriter();

		#region SetUp/TearDown
		[SetUp]
		public void SetUp()
		{
            LogManager.Adapter = new ConsoleOutLoggerFA(new NameValueCollection());

			_log = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

			outWriter.GetStringBuilder().Length = 0;
			errorWriter.GetStringBuilder().Length = 0;

			Console.SetOut(outWriter);
			Console.SetError(errorWriter);
		}

		[TearDown]
		public void TearDown()
		{}
		#endregion

		[Test]
		public void LogDebug()
		{
			string expectedLogOutput = "[DEBUG] SqlBatis.DataMapper.Test.NUnit.CommonTests.Logging.LogTest - LogDebug";
			string actualLogOutput = "";

			_log.Debug("LogDebug");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			Assert.That(actualLogOutput.IndexOf(expectedLogOutput), Is.GreaterThan(-1));
		}

		[Test]
		public void LogInfo()
		{
			string expectedLogOutput = "[INFO]  SqlBatis.DataMapper.Test.NUnit.CommonTests.Logging.LogTest - LogInfo";
			string actualLogOutput = "";

			_log.Info("LogInfo");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			Assert.That(actualLogOutput.IndexOf(expectedLogOutput), Is.GreaterThan(-1));
		}

		[Test]
		public void LogError()
		{
			string expectedLogOutput = "[ERROR] SqlBatis.DataMapper.Test.NUnit.CommonTests.Logging.LogTest - LogError";
			string actualLogOutput = "";

			_log.Error("LogError");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			Assert.That(actualLogOutput.IndexOf(expectedLogOutput), Is.GreaterThan(0));
		}

		[Test]
		public void LogFatal()
		{
			string expectedLogOutput = "[FATAL] SqlBatis.DataMapper.Test.NUnit.CommonTests.Logging.LogTest - LogFatal";
			string actualLogOutput = "";

			_log.Fatal("LogFatal");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			Assert.That(actualLogOutput.IndexOf(expectedLogOutput), Is.GreaterThan(0));
		}


		[Test]
		public void LogWarn()
		{
			string expectedLogOutput = "[WARN]  SqlBatis.DataMapper.Test.NUnit.CommonTests.Logging.LogTest - LogWarn";
			string actualLogOutput = "";

			_log.Warn("LogWarn");

			actualLogOutput = outWriter.GetStringBuilder().ToString();	
			int i = actualLogOutput.IndexOf(expectedLogOutput);
			Assert.That(actualLogOutput.IndexOf(expectedLogOutput), Is.GreaterThan(0));
		}

	}
}