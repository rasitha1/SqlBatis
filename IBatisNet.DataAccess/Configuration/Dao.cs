#region Apache Notice

/*****************************************************************************
 * $Header: $
 * $Revision: 408164 $
 * $Date: 2006-05-21 14:27:09 +0200 (dim., 21 mai 2006) $
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
using System.Xml.Serialization;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.DataAccess.Interfaces;

#endregion

namespace IBatisNet.DataAccess.Configuration
{
    /// <summary>
    ///     Summary description for
    /// </summary>
    [Serializable]
    [XmlRoot("dao", Namespace = "http://ibatis.apache.org/dataAccess")]
    public class Dao
    {
        #region Constructor (s) / Destructor

        #endregion

        #region Methods

        /// <summary>
        ///     Initialize dao object.
        /// </summary>
        public void Initialize(DaoManager daoManager)
        {
            try
            {
                _daoManager = daoManager;
                _daoImplementation = TypeUtils.ResolveType(Implementation);
                _daoInterface = TypeUtils.ResolveType(Interface);
                // Create a new instance of the Dao object.
                _daoInstance = _daoImplementation.GetConstructor(Type.EmptyTypes).Invoke(null) as IDao;
                _proxy = DaoProxy.NewInstance(this);
            }
            catch (Exception e)
            {
                throw new ConfigurationException(string.Format("Error configuring DAO. Cause: {0}", e.Message), e);
            }
        }

        #endregion

        #region Fields

        [NonSerialized] private string _interface;

        [NonSerialized] private string _implementation;

        [NonSerialized] private Type _daoImplementation;

        [NonSerialized] private Type _daoInterface;

        [NonSerialized] private IDao _daoInstance;

        [NonSerialized] private IDao _proxy;

        [NonSerialized] private DaoManager _daoManager;

        #endregion

        #region Properties

        /// <summary>
        ///     The implementation class of the dao.
        /// </summary>
        /// <example>IBatisNet.DataAccess.Test.Implementations.MSSQL.SqlAccountDao</example>
        [XmlAttribute("implementation")]
        public string Implementation
        {
            get => _implementation;
            set
            {
                if ((value == null) || (value.Length < 1))
                    throw new ArgumentNullException("The implementation attribut is mandatory in a dao tag.");
                _implementation = value;
            }
        }


        /// <summary>
        ///     The Interface class that the dao must implement.
        /// </summary>
        [XmlAttribute("interface")]
        public string Interface
        {
            get => _interface;
            set
            {
                if ((value == null) || (value.Length < 1))
                    throw new ArgumentNullException("The interface attribut is mandatory in a dao tag.");
                _interface = value;
            }
        }

        /// <summary>
        ///     The dao interface type.
        /// </summary>
        [XmlIgnore]
        public Type DaoInterface => _daoInterface;

        /// <summary>
        ///     The dao implementation type.
        /// </summary>
        [XmlIgnore]
        public Type DaoImplementation => _daoImplementation;

        /// <summary>
        ///     The concrete dao.
        /// </summary>
        [XmlIgnore]
        public IDao DaoInstance => _daoInstance;

        /// <summary>
        ///     The proxy dao.
        /// </summary>
        [XmlIgnore]
        public IDao Proxy => _proxy;

        /// <summary>
        ///     The DaoManager who manage this dao.
        /// </summary>
        [XmlIgnore]
        public DaoManager DaoManager => _daoManager;

        #endregion
    }
}