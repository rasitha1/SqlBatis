using System;

using IBatisNet.Common;
using IBatisNet.DataAccess; //DaoManager, DaoSession
using IBatisNet.DataAccess.Interfaces; //IDao

namespace IBatisNet.DataAccess.Test.Implementations
{
	/// <summary>
	/// Description résumée de BaseDao.
	/// </summary>
	public class BaseDao: IDao
	{
		/// <summary>
		/// Get the sesion.
		/// </summary>
		/// <returns>A DaoSession</returns>
		protected DaoSession GetContext()
		{
			IDaoManager daoManager = DaoManager.GetInstance(this);
			return (daoManager.LocalDaoSession as DaoSession);
		}
	}
}
