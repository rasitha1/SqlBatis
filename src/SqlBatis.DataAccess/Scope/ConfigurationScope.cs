
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
 * $Date: 2006-03-04 15:21:51 +0100 (sam., 04 mars 2006) $
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

using System.Collections.Specialized;
using System.Xml;
using IBatisNet.DataAccess.Configuration;
using IBatisNet.DataAccess.DaoSessionHandlers;

#endregion

namespace IBatisNet.DataAccess.Scope
{
	/// <summary>
	/// Description résumée de ConfigurationScope.
	/// </summary>
	public class ConfigurationScope
	{
		#region Fields
		
		private ErrorContext _errorContext = null;
		private HybridDictionary _providers = new HybridDictionary();
		private NameValueCollection _properties = new NameValueCollection();
		private HybridDictionary _daoSectionHandlers = new HybridDictionary();
		private bool _useConfigFileWatcher = false;
		private XmlDocument _daoConfigDocument = null;
		private XmlNode _nodeContext = null;
		private XmlNamespaceManager _nsmgr = null;

		#endregion
	
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public ConfigurationScope()
		{
			_errorContext = new ErrorContext();

			_providers.Clear();
			_daoSectionHandlers.Clear();

			_daoSectionHandlers.Add(DomDaoManagerBuilder.DEFAULT_DAOSESSIONHANDLER_NAME, typeof(SimpleDaoSessionHandler));
			_daoSectionHandlers.Add("ADONET", typeof(SimpleDaoSessionHandler));
			_daoSectionHandlers.Add("SqlMap", typeof(SqlMapDaoSessionHandler));

		}
		#endregion 

		#region Properties

		
		/// <summary>
		/// XmlNamespaceManager
		/// </summary>
		public XmlNamespaceManager XmlNamespaceManager
		{
			set { _nsmgr = value; }
			get { return _nsmgr; }
		}

		/// <summary>
		/// The current context node we are analizing
		/// </summary>
		public XmlNode NodeContext
		{
			set
			{
				_nodeContext = value;
			}
			get
			{
				return _nodeContext;
			}
		}

		/// <summary>
		/// The XML dao config file
		/// </summary>
		public XmlDocument DaoConfigDocument
		{
			set
			{
				_daoConfigDocument = value;
			}
			get
			{
				return _daoConfigDocument;
			}
		}

		/// <summary>
		/// Tell us if we use Configuration File Watcher
		/// </summary>
		public bool UseConfigFileWatcher
		{
			set
			{
				_useConfigFileWatcher = value;
			}
			get
			{
				return _useConfigFileWatcher;
			}
		}

		/// <summary>
		///  Get the request's error context
		/// </summary>
		public ErrorContext ErrorContext
		{
			get
			{
				return _errorContext;
			}
		}

		/// <summary>
		///  List of providers
		/// </summary>
		public HybridDictionary Providers
		{
			get
			{
				return _providers;
			}
		}

		/// <summary>
		///  List of global properties
		/// </summary>
		public NameValueCollection Properties
		{
			get
			{
				return _properties;
			}
		}

		/// <summary>
		/// List of Dao Section Handlers
		/// </summary>
		public HybridDictionary DaoSectionHandlers
		{
			get { return _daoSectionHandlers; }
		}
		#endregion 
	}
}
