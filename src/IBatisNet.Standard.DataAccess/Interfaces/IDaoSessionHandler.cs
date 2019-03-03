
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

using System.Collections;
using System.Collections.Specialized;

#endregion


namespace IBatisNet.DataAccess.Interfaces
{
	/// <summary>
	/// Description résumée de IDaoSessionHandler.
	/// </summary>
	public interface IDaoSessionHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="daoManager"></param>
		/// <returns></returns>
		DaoSession GetDaoSession(DaoManager daoManager);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="properties">The properties scope</param>
		/// <param name="resources">A collection of object useful to init</param>
		void Configure(NameValueCollection properties, IDictionary resources);
	}
}
