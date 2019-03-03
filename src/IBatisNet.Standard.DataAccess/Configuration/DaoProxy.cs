
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
 * $Date: 2006-03-04 15:21:51 +0100 (sam., 04 mars 2006) $
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
using System.Collections;
using System.Reflection;
using Castle.DynamicProxy;
using IBatisNet.Common.Logging;
using IBatisNet.DataAccess.Exceptions;
using IBatisNet.DataAccess.Interfaces;

#endregion

namespace IBatisNet.DataAccess.Configuration
{
	/// <summary>
	/// Summary description for DaoProxy.
	/// </summary>
	[CLSCompliant(false)]
	public class DaoProxy : IInterceptor	
	{
		#region Fields
		private static ArrayList _passthroughMethods = new ArrayList();
		private Dao _daoImplementation;
		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor for a DaoProxy
		/// </summary>
		static DaoProxy()
		{
			_passthroughMethods.Add("GetType");
			_passthroughMethods.Add("ToString");
		}

		/// <summary>
		/// Create a new proxy for the Dao
		/// </summary>
		/// <param name="dao">The dao object to proxy</param>
		public DaoProxy(Dao dao)
		{
			_daoImplementation = dao;
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dao"></param>
		/// <returns></returns>
		public static IDao NewInstance(Dao dao) 
		{
			ProxyGenerator proxyGenerator = new ProxyGenerator();
			IInterceptor handler = new DaoProxy(dao);
			Type[] interfaces = {dao.DaoInterface, typeof(IDao)};

		    return (proxyGenerator.CreateInterfaceProxyWithTargetInterface(dao.DaoInterface, interfaces,
		        dao.DaoInstance, handler) as IDao);
        }
		#endregion

		#region IInterceptor menbers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invocation"></param>
		/// <returns></returns>
		public void Intercept(IInvocation invocation)
		{
			Object result = null;

			#region Logging
			if (_logger.IsDebugEnabled) 
			{
				_logger.Debug("Dao Proxy call to " + invocation.Method.Name);
			}
			#endregion

			if (_passthroughMethods.Contains(invocation.Method.Name)) 
			{
				try 
				{
					result = invocation.Method.Invoke( _daoImplementation.DaoInstance, invocation.Arguments);
				} 
				catch (Exception e) 
				{
					throw UnWrapException(e, invocation.Method.Name);
				}
			} 
			else 
			{
				DaoManager daoManager = _daoImplementation.DaoManager;
				if ( daoManager.IsDaoSessionStarted() ) 
				{
					try 
					{
						result = invocation.Method.Invoke(_daoImplementation.DaoInstance, invocation.Arguments);
					} 
					catch (Exception e) 
					{
						throw UnWrapException(e, invocation.Method.Name);
					}
				} 
				else 
				{
					#region Logging
					if (_logger.IsDebugEnabled) 
					{
						_logger.Debug("Dao Proxy, Open a connection ");
					}
					#endregion
					// Open a connection
					try 
					{
						daoManager.OpenConnection();
						result = invocation.Method.Invoke(_daoImplementation.DaoInstance, invocation.Arguments);
					} 
					catch (Exception e) 
					{
						throw UnWrapException(e, invocation.Method.Name);
					} 
					finally 
					{
						daoManager.CloseConnection();
					}
				}
			}

			#region Logging
			if (_logger.IsDebugEnabled) 
			{
				_logger.Debug("End of proxyfied call to " + invocation.Method.Name);
			}
			#endregion

			invocation.ReturnValue = result;
		}

		private Exception UnWrapException(Exception ex, string methodName) 
		{
			Exception e = ex;
			while (true) 
			{
				if (typeof(DataAccessException).IsInstanceOfType(e))
				{
					return e;
				}
				else if (e.InnerException != null)
				{
					e = e.InnerException;
				}
				else
				{
					e = new DataAccessException( string.Format("DaoProxy : unable to intercept method name '{0}', cause : {1}", methodName, e.Message), e);
				}			
			}
		}

		#endregion


	}
}
