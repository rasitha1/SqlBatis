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

#region Using

using System.Collections.Specialized;
using System.Xml;
using IBatisNet.DataAccess.Configuration;
using IBatisNet.DataAccess.DaoSessionHandlers;

#endregion

namespace IBatisNet.DataAccess.Scope
{
    /// <summary>
    ///     Description résumée de ConfigurationScope.
    /// </summary>
    public class ConfigurationScope
    {
        #region Constructors

        /// <summary>
        ///     Default constructor
        /// </summary>
        public ConfigurationScope()
        {
            ErrorContext = new ErrorContext();

            Providers.Clear();
            DaoSectionHandlers.Clear();

            DaoSectionHandlers.Add(DomDaoManagerBuilder.DEFAULT_DAOSESSIONHANDLER_NAME,
                typeof(SimpleDaoSessionHandler));
            DaoSectionHandlers.Add("ADONET", typeof(SimpleDaoSessionHandler));
            DaoSectionHandlers.Add("SqlMap", typeof(SqlMapDaoSessionHandler));
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        /// <summary>
        ///     XmlNamespaceManager
        /// </summary>
        public XmlNamespaceManager XmlNamespaceManager { set; get; } = null;

        /// <summary>
        ///     The current context node we are analizing
        /// </summary>
        public XmlNode NodeContext { set; get; } = null;

        /// <summary>
        ///     The XML dao config file
        /// </summary>
        public XmlDocument DaoConfigDocument { set; get; } = null;

        /// <summary>
        ///     Tell us if we use Configuration File Watcher
        /// </summary>
        public bool UseConfigFileWatcher { set; get; } = false;

        /// <summary>
        ///     Get the request's error context
        /// </summary>
        public ErrorContext ErrorContext { get; }

        /// <summary>
        ///     List of providers
        /// </summary>
        public HybridDictionary Providers { get; } = new HybridDictionary();

        /// <summary>
        ///     List of global properties
        /// </summary>
        public NameValueCollection Properties { get; } = new NameValueCollection();

        /// <summary>
        ///     List of Dao Section Handlers
        /// </summary>
        public HybridDictionary DaoSectionHandlers { get; } = new HybridDictionary();

        #endregion
    }
}