
#region Apache Notice
/*****************************************************************************
 * $Revision: 408164 $
 * $LastChangedDate: 2006-05-21 14:27:09 +0200 (dim., 21 mai 2006) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006/2005 - The Apache Software Foundation
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

using System;
using System.Xml.Serialization;
using SqlBatis.DataMapper.Utilities.Objects.Members;
using SqlBatis.DataMapper.Configuration.Sql.Dynamic.Handlers;

namespace SqlBatis.DataMapper.Configuration.Sql.Dynamic.Elements
{
	/// <summary>
	/// Represent an isLessThan sql tag element.
	/// </summary>
	[Serializable]
	[XmlRoot("isLessThan", Namespace="http://ibatis.apache.org/mapping")]
	public sealed class IsLessThan : Conditional
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="IsLessThan"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public IsLessThan(AccessorFactory accessorFactory)
		{
            Handler = new IsLessThanTagHandler(accessorFactory);
		}
	}
}
