
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 417269 $
 * $Date: 2006-06-26 20:21:12 +0200 (lun., 26 juin 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
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

#region Using

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using IBatisNet.Common;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;

#endregion

namespace IBatisNet.DataAccess.DaoSessionHandlers
{
	/// <summary>
	/// Summary description for SqlMapDaoSessionHandler.
	/// </summary>
	public class SqlMapDaoSessionHandler : IDaoSessionHandler
	{
		#region Fields
        private ISqlMapper _sqlMap;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
        public ISqlMapper SqlMap
		{
			get { return _sqlMap; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// 
		/// </summary>
		public SqlMapDaoSessionHandler()
		{
		}
		#endregion
		
		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="properties"></param>
		/// <param name="resources"></param>
		public void Configure(NameValueCollection properties, IDictionary resources)
		{
			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			XmlDocument document = null;

			try
			{
				DataSource dataSource = (DataSource) resources["DataSource"];
				bool useConfigFileWatcher = (bool) resources["UseConfigFileWatcher"];
				
				if (resources.Contains("resource")||resources.Contains("sqlMapConfigFile"))
				{
					string fileName = string.Empty;
					if (resources.Contains("resource"))
					{
						fileName = (string) resources["resource"];
					}else
					{
						fileName = (string) resources["sqlMapConfigFile"];
					}
					if (useConfigFileWatcher == true)
					{
						ConfigWatcherHandler.AddFileToWatch( Resources.GetFileInfo( fileName ) );
					}
					document = Resources.GetResourceAsXmlDocument(fileName);
				}
				else if ( resources.Contains("url") )
				{
					document = Resources.GetUrlAsXmlDocument( (string) resources["url"] );	
				}
				else if ( resources.Contains("embedded") )
				{
					document = Resources.GetEmbeddedResourceAsXmlDocument( (string) resources["embedded"] );	
				}
				else
				{
					throw new ConfigurationException("Invalid attribute on daoSessionHandler/property ");
				}
				_sqlMap = builder.Build( document, dataSource, useConfigFileWatcher, properties);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(string.Format("DaoManager could not configure SqlMapDaoSessionHandler.Cause: {0}", e.Message),e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="daoManager"></param>
		/// <returns></returns>
		public DaoSession GetDaoSession(DaoManager daoManager)
		{
			return (new SqlMapDaoSession(daoManager, _sqlMap));
		}
		#endregion

	}
}
