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

#region Using

using IBatisNet.DataMapper.Configuration.ParameterMapping;

#endregion

namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic
{
    /// <summary>
    ///     Summary description for SqlText.
    /// </summary>
    public sealed class SqlText : ISqlChild
    {
        #region Fields

        private string _text = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                IsWhiteSpace = (_text.Trim().Length == 0);
            }
        }

        /// <summary>
        /// </summary>
        public bool IsWhiteSpace { get; private set; }

        /// <summary>
        /// </summary>
        public ParameterProperty[] Parameters { get; set; } = null;

        #endregion
    }
}