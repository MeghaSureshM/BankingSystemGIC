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
            // Arrange
            string input = "20230101 123456 D 1000";

            // Act
            processData.ProcessTransaction(input);

            // Assert
            var account = processData.GetAccount("123456");
            Assert.IsNotNull(account);
            Assert.AreEqual(1000, account.Balance);
        }

        [TestMethod]
        public void ProcessTransaction_ValidWithdrawalTransaction_UpdatesAccountBalance()
        {
            // Arrange
            string depositInput = "20230101 123456 D 1000";
            string withdrawalInput = "20230102 123456 W 500";
            processData.ProcessTransaction(depositInput);

            // Act
            processData.ProcessTransaction(withdrawalInput);

            // Assert
            var account = processData.GetAccount("123456");
            Assert.IsNotNull(account);
            Assert.AreEqual(500, account.Balance);
        }

        [TestMethod]
        public void ProcessTransaction_InsufficientBalance_ShowsErrorMessage()
        {
            // Arrange
            string input = "20230101 123456 W 1000";

            // Act
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

                // Assert
                Assert.AreEqual("Invalid transaction: Insufficient balance.", result);
            }
        }

        [TestMethod]
        public void DefineInterest_ValidInterestRule_AddsOrUpdatesInterestRule()
        {
            // Arrange
            string input = "20230101 Rule1 5.0";

            // Act
            processData.DefineInterest(input);

            // Assert
            var rule = processData.GetInterestRule("Rule1");
            Assert.IsNotNull(rule);
            Assert.AreEqual(5.0, rule.InterestRate);
        }

        [TestMethod]
        public void DefineInterest_InvalidInterestRate_ShowsErrorMessage()
        {
            // Arrange
            string input = "20230101 Rule1 -5.0";

            // Act
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

                // Assert
                Assert.AreEqual("Error: Interest rate should be greater than 0 and less than 100", result);
            }
        }

        [TestMethod]
        public void PrintStatement_ValidAccountAndDate_PrintsStatement()
        {
            processData = new ProcessData();
            // Arrange
            string depositInput = "20230101 123456 D 1000";
            string withdrawalInput = "20230102 123456 W 500";
            processData.ProcessTransaction(depositInput);
            processData.ProcessTransaction(withdrawalInput);
            string input = "123456 202301";

            // Act
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

                // Assert
                Assert.IsTrue(result.Contains("Account: 123456"));
                Assert.IsTrue(result.Contains("| 20230101 | 20230101-01 | D | 1000.00 | 1000.00 |"));
                Assert.IsTrue(result.Contains("| 20230102 | 20230102-01 | W | 500.00 | 500.00 |"));
            }
        }

        [TestMethod]
        public void CalculateInterest_ValidAccountAndDate_CalculatesInterest()
        {
            // Arrange
            string depositInput = "20230101 123456 D 1000";
            string interestRuleInput = "20230101 Rule1 5.0";
            processData.ProcessTransaction(depositInput);
            processData.DefineInterest(interestRuleInput);
            string input = "123456 202301";

            // Act
            processData.CalculateInterest("123456", "202301");

            // Assert
            var account = processData.GetAccount("123456");
            Assert.IsNotNull(account);
            Assert.IsTrue(account.Balance > 1000); // Interest should be added
        }
    }
}