#region Apache Notice

/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-25 19:40:27 +0200 (mar., 25 avr. 2006) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006/2005 - The Apache Software Foundation
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

using System.Data;
using IBatisNet.DataMapper.Configuration.ResultMapping;

namespace IBatisNet.DataMapper.MappedStatements
{
    /// <summary>
    ///     All dataq tor retrieve 'select' <see cref="ResultProperty" />
    /// </summary>
    /// <remarks>
    ///     As ADO.NET allows one open <see cref="IDataReader" /> per connection at once, we keep
    ///     all the datas to open the next <see cref="IDataReader" /> after having closed the current.
    /// </remarks>
    public sealed class PostBindind
    {
        /// <summary>
        ///     Enumeration of the ExecuteQuery method.
        /// </summary>
        public enum ExecuteMethod
        {
            /// <summary>
            ///     Execute Query For Object
            /// </summary>
            ExecuteQueryForObject = 1,

            /// <summary>
            ///     Execute Query For IList
            /// </summary>
            ExecuteQueryForIList,

            /// <summary>
            ///     Execute Query For Generic IList
            /// </summary>
            ExecuteQueryForGenericIList,

            /// <summary>
            ///     Execute Query For Array List
            /// </summary>
            ExecuteQueryForArrayList,

            /// <summary>
            ///     Execute Query For Strong Typed IList
            /// </summary>
            ExecuteQueryForStrongTypedIList
        }

        #region Fields

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the statement.
        /// </summary>
        /// <value>The statement.</value>
        public IMappedStatement Statement { set; get; } = null;


        /// <summary>
        ///     Gets or sets the result property.
        /// </summary>
        /// <value>The result property.</value>
        public ResultProperty ResultProperty { set; get; } = null;


        /// <summary>
        ///     Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public object Target { set; get; } = null;


        /// <summary>
        ///     Gets or sets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public object Keys { set; get; } = null;


        /// <summary>
        ///     Gets or sets the method.
        /// </summary>
        /// <value>The method.</value>
        public ExecuteMethod Method { set; get; } = ExecuteMethod.ExecuteQueryForIList;

        #endregion
    }
}