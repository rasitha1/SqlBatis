
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
using IBatisNet.Common.Utilities;

#endregion

namespace IBatisNet.DataAccess.Configuration
{
	/// <summary>
	/// Description résumée de DaoSessionHandler.
	/// </summary>
	[Serializable]
	[XmlRoot("handler", Namespace="http://ibatis.apache.org/dataAccess")]
	public class DaoSessionHandler
	{
		#region Fields
		[NonSerialized]
		private string _name = string.Empty;
		[NonSerialized]
		private string _implementation =string.Empty;
		[NonSerialized]
		private bool _isDefault = false;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("default")]
		public bool IsDefault
		{             
			get { return _isDefault; }
			set {_isDefault = value;}
		}

	
		/// <summary>
		/// 
		/// </summary>
		/// <example>
		/// Examples of Type: "IBatisNet.DataAccess.DaoSessionHandlers.SimpleDaoSessionHandler"
		/// </example>
		[XmlAttribute("implementation")]
		public string Implementation
		{
			get { return _implementation; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("Implementation");
				_implementation = value;
			}
		}

		/// <summary>
		/// The impelmenattion type
		/// </summary>
		public Type TypeInstance
		{
			get { return TypeUtils.ResolveType(_implementation); }
		}

		/// <summary>
		/// 
		/// </summary>
		[XmlAttribute("id")]
		public string Name
		{
			get { return _name; }
			set
			{
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("Name");
				_name = value;
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public DaoSessionHandler()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name"></param>
		/// <param name="implementation"></param>
		public DaoSessionHandler(string name, string implementation)
		{
			if ((implementation == null) || (implementation.Length < 1))
				throw new ArgumentNullException("implementation");
			if ((name == null) || (name.Length < 1))
				throw new ArgumentNullException("name");

			_implementation = implementation;
			_name = name;
		}
		#endregion

	}
}
