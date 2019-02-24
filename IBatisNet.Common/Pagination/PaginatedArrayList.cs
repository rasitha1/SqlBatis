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

#region Imports

using System;
using System.Collections;

#endregion


namespace IBatisNet.Common.Pagination
{
    /// <summary>
    ///     Summary description for PaginatedArrayList.
    /// </summary>
    public class PaginatedArrayList : IPaginatedList
    {
        #region Properties

        /// <summary>
        /// </summary>
        public bool IsEmpty => (_page.Count == 0);

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _page.GetEnumerator();
        }

        #endregion

        #region Fields

        private static readonly ArrayList _emptyList = new ArrayList();

        private readonly IList _list;
        private IList _page;

        #endregion

        #region Constructor (s) / Destructor

        /// <summary>
        /// </summary>
        /// <param name="pageSize"></param>
        public PaginatedArrayList(int pageSize)
        {
            PageSize = pageSize;
            PageIndex = 0;
            _list = new ArrayList();
            Repaginate();
        }

        /// <summary>
        /// </summary>
        /// <param name="initialCapacity"></param>
        /// <param name="pageSize"></param>
        public PaginatedArrayList(int initialCapacity, int pageSize)
        {
            PageSize = pageSize;
            PageIndex = 0;
            _list = new ArrayList(initialCapacity);
            Repaginate();
        }

        /// <summary>
        /// </summary>
        /// <param name="c"></param>
        /// <param name="pageSize"></param>
        public PaginatedArrayList(ICollection c, int pageSize)
        {
            PageSize = pageSize;
            PageIndex = 0;
            _list = new ArrayList(c);
            Repaginate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        private void Repaginate()
        {
            if (_list.Count == 0)
            {
                _page = _emptyList;
            }
            else
            {
                int start = PageIndex * PageSize;
                int end = start + PageSize - 1;
                if (end >= _list.Count) end = _list.Count - 1;
                if (start >= _list.Count)
                {
                    PageIndex = 0;
                    Repaginate();
                }
                else if (start < 0)
                {
                    PageIndex = _list.Count / PageSize;
                    if (_list.Count % PageSize == 0) PageIndex--;
                    Repaginate();
                }
                else
                {
                    _page = SubList(_list, start, end + 1);
                }
            }
        }


        /// <summary>
        ///     Provides a view of the IList pramaeter
        ///     from the specified position <paramref name="fromIndex" />
        ///     to the specified position <paramref name="toIndex" />.
        /// </summary>
        /// <param name="list">The IList elements.</param>
        /// <param name="fromIndex">Starting position for the view of elements. </param>
        /// <param name="toIndex">Ending position for the view of elements. </param>
        /// <returns>
        ///     A view of list.
        /// </returns>
        /// <remarks>
        ///     The list that is returned is just a view, it is still backed
        ///     by the orignal list.  Any changes you make to it will be
        ///     reflected in the orignal list.
        /// </remarks>
        private IList SubList(IList list, int fromIndex, int toIndex)
        {
            return ((ArrayList) list).GetRange(fromIndex, toIndex - fromIndex);
        }

        #endregion

        #region IPaginatedList Members

        /// <summary>
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// </summary>
        public bool IsFirstPage => (PageIndex == 0);

        /// <summary>
        /// </summary>
        public bool IsMiddlePage => !(IsFirstPage || IsLastPage);

        /// <summary>
        /// </summary>
        public bool IsLastPage => _list.Count - ((PageIndex + 1) * PageSize) < 1;

        /// <summary>
        /// </summary>
        public bool IsNextPageAvailable => !IsLastPage;

        /// <summary>
        /// </summary>
        public bool IsPreviousPageAvailable => !IsFirstPage;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public bool NextPage()
        {
            if (IsNextPageAvailable)
            {
                PageIndex++;
                Repaginate();
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public bool PreviousPage()
        {
            if (IsPreviousPageAvailable)
            {
                PageIndex--;
                Repaginate();
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="pageIndex"></param>
        public void GotoPage(int pageIndex)
        {
            PageIndex = pageIndex;
            Repaginate();
        }

        /// <summary>
        /// </summary>
        public int PageIndex { get; private set; }

        #endregion

        #region IList Members

        /// <summary>
        /// </summary>
        public bool IsReadOnly => _list.IsReadOnly;

        /// <summary>
        /// </summary>
        public object this[int index]
        {
            get => _page[index];
            set
            {
                _list[index] = value;
                Repaginate();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            Repaginate();
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(int index, object value)
        {
            _list.Insert(index, value);
            Repaginate();
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        public void Remove(object value)
        {
            _list.Remove(value);
            Repaginate();
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(object value)
        {
            return _page.Contains(value);
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            _list.Clear();
            Repaginate();
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(object value)
        {
            return _page.IndexOf(value);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(object value)
        {
            int i = _list.Add(value);
            Repaginate();
            return i;
        }

        /// <summary>
        /// </summary>
        public bool IsFixedSize => _list.IsFixedSize;

        #endregion

        #region ICollection Members

        /// <summary>
        /// </summary>
        public bool IsSynchronized => _page.IsSynchronized;

        /// <summary>
        /// </summary>
        public int Count => _page.Count;

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            _page.CopyTo(array, index);
        }

        /// <summary>
        /// </summary>
        public object SyncRoot => _page.SyncRoot;

        #endregion

        #region IEnumerator Members

        /// <summary>
        ///     Sets the enumerator to its initial position,
        ///     which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            _page.GetEnumerator().Reset();
        }

        /// <summary>
        ///     Gets the current element in the page.
        /// </summary>
        public object Current => _page.GetEnumerator().Current;

        /// <summary>
        ///     Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element;
        ///     false if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            return _page.GetEnumerator().MoveNext();
        }

        #endregion
    }
}