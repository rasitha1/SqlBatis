using System;
using System.Collections;
using System.Data;

using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Exceptions;
using IBatisNet.DataAccess.Interfaces;

using IBatisNet.DataAccess.Test.Dao.Interfaces;
using IBatisNet.DataAccess.Test.Implementations;

using IBatisNet.DataAccess.Test.Domain;

namespace IBatisNet.DataAccess.Test.Dao.Implementations.Ado
{
	/// <summary>
	/// Description résumée de SqlAccountDao.
	/// </summary>
	public class AccountDao : BaseDao, IAccountDao
	{
		// Sql Server
		private const string SELECT_ACCOUNT_BY_ID ="select Account_ID, Account_FirstName, Account_LastName, Account_Email from Accounts where Account_ID = @Account_ID";
		private const string INSERT_ACCOUNT = "insert into Accounts (Account_ID, Account_FirstName, Account_LastName, Account_Email) values (@Account_ID, @Account_FirstName, @Account_LastName, @Account_Email)";
		private const string UPDATE_ACCOUNT = "update Accounts set Account_FirstName = @Account_FirstName, Account_LastName = @Account_LastName, Account_Email = @Account_Email where Account_ID = @Account_ID";
		private const string DELETE_ACCOUNT = "delete from Accounts where Account_ID = @Account_ID";

		// OLEDB doesn't support named parameters !!!
		// Caution : You must declare the parameters in the order declaration
		private const string OLEDB_SELECT_ACCOUNT_BY_ID ="select Account_ID, Account_FirstName, Account_LastName, Account_Email from Accounts where Account_ID = ?";
		private const string OLEDB_INSERT_ACCOUNT ="insert into Accounts (Account_ID, Account_FirstName, Account_LastName, Account_Email) values (?, ?, ?, ?)";
		private const string OLEDB_UPDATE_ACCOUNT ="update Accounts set Account_FirstName = ?, Account_LastName = ?, Account_Email = ? where Account_ID = ?";
		private const string OLEDB_DELETE_ACCOUNT ="delete from Accounts where Account_ID = ?";

		private const string PARAM_ACCOUNT_ID = "@Account_ID";
		private const string PARAM_ACCOUNT_FIRSTNAME = "@Account_FirstName";
		private const string PARAM_ACCOUNT_LASTNAME = "@Account_LastName";
		private const string PARAM_ACCOUNT_EMAIL = "@Account_Email";

		/// <summary>
		/// Create an account
		/// </summary>
		/// <param name="account">The account to create</param>
		public void Create(Account account)
		{
			IDbCommand command = null;
			DaoSession daoSession = null;

			daoSession = this.GetContext();

			command = daoSession.CreateCommand(CommandType.Text);

			try
			{
				if (daoSession.DataSource.DbProvider.UseParameterPrefixInSql)
				{
					command.CommandText = INSERT_ACCOUNT;
				}
				else
				{
					command.CommandText = OLEDB_INSERT_ACCOUNT;
				}

				IDbDataParameter sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_ID;
				sqlParameter.Value = account.Id;
				command.Parameters.Add(  sqlParameter );

				sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_FIRSTNAME;
				sqlParameter.Value = account.FirstName;
				command.Parameters.Add(  sqlParameter );

				sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_LASTNAME;
				sqlParameter.Value = account.LastName;
				command.Parameters.Add(  sqlParameter );

				sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_EMAIL;
				sqlParameter.Value = account.EmailAddress;
				command.Parameters.Add(  sqlParameter );

				command.ExecuteNonQuery();

				command.Parameters.Clear();
			}
			catch (System.Exception e)
			{
				throw new DataAccessException("Error executing SqlAccountDao.Create. Cause :" + e.Message, e);
			}
			finally
			{
				command.Dispose();
			}
		}


