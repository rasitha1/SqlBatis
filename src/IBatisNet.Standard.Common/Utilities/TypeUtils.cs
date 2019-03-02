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
using System.Collections.Generic;
using IBatisNet.Common.Utilities.TypesResolver;

namespace IBatisNet.Common.Utilities
{
    /// <summary>
    ///     Helper methods with regard to type.
    /// </summary>
    /// <remarks>
    ///     <p>
    ///         Mainly for internal use within the framework.
    ///     </p>
    /// </remarks>
    public sealed class TypeUtils
    {
        #region Fields

        private static readonly ITypeResolver _internalTypeResolver = new CachedTypeResolver(new TypeResolver());

        #endregion

        #region Constructor (s) / Destructor

        /// <summary>
        ///     Creates a new instance of the <see cref="IBatisNet.Common.Utilities.TypeUtils" /> class.
        /// </summary>
        /// <remarks>
        ///     <p>
        ///         This is a utility class, and as such exposes no public constructors.
        ///     </p>
        /// </remarks>
        private TypeUtils()
        {
        }

        #endregion

        /// <summary>
        ///     Resolves the supplied type name into a <see cref="System.Type" />
        ///     instance.
        /// </summary>
        /// <param name="typeName">
        ///     The (possibly partially assembly qualified) name of a
        ///     <see cref="System.Type" />.
        /// </param>
        /// <returns>
        ///     A resolved <see cref="System.Type" /> instance.
        /// </returns>
        /// <exception cref="System.TypeLoadException">
        ///     If the type cannot be resolved.
        /// </exception>
        public static Type ResolveType(string typeName)
        {
            Type type = TypeRegistry.ResolveType(typeName);
            if (type == null) type = _internalTypeResolver.Resolve(typeName);
            return type;
        }

        /// <summary>
        ///     Instantiate a 'Primitive' Type.
        /// </summary>
        /// <param name="typeCode">a typeCode.</param>
        /// <returns>An object.</returns>
        public static object InstantiatePrimitiveType(TypeCode typeCode)
        {
            object resultObject = null;

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    resultObject = new bool();
                    break;
                case TypeCode.Byte:
                    resultObject = new byte();
                    break;
                case TypeCode.Char:
                    resultObject = new char();
                    break;
                case TypeCode.DateTime:
                    resultObject = new DateTime();
                    break;
                case TypeCode.Decimal:
                    resultObject = new decimal();
                    break;
                case TypeCode.Double:
                    resultObject = new double();
                    break;
                case TypeCode.Int16:
                    resultObject = new short();
                    break;
                case TypeCode.Int32:
                    resultObject = new int();
                    break;
                case TypeCode.Int64:
                    resultObject = new long();
                    break;
                case TypeCode.SByte:
                    resultObject = new sbyte();
                    break;
                case TypeCode.Single:
                    resultObject = new float();
                    break;
                case TypeCode.String:
                    resultObject = "";
                    break;
                case TypeCode.UInt16:
                    resultObject = new ushort();
                    break;
                case TypeCode.UInt32:
                    resultObject = new uint();
                    break;
                case TypeCode.UInt64:
                    resultObject = new ulong();
                    break;
            }

            return resultObject;
        }

        /// <summary>
        ///     Instantiate a Nullable Type.
        /// </summary>
        /// <param name="type">The nullable type.</param>
        /// <returns>An object.</returns>
        public static object InstantiateNullableType(Type type)
        {
            object resultObject = null;

            if (type == typeof(bool?))
                resultObject = false;
            else if (type == typeof(byte?))
                resultObject = byte.MinValue;
            else if (type == typeof(char?))
                resultObject = char.MinValue;
            else if (type == typeof(DateTime?))
                resultObject = DateTime.MinValue;
            else if (type == typeof(decimal?))
                resultObject = decimal.MinValue;
            else if (type == typeof(double?))
                resultObject = double.MinValue;
            else if (type == typeof(short?))
                resultObject = short.MinValue;
            else if (type == typeof(int?))
                resultObject = int.MinValue;
            else if (type == typeof(long?))
                resultObject = long.MinValue;
            else if (type == typeof(sbyte?))
                resultObject = sbyte.MinValue;
            else if (type == typeof(float?))
                resultObject = float.MinValue;
            else if (type == typeof(ushort?))
                resultObject = ushort.MinValue;
            else if (type == typeof(uint?))
                resultObject = uint.MinValue;
            else if (type == typeof(ulong?)) resultObject = ulong.MinValue;

            return resultObject;
        }

        /// <summary>
        ///     Determines whether the specified type is implement generic Ilist interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if the specified type is implement generic Ilist interface; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsImplementGenericIListInterface(Type type)
        {
            bool ret = false;

            if (!type.IsGenericType) ret = false;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>)) return true;

            Type[] interfaceTypes = type.GetInterfaces();
            foreach (Type interfaceType in interfaceTypes)
            {
                ret = IsImplementGenericIListInterface(interfaceType);
                if (ret) break;
            }

            return ret;
        }
    }
}