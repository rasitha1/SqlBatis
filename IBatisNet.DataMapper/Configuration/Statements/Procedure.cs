#region Apache Notice

/*****************************************************************************
 * $Header: $
 * $Revision: 443064 $
 * $Date: 2006-09-13 20:38:29 +0200 (mer., 13 sept. 2006) $
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

using System;
using System.Data;
using System.Xml.Serialization;
using IBatisNet.DataMapper.Scope;

#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
    /// <summary>
    ///     Represent a store Procedure.
    /// </summary>
    [Serializable]
    [XmlRoot("procedure", Namespace = "http://ibatis.apache.org/mapping")]
    public class Procedure : Statement
    {
        #region Constructor (s) / Destructor

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="configurationScope">The scope of the configuration</param>
        override internal void Initialize(ConfigurationScope configurationScope)
        {
            base.Initialize(configurationScope);
            if (ParameterMap == null)
                ParameterMap = configurationScope.SqlMapper.GetParameterMap(ConfigurationScope.EMPTY_PARAMETER_MAP);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The type of the statement StoredProcedure.
        /// </summary>
        [XmlIgnore]
        public override CommandType CommandType => CommandType.StoredProcedure;

        /// <summary>
        ///     Extend statement attribute
        /// </summary>
        [XmlIgnore]
        public override string ExtendStatement
        {
            get => string.Empty;
            set { }
        }

        #endregion
    }
}