using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class EnumData
    {
        public enum PaymentType
        {
            Deposit = 1,
            ActualPayment,
            Rejected
        }

        public enum PaymentMethod
        {
            Cash = 1,
            Cheque,
            BankTransfer
        }

        public enum ExpenseType
        {
            Salaries = 1,
        }
    }
}
