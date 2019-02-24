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
using IBatisNet.Common.Xml;
using IBatisNet.DataAccess.Scope;

#endregion 

namespace IBatisNet.DataAccess.Configuration.Serializers
{
	/// <summary>
	/// Summary description for DaoDeSerializer.
	/// </summary>
	public class DaoDeSerializer
	{
		/// <summary>
		/// Deserialize a Dao object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static Dao Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			Dao dao = new Dao();

			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			dao.Implementation = NodeUtils.GetStringAttribute(prop, "implementation");
			dao.Interface = NodeUtils.GetStringAttribute(prop, "interface");

			return dao;
		}
	}
}
