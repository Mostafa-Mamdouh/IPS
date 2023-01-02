using System;

namespace Core.Specifications
{
    public class ExpenseSpecParams
    {
        private const int MaxPageSize = int.MaxValue;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public DateTime? TransactionFromDate { get; set; }
        public DateTime? TransactionToDate { get; set; }

        public string Sort { get; set; }
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}