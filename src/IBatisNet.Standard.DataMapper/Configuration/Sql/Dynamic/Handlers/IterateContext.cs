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

#region Imports

using System;
using System.Collections;
using IBatisNet.DataMapper.Exceptions;

#endregion

namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic.Handlers
{
    /// <summary>
    ///     Summary description for IterateContext.
    ///     http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp01212002.asp
    ///     http://www.microsoft.com/mspress/books/sampchap/6173.asp
    ///     http://www.dur.ac.uk/barry.cornelius/java/a.taste.of.csharp/onefile/
    /// </summary>
    public sealed class IterateContext : IEnumerator
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="collection"></param>
        public IterateContext(object collection)
        {
            if (collection is ICollection)
            {
                _collection = (ICollection) collection;
            }
            else if (collection.GetType().IsArray)
            {
                object[] array = (object[]) collection;
                ArrayList list = new ArrayList();
                int lenght = array.Length;
                for (int i = 0; i < lenght; i++) list.Add(array[i]);
                _collection = list;
            }
            else
            {
                throw new DataMapperException("ParameterObject or property was not a Collection, Array or Iterator.");
            }

            IEnumerable enumerable = (IEnumerable) collection;
            IEnumerator enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext()) _items.Add(enumerator.Current);
            IDisposable disposable = enumerator as IDisposable;
            if (disposable != null) disposable.Dispose();
            Index = -1;
        }

        /// <summary>
        ///     Gets the index of the current element in the collection.
        /// </summary>
        public int Index { get; private set; } = -1;

        /// <summary>
        ///     Return true if the current element is the first.
        /// </summary>
        public bool IsFirst => (Index == 0);

        /// <summary>
        ///     Return true if the current element is the last.
        /// </summary>
        public bool IsLast => (Index == (_items.Count - 1));

        /// <summary>
        ///     Returns true if the iteration has more elements. (In other words, returns true
        ///     if next would return an element rather than throwing an exception.)
        /// </summary>
        public bool HasNext
        {
            get
            {
                if ((Index >= -1) && (Index < (_items.Count - 1)))
                    return true;
                return false;
            }
        }

        /// <summary>
        ///     Sets the enumerator to its initial position,
        ///     which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            Index = -1;
        }

        /// <summary>
        ///     Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        ///     True if the enumerator was successfully advanced to the next element;
        ///     False if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            Index++;
            if (Index == _items.Count)
                return false;

            return true;
        }

        /// <summary>
        ///     Gets the current element in the collection.
        /// </summary>
        public object Current => _items[Index];

        /// <summary>
        ///     Removes from the underlying collection the last element returned by the iterator.
        /// </summary>
        public void Remove()
        {
            if (_collection is IList)
                ((IList) _collection).Remove(Current);
            else if (_collection is IDictionary) ((IDictionary) _collection).Remove(Current);
        }

        #region Fields

        private readonly ICollection _collection;
        private readonly ArrayList _items = new ArrayList();

        #endregion
    }
}