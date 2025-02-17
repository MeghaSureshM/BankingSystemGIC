using BankingSystem.Actions;
using BankingSystem.Modals;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Numerics;

namespace BankingSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        public void Run()
        {
            var processData = new ProcessData();
            var choice = "";
            Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
            do
            {
                Console.WriteLine("[T] Deposit");
                Console.WriteLine("[I] Define Interest Rules");
                Console.WriteLine("[P] Print Statement");
                Console.WriteLine("[Q] Quit");
                choice = Console.ReadLine();
                Console.WriteLine(choice);

                if (choice == null)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                    continue;
                }

                switch (choice.ToUpper())
                {
                    case "T":
                        Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format \n");
                        var transactionInput = Console.ReadLine();
                        if (transactionInput != null)
                        {
                            processData.ProcessTransaction(transactionInput);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                        }
                        break;
                    case "I":
                        Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format \n");
                        var interestInput = Console.ReadLine();
                        if (interestInput != null)
                        {
                            processData.DefineInterest(interestInput);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                        }
                        break;
                    case "P":
                        Console.WriteLine("Please enter account and month to generate the statement<Account> < Year >< Month >(or enter blank to go back to main menu:)\n");
                        var statementInput = Console.ReadLine();
                        if (statementInput != null)
                        {
                            processData.PrintStatement(statementInput);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                        }
                        break;
                    case "Q":
                        Console.WriteLine("Thank you for banking with us!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                Console.WriteLine("Is there anything else you'd like to do?");
            } while (choice.ToUpper() != "Q");
        }
    }
}
