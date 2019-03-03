#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-03-22 22:39:21 +0100 (mer., 22 mars 2006) $
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
using System.Data;

using IBatisNet.Common;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.SessionStore;

namespace IBatisNet.DataAccess
{
    /// <summary>
    /// Contract for a dao manager
    /// </summary>
    public interface IDaoManager
    {
        /// <summary>
        /// Name used to identify the the <see cref="IDaoManager"/>
        /// </summary>
        string Id { get;set; }

        /// <summary>
        /// Allow to set a custom session store like the <see cref="HybridWebThreadSessionStore"/>
        /// </summary>
        /// <remarks>Set it after the configuration and before use of the <see cref="IDaoManager"/></remarks>
        /// <example>
        /// daoManager.SessionStore = new HybridWebThreadSessionStore( daoManager.Id );
        /// </example>
        ISessionStore SessionStore { set; }
        
        /// <summary>
        /// Begins a database transaction with the specified isolation level.
        /// </summary>
        /// <param name="isolationLevel">
        /// The isolation level under which the transaction should run.
        /// </param>
        /// <returns>A IDalSession.</returns>
        IDalSession BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        /// <returns>A IDalSession</returns>
        IDalSession BeginTransaction();

        /// <summary>
        /// Close a connection
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        /// <remarks>
        /// Auto close the connection.
        /// </remarks>
        void CommitTransaction();

        /// <summary>
        /// Gets a Dao instance for the requested interface type.
        /// </summary>
        /// <param name="daoInterface">The requested interface type.</param>
        /// <returns>A Dao instance</returns>
        IDao GetDao(Type daoInterface);


        /// <summary>
        /// Get a new DaoSession
        /// </summary>
        /// <returns>A new DaoSession</returns>
        DaoSession GetDaoSession();


        /// <summary>
        /// Determines whether [is DAO session started].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is DAO session started]; otherwise, <c>false</c>.
        /// </returns>
        bool IsDaoSessionStarted();

        /// <summary>
        /// Gets the local DAO session.
        /// </summary>
        /// <value>The local DAO session.</value>
        IDalSession LocalDaoSession { get; }

        /// <summary>
        /// Gets the local data source.
        /// </summary>
        /// <value>The local data source.</value>
        IDataSource LocalDataSource { get; }


        /// <summary>
        /// Open a connection, on the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        IDalSession OpenConnection(string connectionString);

        /// <summary>
        /// Open a connection.
        /// </summary>
        /// <returns>A IDalSession.</returns>
        IDalSession OpenConnection();

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        /// <remarks>
        /// Close the connection.
        /// </remarks>
        void RollBackTransaction();

        /// <summary>
        /// Gets a Dao instance for the requested interface type.
        /// </summary>
        IDao this[Type daoInterface] { get; }
    }
}
