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

#region Autors

/************************************************
* Gilles Bayon 
*************************************************/

#endregion

#region Using

using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

#endregion

namespace IBatisNet.DataAccess
{
    /// <summary>
    ///     Summary description for MessageManager.
    /// </summary>
    public class MessageManager
    {
        #region Constructor

        /// <summary>
        ///     Constructor.
        /// </summary>
        public MessageManager()
        {
            _resourceManager =
                new ResourceManager(GetType().Namespace + RESOURCE_FILENAME, Assembly.GetExecutingAssembly());
        }

        #endregion


        /// <summary>
        ///     Class used to expose constants that represent keys in the resource file.
        /// </summary>
        internal abstract class MessageKeys
        {
            internal const string ViewAlreadyConfigured = "ViewAlreadyConfigured";
            internal const string CantFindCommandMapping = "CantFindCommandMapping";
            internal const string CantGetNextView = "CantGetNextView";
            internal const string DocumentNotValidated = "DocumentNotValidated";
        }

        #region Fields

        private const string RESOURCE_FILENAME = "IBatisNet.DataAccess";
        private readonly ResourceManager _resourceManager;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets a message manager for the assembly resource file.
        /// </summary>
        public static MessageManager Instance { get; } = new MessageManager();


        /// <summary>
        ///     Gets the message with the specified key from the assembly resource file.
        /// </summary>
        /// <param name="key">Key of the item to retrieve from the resource file.</param>
        /// <returns>Value from the resource file identified by the key.</returns>
        public string this[string key] => _resourceManager.GetString(key, CultureInfo.CurrentUICulture);

        #endregion

        #region Methods

        /// <summary>
        ///     Gets a resource stream.
        /// </summary>
        /// <param name="name">The resource key.</param>
        /// <returns>A resource stream.</returns>
        public Stream GetStream(string name)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(GetType().Namespace + "." + name);
        }

        /// <summary>
        ///     Formats a message stored in the assembly resource file.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <param name="format">The format arguments.</param>
        /// <returns>A formatted string.</returns>
        public string FormatMessage(string key, params object[] format)
        {
            return string.Format(CultureInfo.CurrentCulture, this[key], format);
        }

        #endregion
    }
}