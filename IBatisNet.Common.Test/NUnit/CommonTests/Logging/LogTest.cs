using System;
using System.IO;
using System.Reflection;
using IBatisNet.Common.Logging;
using NUnit.Framework;


namespace IBatisNet.Common.Test.NUnit.CommonTests.Logging
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
			string expectedLogOutput = "[DEBUG] IBatisNet.Common.Test.NUnit.CommonTests.Logging.LogTest - LogDebug";
			string actualLogOutput = "";

			_log.Debug("LogDebug");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			Assert.IsTrue(actualLogOutput.IndexOf(expectedLogOutput)>0);
		}

		[Test]
		public void LogInfo()
		{
			string expectedLogOutput = "[INFO]  IBatisNet.Common.Test.NUnit.CommonTests.Logging.LogTest - LogInfo";
			string actualLogOutput = "";

			_log.Info("LogInfo");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			Assert.IsTrue(actualLogOutput.IndexOf(expectedLogOutput)>0);
		}

		[Test]
		public void LogError()
		{
			string expectedLogOutput = "[ERROR] IBatisNet.Common.Test.NUnit.CommonTests.Logging.LogTest - LogError";
			string actualLogOutput = "";

			_log.Error("LogError");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			Assert.IsTrue(actualLogOutput.IndexOf(expectedLogOutput)>0);
		}

		[Test]
		public void LogFatal()
		{
			string expectedLogOutput = "[FATAL] IBatisNet.Common.Test.NUnit.CommonTests.Logging.LogTest - LogFatal";
			string actualLogOutput = "";

			_log.Fatal("LogFatal");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			Assert.IsTrue(actualLogOutput.IndexOf(expectedLogOutput)>0);
		}


		[Test]
		public void LogWarn()
		{
			string expectedLogOutput = "[WARN]  IBatisNet.Common.Test.NUnit.CommonTests.Logging.LogTest - LogWarn";
			string actualLogOutput = "";

			_log.Warn("LogWarn");

			actualLogOutput = outWriter.GetStringBuilder().ToString();
			int i = actualLogOutput.IndexOf(expectedLogOutput);
			Assert.IsTrue(actualLogOutput.IndexOf(expectedLogOutput)>0);
		}

	}
}