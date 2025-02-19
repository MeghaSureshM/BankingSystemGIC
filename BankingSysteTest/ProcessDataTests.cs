using BankingSystem.Actions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace BankingSysteTest
{
    [TestClass]
    public sealed class ProcessDataTests
    {
        private ProcessData processData;

        [TestInitialize]
        public void Setup()
        {
            processData = new ProcessData();
        }

        [TestMethod]
        public void ProcessTransaction_ValidDepositTransaction_UpdatesAccountBalance()
        {
            string input = "20230101 123456 D 1000";

            
            processData.ProcessTransaction(input);

           
            var account = processData.GetAccount("123456");
            Assert.IsNotNull(account);
            Assert.AreEqual(1000, account.Balance);
        }

        [TestMethod]
        public void ProcessTransaction_ValidWithdrawalTransaction_UpdatesAccountBalance()
        {
            string depositInput = "20230101 123456 D 1000";
            string withdrawalInput = "20230102 123456 W 500";
            processData.ProcessTransaction(depositInput);

          
            processData.ProcessTransaction(withdrawalInput);

            
            var account = processData.GetAccount("123456");
            Assert.IsNotNull(account);
            Assert.AreEqual(500, account.Balance);
        }

        [TestMethod]
        public void ProcessTransaction_InsufficientBalance_ShowsErrorMessage()
        {
            processData=new ProcessData();
            string input = "20230101 123456 W 1000";

            
            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    processData.ProcessTransaction(input);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
                var result = sw.ToString().Trim();

                
                Assert.AreEqual("Invalid transaction: Insufficient balance.", result);
            }
        }

        [TestMethod]
        public void DefineInterest_ValidInterestRule_AddsOrUpdatesInterestRule()
        {
            processData = new ProcessData();
            string input = "20230101 Rule1 5.0";

           
            processData.DefineInterest(input);

            
            var rule = processData.GetInterestRule("Rule1");
            Assert.IsNotNull(rule);
            Assert.AreEqual(5.0, rule.InterestRate);
        }

        [TestMethod]
        public void DefineInterest_InvalidInterestRate_ShowsErrorMessage()
        {
            processData = new ProcessData();
            string input = "20230101 Rule1 -5.0";


            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    processData.DefineInterest(input);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
                var result = sw.ToString().Trim();


                Assert.AreEqual("Error: Interest rate should be greater than 0 and less than 100", result);
            }
        }

        [TestMethod]
        public void PrintStatement_ValidAccountAndDate_PrintsStatement()
        {
           ProcessData processData = new ProcessData();

            string depositInput = "20230101 123456 D 1000";
            string withdrawalInput = "20230102 123456 W 500";
            processData.ProcessTransaction(depositInput);
            processData.ProcessTransaction(withdrawalInput);
            string input = "123456 202301";


            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    processData.PrintStatement(input);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
                var result = sw.ToString().Trim();


                Assert.IsTrue(result.Contains("Account: 123456"));
                Assert.IsTrue(result.Contains("| 20230101 | 20230101-01 | D | 1000.00 | 1000.00 |"));
                Assert.IsTrue(result.Contains("| 20230102 | 20230102-01 | W | 500.00 | 500.00 |"));
            }
        }

        [TestMethod]
        public void CalculateInterest_ValidAccountAndDate_CalculatesInterest()
        {
            processData = new ProcessData();
            string depositInput = "20230101 123456 D 1000";
            string interestRuleInput = "20230101 Rule1 5.0";
            processData.ProcessTransaction(depositInput);
            processData.DefineInterest(interestRuleInput);
            string input = "123456 202301";

           
            processData.CalculateInterest("123456", "202301");

            var account = processData.GetAccount("123456");
            Assert.IsNotNull(account);
            Assert.IsTrue(account.Balance > 1000); // Interest should be added
        }

        [TestMethod]
        public void ProcessTransaction_InvalidTransactionType_ShowsErrorMessage()
        {
            processData = new ProcessData();
            string input = "20230101 123456 X 1000";

            
            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    processData.ProcessTransaction(input);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
                var result = sw.ToString().Trim();

                Assert.AreEqual("Invalid transaction type.", result);
            }
        }

        [TestMethod]
        public void ProcessTransaction_InvalidInputFormat_ShowsErrorMessage()
        {
            processData = new ProcessData();
            string input = "invalidinputformat";

            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    processData.ProcessTransaction(input);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
                var result = sw.ToString().Trim();

                Assert.AreEqual("Error: Invalid input format.", result);
            }
        }
    


     
       


        [TestMethod]
        public void ProcessTransaction_ValidDepositAndWithdrawal_UpdatesAccountBalance()
        {
            
            string depositInput = "20230101 123456 D 1000";
            string withdrawalInput = "20230102 123456 W 500";
            processData.ProcessTransaction(depositInput);

          
            processData.ProcessTransaction(withdrawalInput);

      
            var account = processData.GetAccount("123456");
            Assert.IsNotNull(account);
            Assert.AreEqual(500, account.Balance);
        }

        [TestMethod]
        public void ProcessTransaction_MultipleTransactions_UpdatesAccountBalance()
        {
         
            string depositInput1 = "20230101 123456 D 1000";
            string withdrawalInput1 = "20230102 123456 W 500";
            string depositInput2 = "20230103 123456 D 200";
            processData.ProcessTransaction(depositInput1);
            processData.ProcessTransaction(withdrawalInput1);

            
            processData.ProcessTransaction(depositInput2);

         
            var account = processData.GetAccount("123456");
            Assert.IsNotNull(account);
            Assert.AreEqual(700, account.Balance);
        }
    

        [TestMethod]
        public void DefineInterest_ValidInterestRule_UpdatesExistingRule()
        {
            processData = new ProcessData();
            string input1 = "20230101 Rule1 5.0";
            string input2 = "20230101 Rule1 6.0";

            processData.DefineInterest(input1);
            processData.DefineInterest(input2);

            var rule = processData.GetInterestRule("Rule1");
            Assert.IsNotNull(rule);
            Assert.AreEqual(6.0, rule.InterestRate);
        }

      

     
     

        [TestMethod]
        public void CalculateInterest_InvalidInputFormat_ShowsErrorMessage()
        {
            processData = new ProcessData();
            string accountNumber = "123456";
            string date = "invaliddateformat";

            using (var sw = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(sw);
                try
                {
                    processData.CalculateInterest(accountNumber, date);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
                var result = sw.ToString().Trim();

                Assert.AreEqual("Error: Invalid input format.", result);
            }
        }

    

    }
}