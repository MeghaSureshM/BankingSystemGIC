# AwesomeGIC Bank Application

Welcome to the AwesomeGIC Bank Application! This application allows you to perform various banking operations such as depositing money, defining interest rules, and printing account statements.

## Prerequisites

- .NET 8 SDK
- Visual Studio or any other C# IDE

## Getting Started

Follow these steps to run the application:

### 1. Clone the Repository

Clone the repository to your local machine using the following command:
https://github.com/MeghaSureshM/BankingSystemGIC.git

### 2. Open the Project

Open the project in Visual Studio or your preferred C# IDE.

### 3. Build the Solution

Build the solution to restore the necessary packages and compile the code.

### 4. Run the Application

Run the application by starting the `BankingSystem` project. You can do this by pressing `F5` or selecting `Debug > Start Debugging` from the menu.

### 5. Using the Application

Once the application is running, you will see the following menu:

You can perform the following operations:

- **Deposit**: Enter `T` and provide the transaction details in the format `<Date> <Account> <Type> <Amount>`.
- **Define Interest Rules**: Enter `I` and provide the interest rule details in the format `<Date> <RuleId> <Rate in %>`.
- **Print Statement**: Enter `P` and provide the account and month details in the format `<Account> <Year><Month>`.
- **Quit**: Enter `Q` to exit the application.

### 6. Running Tests

To run the tests, follow these steps:

1. Open the Test Explorer in Visual Studio by selecting `Test > Test Explorer` from the menu.
2. Build the solution to discover the tests.
3. Run the tests by clicking the `Run All` button in the Test Explorer.

### Project Structure

- `BankingSystem/Actions/ProcessData.cs`: Contains the main logic for processing transactions, defining interest rules, and printing statements.
- `BankingSystem/Program.cs`: Contains the entry point of the application and the main menu logic.
- `BankingSysteTest/ProcessDataTests.cs`: Contains the unit tests for the `ProcessData` class.

