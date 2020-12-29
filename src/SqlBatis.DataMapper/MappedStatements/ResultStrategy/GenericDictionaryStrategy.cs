using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using SqlBatis.DataMapper.Configuration.ResultMapping;
using SqlBatis.DataMapper.Scope;

namespace SqlBatis.DataMapper.MappedStatements.ResultStrategy
{
    /// <summary>
    /// <see cref="IResultStrategy"/> implementation when 
    /// a 'resultClass' attribute is specified and
    /// the type of the result object is dynamic.
    /// </summary>
    public sealed class GenericDictionaryStrategy : IResultStrategy
    {
        #region IResultStrategy Members

        /// <summary>
        /// Processes the specified <see cref="IDataReader"/> 
        /// when a 'resultClass' attribute is specified on the statement and
        /// the 'resultClass' attribute is a dynamic object.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="resultObject">The result object.</param>
        public object Process(RequestScope request, ref IDataReader reader, object resultObject)
        {
            var outObject = resultObject;
            var resultMap = request.CurrentResultMap as AutoResultMap;

            if (outObject == null)
            {
                outObject = resultMap.CreateInstanceOfResultClass();
            }

            var count = reader.FieldCount;
            var dictionary = (IDictionary<string,object>)outObject;
            for (int i = 0; i < count; i++)
            {
                var property = new ResultProperty();
                property.PropertyName = "value";
                property.ColumnIndex = i;
                property.TypeHandler = request.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(reader.GetFieldType(i));
                dictionary.Add(reader.GetName(i), property.GetDataBaseValue(reader));
            }

            return outObject;
        }

        #endregion
    }
}