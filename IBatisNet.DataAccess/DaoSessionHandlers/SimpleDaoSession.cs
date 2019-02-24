
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

using System;
using System.Data;
using System.Reflection;
using IBatisNet.Common;
using IBatisNet.Common.Logging;
using IBatisNet.DataAccess.Exceptions;

#endregion


namespace IBatisNet.DataAccess.DaoSessionHandlers
{
	/// <summary>
	/// An ADO.NET implementation of the DataAccess Session .
	/// </summary>
	public class SimpleDaoSession : DaoSession
	{

		#region Fields
		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

		private IDataSource _dataSource = null;
		private bool _isTransactionOpen = false;
		private bool _consistent = false;

		/// <summary>
		/// Holds value of connection
		/// </summary>
		private IDbConnection _connection = null;

		/// <summary>
		/// Holds value of transaction
		/// </summary>
		private IDbTransaction _transaction = null;
		#endregion

		#region Properties


        /// <summary>
        /// The data source use by the session.
        /// </summary>
        /// <value></value>
		public override IDataSource DataSource
		{
			get { return _dataSource; }
		}


        /// <summary>
        /// The Connection use by the session.
        /// </summary>
        /// <value></value>
		public override IDbConnection Connection
		{
			get { return _connection; }
		}


        /// <summary>
        /// The Transaction use by the session.
        /// </summary>
        /// <value></value>
		public override IDbTransaction Transaction
		{
			get { return _transaction; }
		}


        /// <summary>
        /// Indicates if a transaction is open  on
        /// the session.
        /// </summary>
        /// <value></value>
        public override bool IsTransactionStart
        {
            get { return _isTransactionOpen; }
        }

		/// <summary>
		/// Changes the vote for transaction to commit (true) or to abort (false).
		/// </summary>
		private bool Consistent
		{
			set { _consistent = value; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="daoManager"></param>
		/// <param name="dataSource"></param>
		public SimpleDaoSession(DaoManager daoManager, DataSource dataSource):base(daoManager) 
		{
			_dataSource = dataSource;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Complete (commit) a transaction
		/// </summary>
		/// <remarks>
		/// Use in 'using' syntax.
		/// </remarks>
		public override void Complete()
		{
			this.Consistent = true;
		}

		/// <summary>
		/// Opens a database connection.
		/// </summary>
		public override void OpenConnection()
		{
			this.OpenConnection(_dataSource.ConnectionString);
		}

		/// <summary>
		/// Open a connection, on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		public override void OpenConnection(string connectionString)
		{
			if (_connection == null)
			{
				_connection =  _dataSource.DbProvider.CreateConnection();
				_connection.ConnectionString = connectionString;
				try
				{
					_connection.Open();
					if (_logger.IsDebugEnabled)
					{
						_logger.Debug(string.Format("Open Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description) );
					}
				}
				catch(Exception ex)
				{
					throw new DataAccessException( string.Format("Unable to open connection to \"{0}\".", _dataSource.DbProvider.Description), ex );
				}
			}
			else if (_connection.State != ConnectionState.Open)
			{
				try
				{
					_connection.Open();
					if (_logger.IsDebugEnabled)
					{
						_logger.Debug(string.Format("Open Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description) );
					}
				}
				catch(Exception ex)
				{
					throw new DataAccessException(string.Format("Unable to open connection to \"{0}\".", _dataSource.DbProvider.Description), ex );
				}
			}
		}

		/// <summary>
		/// Closes the connection
		/// </summary>
		public override void CloseConnection()
		{
			if ( (_connection != null) && (_connection.State == ConnectionState.Open) )
			{
				_connection.Close();
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug(string.Format("Close Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _dataSource.DbProvider.Description));
				}
			}
			_connection = null;
		}

		/// <summary>
		/// Begins a transaction.
		/// </summary>
		/// <remarks>
		/// Oepn a connection.
		/// </remarks>
		public override void BeginTransaction()
		{
			this.BeginTransaction( _dataSource.ConnectionString );
		}

		/// <summary>
		/// Open a connection and begin a transaction on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		public override void BeginTransaction(string connectionString)
		{
			if (_connection == null || _connection.State != ConnectionState.Open)
			{
				this.OpenConnection( connectionString );
			}
			_transaction = _connection.BeginTransaction();
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Begin Transaction.");
			}			
			_isTransactionOpen = true;
		}

		/// <summary>
		/// Begins a database transaction
		/// </summary>
		/// <param name="openConnection">Open a connection.</param>
		public override void BeginTransaction(bool openConnection)
		{
			if (openConnection)
			{
				this.BeginTransaction();
			}
			else
			{
				if (_connection == null || _connection.State != ConnectionState.Open)
				{
					throw new DataAccessException("SimpleDaoSession could not invoke BeginTransaction(). A Connection must be started. Call OpenConnection() first.");
				}
				_transaction = _connection.BeginTransaction();
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug("Begin Transaction.");
				}	
				_isTransactionOpen = true;
			}
		}

		/// <summary>
		/// Begins a transaction at the data source with the specified IsolationLevel value.
		/// </summary>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		public override void BeginTransaction(IsolationLevel isolationLevel)
		{
			this.BeginTransaction( _dataSource.ConnectionString, isolationLevel );
		}

		/// <summary>
		/// Open a connection and begin a transaction on the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string</param>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		public override void BeginTransaction(string connectionString, IsolationLevel isolationLevel)
		{
			if (_connection == null || _connection.State != ConnectionState.Open)
			{
				this.OpenConnection( connectionString );
			}
			_transaction = _connection.BeginTransaction(isolationLevel);
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Begin Transaction.");
			}
			_isTransactionOpen = true;			
		}

		/// <summary>
		/// Begins a transaction on the current connection
		/// with the specified IsolationLevel value.
		/// </summary>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		/// <param name="openConnection">Open the connection ?</param>
		public override void BeginTransaction(bool openConnection, IsolationLevel isolationLevel)
		{
			this.BeginTransaction( _dataSource.ConnectionString, openConnection, isolationLevel );
		}

		/// <summary>
		/// Begins a transaction on the current connection
		/// with the specified IsolationLevel value.
		/// </summary>
		/// <param name="isolationLevel">The transaction isolation level for this connection.</param>
		/// <param name="connectionString">The connection string</param>
		/// <param name="openConnection">Open a connection.</param>
		public override void BeginTransaction(string connectionString, bool openConnection, IsolationLevel isolationLevel)
		{
			if (openConnection)
			{
				this.BeginTransaction(connectionString, isolationLevel);
			}
			else
			{
				if (_connection == null || _connection.State != ConnectionState.Open)
				{
					throw new DataAccessException("SimpleDaoSession could not invoke StartTransaction(). A Connection must be started. Call OpenConnection() first.");
				}
				_transaction = _connection.BeginTransaction(isolationLevel);
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug("Begin Transaction.");
				}	
				_isTransactionOpen = true;
			}			
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		/// <remarks>
		/// Will close the connection.
		/// </remarks>
		public override void CommitTransaction()
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Commit Transaction");
			}		
	
			_transaction.Commit();
			_transaction.Dispose();
			_transaction= null;
            _isTransactionOpen = false;

			if (_connection.State != ConnectionState.Closed)
			{
				CloseConnection();
			}
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		/// <param name="closeConnection">Close the connection</param>
		public override void CommitTransaction(bool closeConnection)
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Commit Transaction");
			}

			_transaction.Commit();
			_transaction.Dispose();
			_transaction= null;
            _isTransactionOpen = false;

			if (closeConnection)
			{
				if (_connection.State != ConnectionState.Closed)
				{
					CloseConnection();
				}
			}
		}
		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		/// <remarks>
		/// Will close the connection.
		/// </remarks>
		public override void RollBackTransaction()
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("RollBack Transaction");
			}

			_transaction.Rollback();
			_transaction.Dispose();
			_transaction = null;
            _isTransactionOpen = false;

			if (_connection.State != ConnectionState.Closed)
			{
				CloseConnection();
			}
		}

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		/// <param name="closeConnection">Close the connection</param>
		public override void RollBackTransaction(bool closeConnection)
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("RollBack Transaction");
			}

