using System;

using IBatisNet.DataMapper;
using IBatisNet.DataAccess.DaoSessionHandlers; // SqlMapDaoSession
using IBatisNet.DataAccess.Exceptions;

// Domain Dao
using IBatisNet.DataAccess.Test.Dao.Interfaces; // IAccountDao
using IBatisNet.DataAccess.Test.Implementations; // BaseDao

using IBatisNet.DataAccess.Test.Domain;

namespace IBatisNet.DataAccess.Test.Dao.Implementations.DataMapper
{
	/// <summary>
	/// Summary description for SqlAccountViaSqlMapDao.
	/// </summary>
	public class AccountDao : BaseDao, IAccountDao
	{

		/// <summary>
		/// Create an account
		/// </summary>
		/// <param name="account">The account to create</param>
		public void Create(Account account)
		{
			SqlMapDaoSession sqlMapDaoSession = null;

			try
			{
				sqlMapDaoSession = (SqlMapDaoSession)this.GetContext();

				ISqlMapper sqlMap = sqlMapDaoSession.SqlMap;

				sqlMap.Insert("InsertAccountViaParameterMap", account);
			}
			catch(DataAccessException ex)
			{
				throw new DataAccessException("Error executing SqlAccountViaSqlMapDao.Create. Cause :" +ex.Message,ex);
			}
		}

		/// <summary>
		/// Update an account
		/// </summary>
		/// <param name="account">The account to Update</param>
		public void Update(Account account)
		{
			SqlMapDaoSession sqlMapDaoSession = null;

			try
			{
				sqlMapDaoSession = (SqlMapDaoSession)this.GetContext();

				ISqlMapper sqlMap = sqlMapDaoSession.SqlMap;

				sqlMap.Update("UpdateAccountViaParameterMap", account);
			}
			catch(DataAccessException ex)
			{
				throw new DataAccessException("Error executing SqlAccountViaSqlMapDao.Create. Cause :" +ex.Message,ex);
			}
		}

		/// <summary>
		/// Update an account
		/// </summary>
		/// <param name="account">The account to Delete</param>
		public void Delete(Account account)
		{
			SqlMapDaoSession sqlMapDaoSession = null;

			try
			{
				sqlMapDaoSession = (SqlMapDaoSession)this.GetContext();

				ISqlMapper sqlMap = sqlMapDaoSession.SqlMap;

				sqlMap.Delete("DeleteAccountViaInlineParameters", account);
			}
			catch(DataAccessException ex)
			{
				throw new DataAccessException("Error executing SqlAccountViaSqlMapDao.Delete. Cause :" +ex.Message,ex);
			}
		}

		/// <summary>
		/// Get an account by his id.
		/// </summary>
		/// <param name="accountID">An account id.</param>
		/// <returns>An account.</returns>
		public Account GetAccountById(int accountID)
		{
			SqlMapDaoSession sqlMapDaoSession = null;

			try
			{
				sqlMapDaoSession = (SqlMapDaoSession)this.GetContext();

				ISqlMapper sqlMap = sqlMapDaoSession.SqlMap;

				return (Account) sqlMap.QueryForObject("GetAccountViaColumnName", accountID);
			}
			catch(DataAccessException ex)
			{
				throw new DataAccessException("Error executing SqlAccountViaSqlMapDao.GetAccountById. Cause :" +ex.Message,ex);
			}
		}


	}
}
