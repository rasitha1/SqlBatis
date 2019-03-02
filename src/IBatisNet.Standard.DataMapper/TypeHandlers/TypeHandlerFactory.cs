#region Apache Notice

/*****************************************************************************
 * $Header: $
 * $Revision: 501527 $
 * $Date: 2007-01-30 20:32:11 +0100 (mar., 30 janv. 2007) $
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
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using IBatisNet.Common.Logging;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.TypeHandlers.Nullables;

#endregion

namespace IBatisNet.DataMapper.TypeHandlers
{
    /// <summary>
    ///     Not much of a suprise, this is a factory class for TypeHandler objects.
    /// </summary>
    public class TypeHandlerFactory
    {
        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        public TypeHandlerFactory()
        {
            ITypeHandler handler = null;

            handler = new DBNullTypeHandler();
            Register(typeof(DBNull), handler);

            handler = new BooleanTypeHandler();
            Register(typeof(bool), handler); // key= "System.Boolean"

            handler = new ByteTypeHandler();
            Register(typeof(byte), handler);

            handler = new CharTypeHandler();
            Register(typeof(char), handler);

            handler = new DateTimeTypeHandler();
            Register(typeof(DateTime), handler);

            handler = new DecimalTypeHandler();
            Register(typeof(decimal), handler);

            handler = new DoubleTypeHandler();
            Register(typeof(double), handler);

            handler = new Int16TypeHandler();
            Register(typeof(short), handler);

            handler = new Int32TypeHandler();
            Register(typeof(int), handler);

            handler = new Int64TypeHandler();
            Register(typeof(long), handler);

            handler = new SingleTypeHandler();
            Register(typeof(float), handler);

            handler = new StringTypeHandler();
            Register(typeof(string), handler);

            handler = new GuidTypeHandler();
            Register(typeof(Guid), handler);

            handler = new TimeSpanTypeHandler();
            Register(typeof(TimeSpan), handler);

            handler = new ByteArrayTypeHandler();
            Register(typeof(byte[]), handler);

            handler = new ObjectTypeHandler();
            Register(typeof(object), handler);

            handler = new EnumTypeHandler();
            Register(typeof(Enum), handler);

            handler = new UInt16TypeHandler();
            Register(typeof(ushort), handler);

            handler = new UInt32TypeHandler();
            Register(typeof(uint), handler);

            handler = new UInt64TypeHandler();
            Register(typeof(ulong), handler);

            handler = new SByteTypeHandler();
            Register(typeof(sbyte), handler);

            handler = new NullableBooleanTypeHandler();
            Register(typeof(bool?), handler);

            handler = new NullableByteTypeHandler();
            Register(typeof(byte?), handler);

            handler = new NullableCharTypeHandler();
            Register(typeof(char?), handler);

            handler = new NullableDateTimeTypeHandler();
            Register(typeof(DateTime?), handler);

            handler = new NullableDecimalTypeHandler();
            Register(typeof(decimal?), handler);

            handler = new NullableDoubleTypeHandler();
            Register(typeof(double?), handler);

            handler = new NullableGuidTypeHandler();
            Register(typeof(Guid?), handler);

            handler = new NullableInt16TypeHandler();
            Register(typeof(short?), handler);

            handler = new NullableInt32TypeHandler();
            Register(typeof(int?), handler);

            handler = new NullableInt64TypeHandler();
            Register(typeof(long?), handler);

            handler = new NullableSingleTypeHandler();
            Register(typeof(float?), handler);

            handler = new NullableUInt16TypeHandler();
            Register(typeof(ushort?), handler);

            handler = new NullableUInt32TypeHandler();
            Register(typeof(uint?), handler);

            handler = new NullableUInt64TypeHandler();
            Register(typeof(ulong?), handler);

            handler = new NullableSByteTypeHandler();
            Register(typeof(sbyte?), handler);

            handler = new NullableTimeSpanTypeHandler();
            Register(typeof(TimeSpan?), handler);


            _unknownTypeHandler = new UnknownTypeHandler(this);
        }

        #endregion

        /// <summary>
        ///     Gets a named TypeAlias from the list of available TypeAlias
        /// </summary>
        /// <param name="name">The name of the TypeAlias.</param>
        /// <returns>The TypeAlias.</returns>
        internal TypeAlias GetTypeAlias(string name)
        {
            if (_typeAliasMaps.Contains(name))
                return (TypeAlias) _typeAliasMaps[name];
            return null;
        }

        /// <summary>
        ///     Gets the type object from the specific class name.
        /// </summary>
        /// <param name="className">The supplied class name.</param>
        /// <returns>
        ///     The correpsonding type.
        /// </returns>
        internal Type GetType(string className)
        {
            Type type = null;
            TypeAlias typeAlias = GetTypeAlias(className);

            if (typeAlias != null)
                type = typeAlias.Class;
            else
                type = TypeUtils.ResolveType(className);

            return type;
        }

        /// <summary>
        ///     Adds a named TypeAlias to the list of available TypeAlias.
        /// </summary>
        /// <param name="key">The key name.</param>
        /// <param name="typeAlias"> The TypeAlias.</param>
        internal void AddTypeAlias(string key, TypeAlias typeAlias)
        {
            if (_typeAliasMaps.Contains(key))
                throw new DataMapperException(" Alias name conflict occurred.  The type alias '" + key +
                                              "' is already mapped to the value '" + typeAlias.ClassName + "'.");
            _typeAliasMaps.Add(key, typeAlias);
        }

        #region Fields

        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IDictionary _typeHandlerMap = new HybridDictionary();
        private readonly ITypeHandler _unknownTypeHandler;

        private const string NULL = "_NULL_TYPE_";

        //(typeAlias name, type alias)
        private readonly IDictionary _typeAliasMaps = new HybridDictionary();

        #endregion

        #region Methods

        /// <summary>
        ///     Get a TypeHandler for a Type
        /// </summary>
        /// <param name="type">the Type you want a TypeHandler for</param>
        /// <returns>the handler</returns>
        public ITypeHandler GetTypeHandler(Type type)
        {
            return GetTypeHandler(type, null);
        }

        /// <summary>
        ///     Get a TypeHandler for a type
        /// </summary>
        /// <param name="type">the type you want a TypeHandler for</param>
        /// <param name="dbType">the database type</param>
        /// <returns>the handler</returns>
        public ITypeHandler GetTypeHandler(Type type, string dbType)
        {
            if (type.IsEnum)
                return GetPrivateTypeHandler(typeof(Enum), dbType);
            return GetPrivateTypeHandler(type, dbType);
        }

        /// <summary>
        ///     Get a TypeHandler for a type and a dbType type
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="dbType">the dbType type</param>
        /// <returns>the handler</returns>
        private ITypeHandler GetPrivateTypeHandler(Type type, string dbType)
        {
            IDictionary dbTypeHandlerMap = (IDictionary) _typeHandlerMap[type];
            ITypeHandler handler = null;

            if (dbTypeHandlerMap != null)
            {
                if (dbType == null)
                {
                    handler = (ITypeHandler) dbTypeHandlerMap[NULL];
                }
                else
                {
                    handler = (ITypeHandler) dbTypeHandlerMap[dbType];
                    if (handler == null) handler = (ITypeHandler) dbTypeHandlerMap[NULL];
                }

                if (handler == null)
                    throw new DataMapperException(string.Format("Type handler for {0} not registered.", type.Name));
            }

            return handler;
        }


        /// <summary>
        ///     Register (add) a type handler for a type
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="handler">the handler instance</param>
        public void Register(Type type, ITypeHandler handler)
        {
            Register(type, null, handler);
        }

        /// <summary>
        ///     Register (add) a type handler for a type and dbType
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="dbType">the dbType (optional, if dbType is null the handler will be used for all dbTypes)</param>
        /// <param name="handler">the handler instance</param>
        public void Register(Type type, string dbType, ITypeHandler handler)
        {
            HybridDictionary map = (HybridDictionary) _typeHandlerMap[type];
            if (map == null)
            {
                map = new HybridDictionary();
                _typeHandlerMap.Add(type, map);
            }

            if (dbType == null)
            {
                if (_logger.IsInfoEnabled)
                {
                    // notify the user that they are no longer using one of the built-in type handlers
                    ITypeHandler oldTypeHandler = (ITypeHandler) map[NULL];

                    if (oldTypeHandler != null)
                    {
                        // the replacement will always(?) be a CustomTypeHandler
                        CustomTypeHandler customTypeHandler = handler as CustomTypeHandler;

                        string replacement = string.Empty;

                        if (customTypeHandler != null)
                            replacement = customTypeHandler.Callback.ToString();
                        else
                            replacement = handler.ToString();

                        // should oldTypeHandler be checked if its a CustomTypeHandler and if so report the Callback property ???
                        _logger.Info("Replacing type handler [" + oldTypeHandler + "] with [" + replacement + "].");
                    }
                }

                map[NULL] = handler;
            }
            else
            {
                map.Add(dbType, handler);
            }
        }

        /// <summary>
        ///     When in doubt, get the "unknown" type handler
        /// </summary>
        /// <returns>if I told you, it would not be unknown, would it?</returns>
        public ITypeHandler GetUnkownTypeHandler()
        {
            return _unknownTypeHandler;
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsSimpleType(Type type)
        {
            bool result = false;
            if (type != null)
            {
                ITypeHandler handler = GetTypeHandler(type, null);
                if (handler != null) result = handler.IsSimpleType;
            }

            return result;
        }

        #endregion
    }
}