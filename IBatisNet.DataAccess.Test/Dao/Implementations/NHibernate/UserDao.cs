using System;

using IBatisNet.DataMapper;
using IBatisNet.DataAccess.Extensions.DaoSessionHandlers; // NHibernateDaoSession
using IBatisNet.DataAccess.Exceptions;

// Domain Dao
using IBatisNet.DataAccess.Test.Dao.Interfaces; // IUserDao
using IBatisNet.DataAccess.Test.Implementations; // BaseDao

using IBatisNet.DataAccess.Test.Domain;

using NHibernate;

namespace IBatisNet.DataAccess.Test.Dao.Implementations.NHibernate
{
	/// <summary>
	/// Summary description for UserDao.
	/// </summary>
	public class UserDao : BaseDao, IUserDao
	{
		/// <summary>
		/// Create an user
		/// </summary>
		/// <param name="user">The user to create</param>
		public void Create(User user)
		{
			NHibernateDaoSession nHibernateDaoSession = null;

			try
			{
				nHibernateDaoSession = (NHibernateDaoSession)this.GetContext();

				ISession session = nHibernateDaoSession.Session;

				session.Save( user );
			}
			catch(DataAccessException ex)
			{
				throw new DataAccessException("Error executing UserDao.Create. Cause :" +ex.Message,ex);
			}
		}

		/// <summary>
		/// Load a user
		/// </summary>
		public User Load(string id)
		{
			NHibernateDaoSession nHibernateDaoSession = null;
			User user = null;

			try
			{
				nHibernateDaoSession = (NHibernateDaoSession)this.GetContext();

				ISession session = nHibernateDaoSession.Session;

				user = session.Load(typeof(User),id) as User;
			}
			catch(Exception ex)
			{
				throw new DataAccessException("Error executing UserDao.Create. Cause :" +ex.Message,ex);
			}

			return user;
		}
	}
}
