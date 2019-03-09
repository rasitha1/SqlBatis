#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 378715 $
 * $Date: 2006-10-10 20:57:36 +0200 (mar., 10 oct. 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006 - Apache Fondation
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
using IBatisNet.Common;

namespace IBatisNet.DataAccess.SessionStore
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class AbstractSessionStore  : MarshalByRefObject, ISessionStore
	{
		const string KEY = "_IBATIS_LOCAL_DAOSESSION_";
	    /// <summary>
	    /// session name
	    /// </summary>
		protected string sessionName = string.Empty;

		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="daoManagerId">The DaoManager name.</param>
        public AbstractSessionStore(string daoManagerId)
		{
            sessionName = KEY + daoManagerId;
		}

		/// <summary>
		/// Get the local session
		/// </summary>
		public abstract IDalSession LocalSession
		{
			get; 
		}

		/// <summary>
		/// Store the specified session.
		/// </summary>
		/// <param name="session">The session to store</param>
        public abstract void Store(IDalSession session);

		/// <summary>
		/// Remove the local session from the storage.
		/// </summary>
        public abstract void Dispose();
	}
}