			_transaction.Rollback();
			_transaction.Dispose();
			_transaction = null;
            _isTransactionOpen = false;

			if (closeConnection)
			{
				if (_connection.State != ConnectionState.Closed)
				{
					CloseConnection();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="commandType"></param>
		/// <returns></returns>
		public override IDbCommand CreateCommand(CommandType commandType)
		{
			IDbCommand command = null;

            command = _dataSource.DbProvider.CreateCommand();
			command.CommandType = commandType;
			command.Connection = _connection;

			// Assign transaction
			if (_transaction != null)
			{
				try
				{
					command.Transaction = _transaction;
				}
				catch 
				{}
			}
			// Assign connection timeout
			if (_connection!= null)
			{
				try // MySql provider doesn't suppport it !
				{
					command.CommandTimeout = _connection.ConnectionTimeout;
				}
				catch(NotSupportedException e)
				{
					_logger.Info(e.Message);
				}			
			}
			return command;
		}

		/// <summary>
		/// Create an IDataParameter
		/// </summary>
		/// <returns>An IDataParameter.</returns>
		public override IDbDataParameter CreateDataParameter()
		{
			IDbDataParameter dataParameter = _dataSource.DbProvider.CreateDataParameter();

			return dataParameter;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override IDbDataAdapter CreateDataAdapter()
		{
			return _dataSource.DbProvider.CreateDataAdapter();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public override IDbDataAdapter CreateDataAdapter(IDbCommand command)
		{
			IDbDataAdapter dataAdapter = null;

			dataAdapter = _dataSource.DbProvider.CreateDataAdapter();
			dataAdapter.SelectCommand = command;

			return dataAdapter;
			
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// 
		/// </summary>
		public override void Dispose()
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("Dispose DaoSession");
			}

			if (_isTransactionOpen == false)
			{
				if (_connection.State != ConnectionState.Closed)
				{
					daoManager.CloseConnection();
				}
			}
			else
			{
				if (_consistent)
				{
					daoManager.CommitTransaction();
				    _isTransactionOpen = false;
				}
				else
				{
					if (_connection.State != ConnectionState.Closed)
					{
						daoManager.RollBackTransaction();
                        _isTransactionOpen = false;
					}
				}
			}
		}
		#endregion
	}

}
