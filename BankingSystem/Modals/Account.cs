using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Modals
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public Account(string AccNo)
        {
            AccountNumber = AccNo;
            Balance = 0;

        }
    }
}
