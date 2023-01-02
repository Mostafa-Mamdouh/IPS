using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class InventorySpecParams
    {
        private const int MaxPageSize = int.MaxValue;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? CategoryId { get; set; }
        public DateTime? CreateFromDate { get; set; }
        public DateTime? CreateToDate { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }

        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
