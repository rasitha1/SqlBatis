
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 513043 $
 * $Date: 2007-02-28 23:56:03 +0100 (mer., 28 févr. 2007) $
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

using System.Data;
using IBatisNet.Common;

#endregion

namespace IBatisNet.DataAccess
{
	/// <summary>
	/// Abstract definition of a DataAccess Session
	/// </summary>
	public abstract class DaoSession : IDalSession
	{

		#region Fields
		/// <summary>
		/// 
		/// </summary>
		protected DaoManager daoManager = null;
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// The DaoManager that manages this Dao instance will be passed
		/// in as the parameter to this constructor automatically upon
		/// instantiation.
		/// </summary>
		/// <param name="daoManager"></param>
		public DaoSession(DaoManager daoManager) 
		{
			this.daoManager = daoManager;
		}
		#endregion

		#region IDalSession Members

		#region Properties

        /// <summary>
        /// The data source use by the session.
        /// </summary>
        /// <value></value>
		public abstract IDataSource DataSource
		{
			get;
		}


        /// <summary>
        /// The Connection use by the session.
        /// </summary>
        /// <value></value>
		public abstract IDbConnection Connection
		{
			get;
		}


        /// <summary>
        /// Indicates if a transaction is open  on
        /// the session.
        /// </summary>
        public abstract bool IsTransactionStart
		{
			get;
		}

        /// <summary>
        /// The Transaction use by the session.
        /// </summary>
        /// <value></value>
        public abstract IDbTransaction Transaction
        {
            get;
        }

		#endregion

		#region Methods

		/// <summary>
		/// Complete (commit) a transaction
		/// </summary>
		public abstract void Complete();

		/// <summary>
		/// Opens a database connection.
		/// </summary>
		public abstract void OpenConnection();

		/// <summary>
		/// Open a connection, on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		public abstract void OpenConnection(string connectionString);

		/// <summary>
		/// Closes the connection
		/// </summary>
		public abstract void CloseConnection();

		/// <summary>
		/// Begins a transaction.
		/// </summary>
		public abstract void BeginTransaction();

		/// <summary>
		/// Open a connection and begin a transaction on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		public abstract void BeginTransaction(string connectionString);

		/// <summary>
		/// Begins a database transaction
		/// </summary>
		/// <param name="openConnection">Open a connection.</param>
		public abstract void BeginTransaction(bool openConnection);

		/// <summary>
		/// Begins a transaction at the data source with the specified IsolationLevel value.
		/// </summary>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		public abstract void BeginTransaction(IsolationLevel isolationLevel);

		/// <summary>
		/// Open a connection and begin a transaction on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		public abstract void BeginTransaction(string connectionString, IsolationLevel isolationLevel);

		/// <summary>
		/// Begins a transaction on the current connection
		/// with the specified IsolationLevel value.
		/// </summary>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		/// <param name="openConnection">Open a connection.</param>
		public abstract void BeginTransaction(bool openConnection, IsolationLevel isolationLevel);

		/// <summary>
		/// Begins a transaction on the current connection
		/// with the specified IsolationLevel value.
		/// </summary>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		/// <param name="connectionString">The connection string</param>
		/// <param name="openConnection">Open a connection.</param>
		public abstract void BeginTransaction(string connectionString, bool openConnection, IsolationLevel isolationLevel);

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		/// <remarks>
		/// Will close the connection.
		/// </remarks>
		public abstract void CommitTransaction();

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		/// <param name="closeConnection">Close the connection</param>
		public abstract void CommitTransaction(bool closeConnection);

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		/// <remarks>
		/// Will close the connection.
		/// </remarks>
		public abstract void RollBackTransaction();
		
		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		/// <param name="closeConnection">Close the connection</param>
		public abstract void RollBackTransaction(bool closeConnection);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="commandType"></param>
		/// <returns></returns>
		public abstract IDbCommand CreateCommand(CommandType commandType);
		

		/// <summary>
		/// Create an IDataParameter
		/// </summary>
		/// <returns>An IDataParameter.</returns>
		public abstract IDbDataParameter CreateDataParameter();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public abstract IDbDataAdapter CreateDataAdapter();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public abstract IDbDataAdapter CreateDataAdapter(IDbCommand command);
		#endregion

		#endregion

		#region IDisposable Members

		#region Methods

		/// <summary>
		/// Releasing, or resetting resources.
		/// </summary>
		public abstract void Dispose();

		#endregion

		#endregion
	}
}
