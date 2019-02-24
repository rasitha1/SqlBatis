using System;

using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.Test.Domain;

namespace IBatisNet.DataAccess.Test.Dao.Interfaces
{
	/// <summary>
	/// Summary description for IuserDao.
	/// </summary>
	public interface IUserDao : IDao
	{
		/// <summary>
		/// Create an user
		/// </summary>
		/// <param name="user">The user to create</param>
		void Create(User user);

		/// <summary>
		/// Load a user
		/// </summary>
		User Load(string id);
	}
}
