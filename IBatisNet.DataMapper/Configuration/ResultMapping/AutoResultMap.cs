#region Apache Notice

/*****************************************************************************
 * $Revision: 450157 $
 * $LastChangedDate: 2007-02-21 21:23:49 +0100 (mer., 21 févr. 2007) $
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

#region Using

using System;
using System.Collections.Specialized;
using System.Data;
using System.Xml.Serialization;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.DataExchange;

#endregion

namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
    /// <summary>
    ///     Implementation of <see cref="IResultMap" /> interface for auto mapping
    /// </summary>
    public class AutoResultMap : IResultMap
    {
        [NonSerialized] private IDataExchange _dataExchange;

        [NonSerialized] private bool _isInitalized;

        [NonSerialized] private readonly ResultPropertyCollection _properties = new ResultPropertyCollection();

        [NonSerialized] private readonly Type _resultClass;

        [NonSerialized] private readonly IFactory _resultClassFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutoResultMap" /> class.
        /// </summary>
        /// <param name="resultClass">The result class.</param>
        /// <param name="resultClassFactory">The result class factory.</param>
        /// <param name="dataExchange">The data exchange.</param>
        public AutoResultMap(Type resultClass, IFactory resultClassFactory, IDataExchange dataExchange)
        {
            _resultClass = resultClass;
            _resultClassFactory = resultClassFactory;
            _dataExchange = dataExchange;
        }

        /// <summary>
        ///     Clones this instance.
        /// </summary>
        /// <returns></returns>
        public AutoResultMap Clone()
        {
            return new AutoResultMap(_resultClass, _resultClassFactory, _dataExchange);
        }

        /// <summary>
        ///     Create an instance of result class.
        /// </summary>
        /// <returns>An object.</returns>
        public object CreateInstanceOfResultClass()
        {
            if (_resultClass.IsPrimitive || _resultClass == typeof(string))
            {
                TypeCode typeCode = Type.GetTypeCode(_resultClass);
                return TypeUtils.InstantiatePrimitiveType(typeCode);
            }

            if (_resultClass.IsValueType)
            {
                if (_resultClass == typeof(DateTime))
                    return new DateTime();
                if (_resultClass == typeof(decimal))
                    return new decimal();
                if (_resultClass == typeof(Guid))
                    return Guid.Empty;
                if (_resultClass == typeof(TimeSpan))
                    return new TimeSpan(0);
                if (_resultClass.IsGenericType &&
                    typeof(Nullable<>).IsAssignableFrom(_resultClass.GetGenericTypeDefinition()))
                    return TypeUtils.InstantiateNullableType(_resultClass);
                throw new NotImplementedException("Unable to instanciate value type");
            }

            return _resultClassFactory.CreateInstance(null);
        }

        #region IResultMap Members

        /// <summary>
        ///     The GroupBy Properties.
        /// </summary>
        [XmlIgnore]
        public StringCollection GroupByPropertyNames =>
            throw new NotImplementedException("The property 'GroupByPropertyNames' is not implemented.");

        /// <summary>
        ///     The collection of ResultProperty.
        /// </summary>
        [XmlIgnore]
        public ResultPropertyCollection Properties => _properties;

        /// <summary>
        ///     The GroupBy Properties.
        /// </summary>
        /// <value></value>
        public ResultPropertyCollection GroupByProperties =>
            throw new NotImplementedException("The property 'GroupByProperties' is not implemented.");

        /// <summary>
        ///     The collection of constructor parameters.
        /// </summary>
        [XmlIgnore]
        public ResultPropertyCollection Parameters =>
            throw new NotImplementedException("The property 'Parameters' is not implemented.");

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is initalized.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is initalized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitalized
        {
            get => _isInitalized;
            set => _isInitalized = value;
        }

        /// <summary>
        ///     Identifier used to identify the resultMap amongst the others.
        /// </summary>
        /// <value></value>
        /// <example>GetProduct</example>
        public string Id => _resultClass.Name;


        /// <summary>
        ///     The output type class of the resultMap.
        /// </summary>
        /// <value></value>
        public Type Class => _resultClass;


        /// <summary>
        ///     Sets the IDataExchange
        /// </summary>
        /// <value></value>
        public IDataExchange DataExchange
        {
            set => _dataExchange = value;
        }


        /// <summary>
        ///     Create an instance Of result.
        /// </summary>
        /// <param name="parameters">
        ///     An array of values that matches the number, order and type
        ///     of the parameters for this constructor.
        /// </param>
        /// <returns>An object.</returns>
        public object CreateInstanceOfResult(object[] parameters)
        {
            return CreateInstanceOfResultClass();
        }

        /// <summary>
        ///     Set the value of an object property.
        /// </summary>
        /// <param name="target">The object to set the property.</param>
        /// <param name="property">The result property to use.</param>
        /// <param name="dataBaseValue">The database value to set.</param>
        public void SetValueOfProperty(ref object target, ResultProperty property, object dataBaseValue)
        {
            _dataExchange.SetData(ref target, property, dataBaseValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public IResultMap ResolveSubMap(IDataReader dataReader)
        {
            return this;
        }

        #endregion
    }
}