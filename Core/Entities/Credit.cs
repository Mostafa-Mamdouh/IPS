using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public class Credit : BaseEntity
    {
        public Credit()
        {
        }

        public decimal BankCredit { get; set; }

        public decimal CashCredit { get; set; }

        public int Year { get; set; }

        public AppUser CreateUser { get; set; }
        public AppUser UpdateUser { get; set; }
    }
}
