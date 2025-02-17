using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankingSystem.Modals
{
  public  class BankTransaction
    {
       public string? TransactionId { get; set; }
        public string AccountNumber { get; set; }

        public string TransactionType { get; set; }

        public double Amount { get; set; }
        public double Balance { get; set; }
        public DateTime TransactionDate { get; set; }

        public BankTransaction(DateTime transactionDate,string accountNumber, string transactionType, double amount )
        {
            TransactionId = TransactionDate.Date.ToString()+'-'+1;
            AccountNumber = accountNumber;
            TransactionType = transactionType;
            Amount = amount;
            //Balance = balance;
            TransactionDate = transactionDate;
        }

     


    }
}
