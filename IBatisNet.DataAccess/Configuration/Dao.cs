
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 408164 $
 * $Date: 2006-05-21 14:27:09 +0200 (dim., 21 mai 2006) $
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

#region Imports
using System;
using System.Xml.Serialization;

using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess;
#endregion

namespace IBatisNet.DataAccess.Configuration
{
	/// <summary>
	/// Summary description for 
	/// </summary>
	[Serializable]
	[XmlRoot("dao", Namespace="http://ibatis.apache.org/dataAccess")]
	public class Dao
	{
		#region Fields
		[NonSerialized]
		private string _interface;
		[NonSerialized]
		private string _implementation;
		[NonSerialized]
		private Type _daoImplementation = null;
		[NonSerialized]
		private Type _daoInterface = null;
		[NonSerialized]
		private IDao _daoInstance= null;
		[NonSerialized]
		private IDao _proxy = null;
		[NonSerialized]
		private DaoManager _daoManager = null;
		#endregion
	
		#region Properties
		/// <summary>
		/// The implementation class of the dao. 
		/// </summary>
		/// <example>IBatisNet.DataAccess.Test.Implementations.MSSQL.SqlAccountDao</example>
		[XmlAttribute("implementation")]
		public string Implementation
		{
			get { return _implementation; }
			set
			{
				if ((value == null) || (value.Length < 1))
				{
					throw new ArgumentNullException("The implementation attribut is mandatory in a dao tag.");
				}
				_implementation = value;
			}
		}


		/// <summary>
		/// The Interface class that the dao must implement.
		/// </summary>
		[XmlAttribute("interface")]
		public string Interface
		{
			get { return _interface; }
			set
			{
				if ((value == null) || (value.Length < 1))
				{
					throw new ArgumentNullException("The interface attribut is mandatory in a dao tag.");
				}
				_interface = value;
			}
		}

		/// <summary>
		/// The dao interface type.
		/// </summary>
		[XmlIgnoreAttribute]
		public Type DaoInterface
		{
			get { return _daoInterface; }

		}

		/// <summary>
		/// The dao implementation type.
		/// </summary>
		[XmlIgnoreAttribute]
		public Type DaoImplementation
		{
			get { return _daoImplementation; }

		}

		/// <summary>
		/// The concrete dao.
		/// </summary>
		[XmlIgnoreAttribute]
		public IDao DaoInstance
		{
			get { return _daoInstance; }

		}

		/// <summary>
		/// The proxy dao.
		/// </summary>
		[XmlIgnoreAttribute]
		public IDao Proxy
		{
			get { return _proxy; }

		}

		/// <summary>
		/// The DaoManager who manage this dao.
		/// </summary>
		[XmlIgnoreAttribute]
		public DaoManager DaoManager
		{
			get { return _daoManager; }

		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public Dao()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// Initialize dao object.
		/// </summary>
		public void Initialize(DaoManager daoManager)
		{
			try
			{
				_daoManager = daoManager;
                _daoImplementation = TypeUtils.ResolveType(this.Implementation);
                _daoInterface = TypeUtils.ResolveType(this.Interface);
				// Create a new instance of the Dao object.
				_daoInstance = _daoImplementation.GetConstructor(Type.EmptyTypes).Invoke(null) as IDao;
				_proxy = DaoProxy.NewInstance(this);
			}
			catch(Exception e)
			{
				throw new ConfigurationException(string.Format("Error configuring DAO. Cause: {0}", e.Message), e);
			}
		}

		#endregion

	}
}
