using BankingSystem.Modals;
using System.Linq.Expressions;
using System.Text;

namespace BankingSystem.Actions
{
   public class ProcessData
    {
        private Dictionary<string, int> transactionCountByDate = new Dictionary<string, int>();
        private List<BankTransaction> transactions = new List<BankTransaction>();
        private List<InterestRule> interestRules = new List<InterestRule>();
        private List<Account> accounts = new List<Account>();

        public void ProcessTransaction(string input)
        {
            try
            {
                List<string> result = new List<string>(input.Split(' '));
                string accountNumber = result[1];
                DateTime transactionDate = DateTime.ParseExact(result[0], "yyyyMMdd", null);
                string transactionType = result[2];
                double amount = Double.Parse(result[3]);
                if (transactionType != "W" && transactionType != "D")
                {
                    Console.WriteLine("Invalid transaction type.");
                    return;
                }

                Account account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
                if (account == null)
                {
                    account = new Account(accountNumber);
                    accounts.Add(account);
                }

                string dateKey = transactionDate.ToString("yyyyMMdd");
                if (!transactionCountByDate.ContainsKey(dateKey))
                {
                    transactionCountByDate[dateKey] = 0;
                }
                transactionCountByDate[dateKey]++;
                string transactionId = $"{dateKey}-{transactionCountByDate[dateKey]:D2}";

                BankTransaction transaction = new BankTransaction(transactionDate, accountNumber, transactionType, amount)
                {
                    TransactionId = transactionId
                };

                if (transactionType == "W" && account.Balance < amount)
                {
                    Console.WriteLine("Invalid transaction: Insufficient balance.");
                }
                else
                {
                    transactions.Add(transaction);
                    account.Balance = transactionType == "W" ? account.Balance - amount : account.Balance + amount;
                    transaction.Balance = account.Balance;
                    var accountTransactions = transactions
                                        .Where(t => t.AccountNumber == accountNumber)
                                        .ToList();
                    var summary = new StringBuilder();
                    summary.AppendLine($"Account: {accountNumber}");
                    summary.AppendLine("| Date     | Txn Id      | Type | Amount |");
                    foreach (var trans in accountTransactions)
                    {
                        summary.AppendLine($"| {trans.TransactionDate:yyyyMMdd} | {trans.TransactionId} | {trans.TransactionType} | {trans.Amount:F2} |");
                    }
                    Console.WriteLine(summary.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Invalid input format.");
            }
        }

        public void DefineInterest(string input)
        {
            try
            {
                List<string> result = new List<string>(input.Split(' '));

                if (Double.Parse(result[2]) > 0)
                {
                    var date = DateTime.ParseExact(result[0], "yyyyMMdd", null);
                    InterestRule interestRule = new InterestRule(date, result[1], Double.Parse(result[2]));

                    var existingRule = interestRules
                        .FirstOrDefault(t => t.RuleDate.ToString("yyyyMMdd") == date.ToString("yyyyMMdd"));

                    if (existingRule != null)
                    {
                        
                        var index = interestRules.IndexOf(existingRule);
                        interestRules[index] = interestRule;
                    }
                    else
                    {
                        interestRules.Add(interestRule);
                    }
                    var AccountInterestRules = interestRules.ToList();
                    var summary = new StringBuilder();
                    summary.AppendLine($"Interest Rules:");
                    summary.AppendLine("| Date     | RuleId| Rate |");
                    foreach (var rule in AccountInterestRules)
                    {
                        summary.AppendLine($"| {rule.RuleDate:yyyyMMdd} | {rule.RuleId} | {rule.InterestRate:F1} |");
                    }
                    Console.WriteLine(summary.ToString());
                }
                else
                {
                    Console.WriteLine("Error: Interest rate should be greater than 0 and less than 100\r\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Invalid input format.");
            }
        }

        public void PrintStatement(string input)
        {
            try
            {
                List<string> result = new List<string>(input.Split(' '));
                string accountNumber = result[0];
                string date = result[1];
                CalculateInterest(accountNumber, date);
                var accountTransactions = transactions
                                         .Where(t => t.AccountNumber == accountNumber && t.TransactionDate.ToString("yyyyMM") == date)
                                         .ToList();
                var summary = new StringBuilder();
                summary.AppendLine($"Account: {accountNumber}");
                summary.AppendLine("| Date     | Txn Id      | Type | Amount | Balance |");
                foreach (var transaction in accountTransactions)
                {
                    summary.AppendLine($"| {transaction.TransactionDate:yyyyMMdd} | {transaction.TransactionId} | {transaction.TransactionType} | {transaction.Amount:F2} | {transaction.Balance:F2} |");
                }
               
                Console.WriteLine(summary.ToString());
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Invalid input format.");
            }
        }

        public void CalculateInterest(string accountNumber, string date)
        {
            try
            {
                DateTime parsedDate = DateTime.ParseExact(date, "yyyyMM", null);
                DateTime start = new DateTime(parsedDate.Year, parsedDate.Month, 1);
                DateTime end = new DateTime(parsedDate.Year, parsedDate.Month, DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month));

                var accountTransactions = transactions
                                         .Where(t => t.AccountNumber == accountNumber && t.TransactionDate >= start && t.TransactionDate <= end)
                                         .OrderBy(t => t.TransactionDate)
                                         .ToList();

                if (!accountTransactions.Any())
                {
                    Console.WriteLine("No transactions found for the given account and date.");
                    return;
                }

                DateTime firstTransactionDate = accountTransactions.First().TransactionDate;
                DateTime lastTransactionDate = accountTransactions.Last().TransactionDate;

                var sortedInterestRules = interestRules.OrderBy(r => r.RuleDate).ToList();
                var interestPeriods = new List<(DateTime Start, DateTime End, double Balance, InterestRule Rule)>();

                DateTime currentStart = firstTransactionDate < start ? start : firstTransactionDate;
                double currentBalance = accountTransactions.First().Balance;
                InterestRule currentRule = null;

                foreach (var rule in sortedInterestRules)
                {
                    if (rule.RuleDate > end)
                        break;

                    if (currentRule != null)
                    {
                        interestPeriods.Add((currentStart, rule.RuleDate > end ? end : rule.RuleDate, currentBalance, currentRule));
                    }

                    currentStart = rule.RuleDate < currentStart ? currentStart : rule.RuleDate;
                    currentRule = rule;
                }

                foreach (var transaction in accountTransactions)
                {
                    currentBalance = transaction.Balance;
                }

                if (currentRule != null)
                {
                    interestPeriods.Add((currentStart, end, currentBalance, currentRule));
                }

                double totalInterest = 0;
                const int daysInYear = 365;

                foreach (var period in interestPeriods)
                {
                    int numOfDays = (period.End - period.Start).Days + 1;
                    double interest = period.Balance * (period.Rule.InterestRate / 100) * numOfDays / daysInYear;
                    totalInterest += interest;
                }

              
                DateTime lastDayOfMonth = new DateTime(parsedDate.Year, parsedDate.Month, DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month));
                var existingInterestTransaction = transactions.FirstOrDefault(t => t.AccountNumber == accountNumber && t.TransactionDate == lastDayOfMonth && t.TransactionType == "I");
                var CurrentAccount = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (existingInterestTransaction != null)
                {
                    
                    existingInterestTransaction.Amount = totalInterest;
                    existingInterestTransaction.Balance = currentBalance - existingInterestTransaction.Amount + totalInterest;
                    CurrentAccount.Balance = existingInterestTransaction.Balance;
                }
                else
                {
                   
                    string interestTransactionId = "";
                    BankTransaction interestTransaction = new BankTransaction(lastDayOfMonth, accountNumber, "I", totalInterest)
                    {
                        TransactionId = interestTransactionId,
                        Balance = currentBalance + totalInterest
                    };
                    transactions.Add(interestTransaction);
                    CurrentAccount.Balance = interestTransaction.Balance;
                    accounts.Add(CurrentAccount);

                    return;
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Error: Invalid input format.");
            }
          
        }

        
        public Account GetAccount(string accountNumber)
        {
            return accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }

        public InterestRule GetInterestRule(string ruleId)
        {
            return interestRules.FirstOrDefault(r => r.RuleId == ruleId);
        }
    }
}
