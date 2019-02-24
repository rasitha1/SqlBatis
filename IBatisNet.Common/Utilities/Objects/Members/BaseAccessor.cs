#region Apache Notice

/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-03-22 22:39:21 +0100 (mer., 22 mars 2006) $
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

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Reflection.Emit;

namespace IBatisNet.Common.Utilities.Objects.Members
{
    /// <summary>
    ///     Abstract base class for member accessor
    /// </summary>
    public abstract class BaseAccessor
    {
        /// <summary>
        ///     List of type-opCode
        /// </summary>
        protected static IDictionary typeToOpcode = new HybridDictionary();

        /// <summary>
        ///     The null internal value used by this member type
        /// </summary>
        protected object nullInternal = null;

        /// <summary>
        ///     The property name
        /// </summary>
        protected string propertyName = string.Empty;

        /// <summary>
        ///     The target type
        /// </summary>
        protected Type targetType = null;

        /// <summary>
        ///     Static constructor
        ///     "Initialize a private IDictionary with type-opCode pairs
        /// </summary>
        static BaseAccessor()
        {
            typeToOpcode[typeof(sbyte)] = OpCodes.Ldind_I1;
            typeToOpcode[typeof(byte)] = OpCodes.Ldind_U1;
            typeToOpcode[typeof(char)] = OpCodes.Ldind_U2;
            typeToOpcode[typeof(short)] = OpCodes.Ldind_I2;
            typeToOpcode[typeof(ushort)] = OpCodes.Ldind_U2;
            typeToOpcode[typeof(int)] = OpCodes.Ldind_I4;
            typeToOpcode[typeof(uint)] = OpCodes.Ldind_U4;
            typeToOpcode[typeof(long)] = OpCodes.Ldind_I8;
            typeToOpcode[typeof(ulong)] = OpCodes.Ldind_I8;
            typeToOpcode[typeof(bool)] = OpCodes.Ldind_I1;
            typeToOpcode[typeof(double)] = OpCodes.Ldind_R8;
            typeToOpcode[typeof(float)] = OpCodes.Ldind_R4;
        }


        /// <summary>
        ///     Gets the property info.
        /// </summary>
        /// <param name="target">The target type.</param>
        /// <returns></returns>
        protected PropertyInfo GetPropertyInfo(Type target)
        {
            PropertyInfo propertyInfo = null;

            propertyInfo = target.GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (propertyInfo == null)
            {
                if (target.IsInterface)
                    foreach (Type interfaceType in target.GetInterfaces())
                    {
                        // Get propertyinfo and if found the break out of loop
                        propertyInfo = GetPropertyInfo(interfaceType);
                        if (propertyInfo != null) break;
                    }
                else
                    propertyInfo = target.GetProperty(propertyName);
            }

            return propertyInfo;
        }

        /// <summary>
        ///     Get the null value for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object GetNullInternal(Type type)
        {
            if (type.IsValueType)
            {
                if (type.IsEnum) return GetNullInternal(Enum.GetUnderlyingType(type));

                if (type.IsPrimitive)
                {
                    if (type == typeof(int)) return 0;
                    if (type == typeof(double)) return (double) 0;
                    if (type == typeof(short)) return (short) 0;
                    if (type == typeof(sbyte)) return (sbyte) 0;
                    if (type == typeof(long)) return (long) 0;
                    if (type == typeof(byte)) return (byte) 0;
                    if (type == typeof(ushort)) return (ushort) 0;
                    if (type == typeof(uint)) return (uint) 0;
                    if (type == typeof(ulong)) return (ulong) 0;
                    if (type == typeof(ulong)) return (ulong) 0;
                    if (type == typeof(float)) return (float) 0;
                    if (type == typeof(bool)) return false;
                    if (type == typeof(char)) return '\0';
                }
                else
                {
                    //DateTime : 01/01/0001 00:00:00
                    //TimeSpan : 00:00:00
                    //Guid : 00000000-0000-0000-0000-000000000000
                    //Decimal : 0

                    if (type == typeof(DateTime)) return DateTime.MinValue;
                    if (type == typeof(decimal)) return 0m;
                    if (type == typeof(Guid)) return Guid.Empty;
                    if (type == typeof(TimeSpan)) return new TimeSpan(0, 0, 0);
                }
            }

            return null;
        }
    }
}