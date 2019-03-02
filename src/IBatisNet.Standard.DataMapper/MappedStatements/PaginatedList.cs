#region Apache Notice

/*****************************************************************************
 * $Header: $
 * $Revision: 476843 $
 * $Date: 2006-11-19 17:07:45 +0100 (dim., 19 nov. 2006) $
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

using System;
using System.Collections;
using IBatisNet.Common.Pagination;
using IBatisNet.DataMapper.Exceptions;

namespace IBatisNet.DataMapper.MappedStatements
{
    /// <summary>
    ///     Summary description for PaginatedDataList.
    /// </summary>
    public class PaginatedList : IPaginatedList
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mappedStatement"></param>
        /// <param name="parameterObject"></param>
        /// <param name="pageSize"></param>
        public PaginatedList(IMappedStatement mappedStatement, object parameterObject, int pageSize)
        {
            _mappedStatement = mappedStatement;
            _parameterObject = parameterObject;
            PageSize = pageSize;
            PageIndex = 0;
            PageTo(0);
        }


        /// <summary>
        /// </summary>
        public bool IsEmpty => (_currentPageList.Count == 0);

        #region IEnumerable Members

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _currentPageList.GetEnumerator();
        }

        #endregion


        /// <summary>
        /// </summary>
        private void PageForward()
        {
            try
            {
                _prevPageList = _currentPageList;
                _currentPageList = _nextPageList;
                _nextPageList = GetList(PageIndex + 1, PageSize);
            }
            catch (DataMapperException e)
            {
                throw new DataMapperException("Unexpected error while repaginating paged list.  Cause: " + e.Message,
                    e);
            }
        }

        /// <summary>
        /// </summary>
        private void PageBack()
        {
            try
            {
                _nextPageList = _currentPageList;
                _currentPageList = _prevPageList;
                if (PageIndex > 0)
                    _prevPageList = GetList(PageIndex - 1, PageSize);
                else
                    _prevPageList = new ArrayList();
            }
            catch (DataMapperException e)
            {
                throw new DataMapperException("Unexpected error while repaginating paged list.  Cause: " + e.Message,
                    e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        private void SafePageTo(int index)
        {
            try
            {
                PageTo(index);
            }
            catch (DataMapperException e)
            {
                throw new DataMapperException("Unexpected error while repaginating paged list.  Cause: " + e.Message,
                    e);
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        public void PageTo(int index)
        {
            PageIndex = index;
            IList list = null;

            if (index < 1)
                list = GetList(PageIndex, PageSize * 2);
            else
                list = GetList(index - 1, PageSize * 3);

            if (list.Count < 1)
            {
                _prevPageList = new ArrayList();
                _currentPageList = new ArrayList();
                _nextPageList = new ArrayList();
            }
            else
            {
                if (index < 1)
                {
                    _prevPageList = new ArrayList();
                    if (list.Count <= PageSize)
                    {
                        _currentPageList = SubList(list, 0, list.Count);
                        _nextPageList = new ArrayList();
                    }
                    else
                    {
                        _currentPageList = SubList(list, 0, PageSize);
                        _nextPageList = SubList(list, PageSize, list.Count);
                    }
                }
                else
                {
                    if (list.Count <= PageSize)
                    {
                        _prevPageList = SubList(list, 0, list.Count);
                        _currentPageList = new ArrayList();
                        _nextPageList = new ArrayList();
                    }
                    else if (list.Count <= PageSize * 2)
                    {
                        _prevPageList = SubList(list, 0, PageSize);
                        _currentPageList = SubList(list, PageSize, list.Count);
                        _nextPageList = new ArrayList();
                    }
                    else
                    {
                        _prevPageList = SubList(list, 0, PageSize);
                        _currentPageList = SubList(list, PageSize, PageSize * 2);
                        _nextPageList = SubList(list, PageSize * 2, list.Count);
                    }
                }
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="localPageSize"></param>
        /// <returns></returns>
        private IList GetList(int index, int localPageSize)
        {
            bool isSessionLocal = false;

            ISqlMapSession session = _mappedStatement.SqlMap.LocalSession;

            if (session == null)
            {
                session = new SqlMapSession(_mappedStatement.SqlMap);
                session.OpenConnection();
                isSessionLocal = true;
            }

            IList list = null;
            try
            {
                list = _mappedStatement.ExecuteQueryForList(session, _parameterObject, (index) * PageSize,
                    localPageSize);
            }
            finally
            {
                if (isSessionLocal) session.CloseConnection();
            }

            return list;
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

        #region Fields

        private IList _prevPageList;
        private IList _currentPageList;
        private IList _nextPageList;

        private readonly IMappedStatement _mappedStatement;
        private readonly object _parameterObject;

        #endregion

        #region IPaginatedList Members

        /// <summary>
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsPreviousPageAvailable => (_prevPageList.Count > 0);

        /// <summary>
        /// </summary>
        public bool IsFirstPage => (PageIndex == 0);

        /// <summary>
        /// </summary>
        /// <param name="pageIndex"></param>
        public void GotoPage(int pageIndex)
        {
            SafePageTo(pageIndex);
        }

        /// <summary>
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public bool NextPage()
        {
            if (IsNextPageAvailable)
            {
                PageIndex++;
                PageForward();
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        public bool IsMiddlePage => !(IsFirstPage || IsLastPage);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public bool PreviousPage()
        {
            if (IsPreviousPageAvailable)
            {
                PageIndex--;
                PageBack();
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        public bool IsNextPageAvailable => (_nextPageList.Count > 0);

        /// <summary>
        /// </summary>
        public bool IsLastPage => (_nextPageList.Count < 1);

        #endregion

        #region IList Members

        /// <summary>
        /// </summary>
        public bool IsReadOnly => _currentPageList.IsReadOnly;

        /// <summary>
        /// </summary>
        public object this[int index]
        {
            get => _currentPageList[index];
            set => _currentPageList[index] = value;
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _currentPageList.RemoveAt(index);
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert(int index, object value)
        {
            _currentPageList.Insert(index, value);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        public void Remove(object value)
        {
            _currentPageList.Remove(value);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(object value)
        {
            return _currentPageList.Contains(value);
        }

        /// <summary>
        /// </summary>
        public void Clear()
        {
            _currentPageList.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf(object value)
        {
            return _currentPageList.IndexOf(value);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(object value)
        {
            return _currentPageList.Add(value);
        }

        /// <summary>
        /// </summary>
        public bool IsFixedSize => _currentPageList.IsFixedSize;

        #endregion

        #region ICollection Members

        /// <summary>
        /// </summary>
        public bool IsSynchronized => _currentPageList.IsSynchronized;

        /// <summary>
        /// </summary>
        public int Count => _currentPageList.Count;

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            _currentPageList.CopyTo(array, index);
        }

        /// <summary>
        /// </summary>
        public object SyncRoot => _currentPageList.SyncRoot;

        #endregion

        #region IEnumerator Members

        /// <summary>
        ///     Sets the enumerator to its initial position,
        ///     which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            _currentPageList.GetEnumerator().Reset();
        }

        /// <summary>
        ///     Gets the current element in the page.
        /// </summary>
        public object Current => _currentPageList.GetEnumerator().Current;

        /// <summary>
        ///     Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        ///     true if the enumerator was successfully advanced to the next element;
        ///     false if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            return _currentPageList.GetEnumerator().MoveNext();
        }

        #endregion
    }
}