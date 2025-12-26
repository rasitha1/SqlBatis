using System.IO;
using System.Xml;
using SqlBatis.DataMapper.Utilities;
using NUnit.Framework;

namespace SqlBatis.DataMapper.Test.NUnit.CommonTests.Utilities
{
	/// <summary>
	/// Description résumée de ResourcesTest.
	/// </summary>
	[TestFixture] 
	public class ResourcesTest
	{
		#region SetUp & TearDown

		/// <summary>
		/// SetUp
		/// </summary>
		[SetUp] 
		public void SetUp() 
		{
		}


		/// <summary>
		/// TearDown
		/// </summary>
		[TearDown] 
		public void Dispose()
		{ 
		} 

		#endregion

		#region Test ResourcesTest

		/// <summary>
		/// Test loading Embedded Resource
		/// </summary>
		[Test] 
		public void TestEmbeddedResource() 
		{
			XmlDocument doc = null;

			doc = Resources.GetEmbeddedResourceAsXmlDocument("SqlBatis.DataMapper.Test.properties.xml, SqlBatis.DataMapper.Test");

			Assert.That(doc, Is.Not.Null);
			Assert.That(doc.HasChildNodes, Is.True);
			Assert.That(doc.ChildNodes.Count, Is.EqualTo(2));
			Assert.That(doc.SelectNodes("/settings/add").Count, Is.EqualTo(4));
		}

		/// <summary>
		/// Test loading Embedded Resource
		/// </summary>
		[Test] 
		public void TestEmbeddedResourceWhenNamespaceDiffersFromAssemblyName() 
		{
			XmlDocument doc = null;

			doc = Resources.GetEmbeddedResourceAsXmlDocument("CompanyName.ProductName.Maps.ISCard.xml, OctopusService");

			Assert.That(doc, Is.Not.Null);
			Assert.That(doc.HasChildNodes, Is.True);
			Assert.That(doc.ChildNodes.Count, Is.EqualTo(2));
		}
		#endregion

		#region GetFileInfo Tests

		[Test] 
		public void GetFileInfoWithRelative() 
		{ 
			FileInfo fileInfo = Resources.GetFileInfo("SqlBatis.DataMapper.Test.dll");
			Assert.That(fileInfo, Is.Not.Null);
		}


		[Test] 
		public void GetFileInfoWithAbsolute() 
		{ 
			string resourcePath = Resources.ApplicationBase+Path.DirectorySeparatorChar+"SqlBatis.DataMapper.Test.dll";
			FileInfo fileInfo = Resources.GetFileInfo(resourcePath);
			Assert.That(fileInfo, Is.Not.Null);
		}

		[Test] 
		public void GetFileInfoWithAbsoluteProtocol() 
		{ 
			string resourcePath = "file://"+Resources.ApplicationBase+Path.DirectorySeparatorChar+"SqlBatis.DataMapper.Test.dll";
			FileInfo fileInfo = Resources.GetFileInfo(resourcePath);
			Assert.That(fileInfo, Is.Not.Null);
		}

		[Test] 
		public void GetFileInfoWithAbsoluteProtocolPlusSlash() 
		{ 
			string resourcePath = "file:///"+Resources.ApplicationBase+Path.DirectorySeparatorChar+"SqlBatis.Data.Mapper.Test.dll";
			FileInfo fileInfo = Resources.GetFileInfo(resourcePath);
			Assert.That(fileInfo, Is.Not.Null);
		}
		#endregion 

		#region GetConfigAsXmlDocument Tests

		[Test] 
		public void GetConfigAsXmlDocumentWithAbsolute() 
		{ 
			string resourcePath = Resources.ApplicationBase+Path.DirectorySeparatorChar+"SqlMap_MSSQL_SqlClient.config";
			XmlDocument doc = Resources.GetConfigAsXmlDocument(resourcePath);
			Assert.That(doc, Is.Not.Null);
		}

		[Test] 
		public void GetConfigAsXmlDocumentWithAbsoluteProtocol() 
		{ 
			string resourcePath = "file://"+Resources.ApplicationBase+Path.DirectorySeparatorChar+"SqlMap_MSSQL_SqlClient.config";
			XmlDocument doc = Resources.GetConfigAsXmlDocument(resourcePath);
			Assert.That(doc, Is.Not.Null);
		}


		[Test] 
		public void GetConfigAsXmlDocumentWithAbsoluteProtocolPlusSlash() 
		{ 
			XmlDocument doc = Resources.GetConfigAsXmlDocument("file:///"+Resources.ApplicationBase+Path.DirectorySeparatorChar+"SqlMap_MSSQL_SqlClient.config");
			Assert.That(doc, Is.Not.Null);
		}

		[Test] 
		public void GetConfigAsXmlDocumentWithRelative() 
		{ 
			XmlDocument doc = Resources.GetConfigAsXmlDocument("SqlMap_MSSQL_SqlClient.config");
			Assert.That(doc, Is.Not.Null);
		}

		#endregion
	}
}
