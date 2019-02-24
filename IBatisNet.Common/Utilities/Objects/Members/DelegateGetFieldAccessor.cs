#region Apache Notice

/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-09 20:24:53 +0200 (dim., 09 avr. 2006) $
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
using System.Reflection;
using System.Reflection.Emit;

namespace IBatisNet.Common.Utilities.Objects.Members
{
    /// <summary>
    ///     The <see cref="DelegateFieldGetAccessor" /> class defines a field get accessor and
    ///     provides <c>Reflection.Emit</c>-generated <see cref="IGet" />
    ///     via the new DynamicMethod (.NET V2).
    /// </summary>
    public sealed class DelegateFieldGetAccessor : BaseAccessor, IGetAccessor
    {
        /// <summary>
        ///     The field name
        /// </summary>
        private readonly string _fieldName = string.Empty;

        /// <summary>
        ///     The class parent type
        /// </summary>
        private readonly Type _fieldType;

        private readonly GetValue _get;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:DelegateFieldGetAccessor" /> class
        ///     for field get access via DynamicMethod.
        /// </summary>
        /// <param name="targetObjectType">Type of the target object.</param>
        /// <param name="fieldName">Name of the field.</param>
        public DelegateFieldGetAccessor(Type targetObjectType, string fieldName)
        {
            // this.targetType = targetObjectType;
            _fieldName = fieldName;

            FieldInfo fieldInfo = targetObjectType.GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Make sure the field exists
            if (fieldInfo == null)
            {
                throw new NotSupportedException(
                    string.Format("Field \"{0}\" does not exist for type "
                                  + "{1}.", fieldName, targetObjectType));
            }

            _fieldType = fieldInfo.FieldType;
            nullInternal = GetNullInternal(_fieldType);

            DynamicMethod dynamicMethodGet = new DynamicMethod("GetImplementation", typeof(object),
                new[] {typeof(object)}, GetType().Module, false);
            ILGenerator ilgen = dynamicMethodGet.GetILGenerator();

            // Emit the IL for get access. 

            // We need a reference to the current instance (stored in local argument index 0) 
            // so Ldfld can load from the correct instance (this one).
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldfld, fieldInfo);
            if (_fieldType.IsValueType) ilgen.Emit(OpCodes.Box, fieldInfo.FieldType);
            ilgen.Emit(OpCodes.Ret);
            _get = (GetValue) dynamicMethodGet.CreateDelegate(typeof(GetValue));
        }

        #region IGet Members

        /// <summary>
        ///     Gets the field value from the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Property value.</returns>
        public object Get(object target)
        {
            return _get(target);
        }

        #endregion

        private delegate object GetValue(object instance);

        #region IAccessor Members

        /// <summary>
        ///     Gets the field's name.
        /// </summary>
        /// <value></value>
        public string Name => _fieldName;

        /// <summary>
        ///     Gets the field's type.
        /// </summary>
        /// <value></value>
        public Type MemberType => _fieldType;

        #endregion
    }
}