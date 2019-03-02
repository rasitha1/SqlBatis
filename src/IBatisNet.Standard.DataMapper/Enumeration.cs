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

namespace IBatisNet.DataMapper
{
    /// <summary>
    ///     Indicate if the generated key by a selectKey statement
    ///     concern a pre or post-generated key.
    /// </summary>
    public enum SelectKeyType
    {
        /// <summary>
        /// </summary>
        pre = 1,

        /// <summary>
        /// </summary>
        post = 2
    }

    /// <summary>
    /// </summary>
    public enum CacheKeyType
    {
        /// <summary>
        /// </summary>
        Object = 1,

        /// <summary>
        /// </summary>
        List = 2,

        /// <summary>
        /// </summary>
        Map = 3
    }
}