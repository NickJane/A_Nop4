using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItemCount { get; }
        int TotalPageCount { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
