using System;

using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.Test.Domain;

namespace IBatisNet.DataAccess.Test.Dao.Interfaces
{
	/// <summary>
	/// Description résumée de IAccountDao.
	/// </summary>
	public interface IAccountDao
	{
		/// <summary>
		/// Get an account by his id.
		/// </summary>
		/// <param name="accountID">An account id.</param>
		/// <returns>An account.</returns>
		Account GetAccountById(int accountID);

		/// <summary>
		/// Create an account
		/// </summary>
		/// <param name="account">The account to create</param>
		void Create(Account account);

		/// <summary>
		/// Update an account
		/// </summary>
		/// <param name="account">The account to create</param>
		void Update(Account account);

		/// <summary>
		/// Delete an account
		/// </summary>
		/// <param name="account">The account to delete</param>
		void Delete(Account account);
	}
}