		/// <summary>
		/// Update an account
		/// </summary>
		/// <param name="account">The account to create</param>
		public void Update(Account account)
		{
			IDbCommand  command = null;
			DaoSession daoSession = null;

			daoSession = this.GetContext();

			command = daoSession.CreateCommand(CommandType.Text);

			try
			{
				if (daoSession.DataSource.DbProvider.UseParameterPrefixInSql)
				{
					command.CommandText = UPDATE_ACCOUNT;
				}
				else
				{
					command.CommandText = OLEDB_UPDATE_ACCOUNT;
				}

				IDbDataParameter sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_FIRSTNAME;
				sqlParameter.Value = account.FirstName;
				command.Parameters.Add(  sqlParameter );

				sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_LASTNAME;
				sqlParameter.Value = account.LastName;
				command.Parameters.Add(  sqlParameter );

				sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_EMAIL;
				sqlParameter.Value = account.EmailAddress;
				command.Parameters.Add(  sqlParameter );

				sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_ID;
				sqlParameter.Value = account.Id;
				command.Parameters.Add(  sqlParameter );

				command.ExecuteNonQuery();

				command.Parameters.Clear();
			}
			catch (System.Exception e)
			{
				throw new DataAccessException("Error executing SqlAccountDao.Update. Cause :" + e.Message, e);
			}
			finally
			{
				command.Dispose();
			}
		}


		/// <summary>
		/// Get an account by his id.
		/// </summary>
		/// <param name="accountID">An account id.</param>
		/// <returns>An account.</returns>
		public Account GetAccountById(int accountID)
		{
			IDbCommand command = null;
			DaoSession daoSession = null;
			Account account = null;

			daoSession = this.GetContext();

			command = daoSession.CreateCommand(CommandType.Text);

			try
			{
				if (daoSession.DataSource.DbProvider.UseParameterPrefixInSql)
				{
					command.CommandText = SELECT_ACCOUNT_BY_ID;
				}
				else
				{
					command.CommandText = OLEDB_SELECT_ACCOUNT_BY_ID;
				}

				IDbDataParameter sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_ID;
				sqlParameter.Value = accountID;
				command.Parameters.Add(  sqlParameter );

				IDataReader dataReader;

				dataReader = command.ExecuteReader();

				if (dataReader.Read())
				{
					account = PopulateAccount(dataReader);
				}

				dataReader.Close();
				command.Parameters.Clear();
			}
			catch (System.Exception e)
			{
				throw new DataAccessException("Error executing SqlAccountDao.GetAccountById. Cause :" + e.Message, e);
			}
			finally
			{
				command.Dispose();
			}

			return account;
		}


		/// <summary>
		/// Delete an account
		/// </summary>
		/// <param name="account">The account to delete</param>
		public void Delete(Account account)
		{
			IDbCommand command = null;
			DaoSession daoSession = null;

			daoSession = this.GetContext();

			command = daoSession.CreateCommand(CommandType.Text);

			try
			{
				if (daoSession.DataSource.DbProvider.UseParameterPrefixInSql)
				{
					command.CommandText = DELETE_ACCOUNT;
				}
				else
				{
					command.CommandText = OLEDB_DELETE_ACCOUNT;
				}

				IDbDataParameter sqlParameter = command.CreateParameter();
				sqlParameter.ParameterName = PARAM_ACCOUNT_ID;
				sqlParameter.Value = account.Id;
				command.Parameters.Add(  sqlParameter );

				command.ExecuteNonQuery();

				command.Parameters.Clear();
			}
			catch (System.Exception e)
			{
				throw new DataAccessException("Error executing SqlAccountDao.Delete. Cause :" + e.Message, e);
			}
			finally
			{
				command.Dispose();
			}
		}

		/// <summary>
		/// Populate an account object from an datareader object.
		/// </summary>
		/// <param name="dataReader">The datareader ue to populate the account.</param>
		/// <returns>The account.</returns>
		private Account PopulateAccount(IDataReader dataReader)
		{
			Account account = null;

			account = new Account();
 
			account.Id = (int)dataReader["Account_Id"];
			account.EmailAddress = dataReader.GetString(dataReader.GetOrdinal("Account_Email"));
			account.FirstName = dataReader.GetString(dataReader.GetOrdinal("Account_FirstName"));
			account.LastName = dataReader.GetString(dataReader.GetOrdinal("Account_LastName"));

			return account;
		}

	}
}
