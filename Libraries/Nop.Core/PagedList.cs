using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Core
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        //实现IpageList
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItemCount { get; private set; }
        public int TotalPageCount { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPageCount); }
        }
        //---end实现IpageList


        /// <summary>
        /// Ctor (paging in performed inside)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            Init(source, pageIndex, pageSize);
        }

        /// <summary>
        /// Ctor (already paged soure is passed)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            Init(source, pageIndex, pageSize, totalCount);
        }
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        private void Init(IQueryable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (pageSize <= 0)
                throw new ArgumentException("pageSize must be greater than zero");

            TotalItemCount = totalCount ?? source.Count();
            TotalPageCount = TotalItemCount / pageSize;

            if (TotalItemCount % pageSize > 0)
                TotalPageCount++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            List<T> result;
            if (!totalCount.HasValue)
            {
                result = TotalItemCount > 0 ? source.Skip(pageIndex * pageSize).Take(pageSize).ToList() : null;
            }
            else
            {
                result = source.ToList();
            }
            if (result != null)
            {
                AddRange(result);
            }
        }
        /// <summary>
        /// Ctor (paging in performed inside)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            Init(source, pageIndex, pageSize);
        }

        /// <summary>
        /// Ctor (already paged soure is passed)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            Init(source, pageIndex, pageSize, totalCount);
        }

       
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        private void Init(IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (pageSize <= 0)
                throw new ArgumentException("pageSize must be greater than zero");

            TotalItemCount = totalCount ?? source.Count();
            TotalPageCount = TotalItemCount / pageSize;

            if (TotalItemCount % pageSize > 0)
                TotalPageCount++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            var result = TotalItemCount>0 ? source.Skip(pageIndex * pageSize).Take(pageSize).ToList() : null;
            if (result != null)
            {
                AddRange(result);
            }
        }

    }
}
