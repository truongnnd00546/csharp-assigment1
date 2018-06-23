using HL_Bank.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace HL_Bank.view
{
    class Menu
    {
        private readonly AccountController controller = new AccountController();
        private readonly TransactionController transactionController = new TransactionController();

        public void GenerateDefaultMenu()
        {
            while (true)
            {
                Console.WriteLine("--------------WELCOME TO HUONG LY BANK--------------");
                Console.WriteLine("1. Register for free.");
                Console.WriteLine("2. Login.");
                Console.WriteLine("3. Exit.");
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Please enter you choice (1|2|3): ");
                var choice = ParseChoice.GetInputNumber();
                switch (choice)
                {
                    case 1:
                        Console.WriteLine(controller.AddAccount()
                            ? "Register success!"
                            : "Register fails. Please try again later.");
                        Console.WriteLine("Press enter to continue.");
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine(controller.CheckLogin()
                            ? "Login success! Welcome back " + Program.currentLoggedIn.Fullname + "!"
                            : "Login fails. Please try again later.");
                        Console.WriteLine("Press enter to continue.");
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.WriteLine("See you later.");
                        Environment.Exit(1);
                        break;
                }

                if (Program.currentLoggedIn != null)
                {
                    break;
                }
            }
        }

        public void GenerateCustomerMenu()
        {
            while (true)
            {
                Console.WriteLine("--------------HUONG LY BANK CUSTOMER MENU--------------");
                Console.WriteLine("Welcome back " + Program.currentLoggedIn.Fullname);
                Console.WriteLine("1. Check information.");
                Console.WriteLine("2. Withdraw.");
                Console.WriteLine("3. Deposit.");
                Console.WriteLine("4. Transfer.");
                Console.WriteLine("5. Transaction history.");
                Console.WriteLine("6. Logout.");
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("Please enter you choice (1|2|3|4|5|6): ");
                var choice = ParseChoice.GetInputNumber();
                switch (choice)
                {
                    case 1:
                        controller.ShowAccountInformation();
                        break;
                    case 2:
                        controller.Withdraw();
                        break;
                    case 3:
                        controller.Deposit();
                        break;
                    case 4:
                        controller.Transfer();
                        break;
                    case 5:
                        GenerateTransactionMenu();
                        break;
                    case 6:
                        Program.currentLoggedIn = null;
                        Console.WriteLine("See you again.");
                        break;
                }

                if (Program.currentLoggedIn == null)
                {
                    break;
                }
            }
        }

        public void GenerateTransactionMenu()
        {
            Console.WriteLine("---------Welcome to menu transaction history---------");
            Console.WriteLine("1. Get the last 5 times.");
            Console.WriteLine("2. Get the last 10 times.");
            Console.WriteLine("3. Take the time.");
            Console.WriteLine("4. Logout.");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Please enter your choice (1|2|3) ");
            var choice = ParseChoice.GetInputNumber();

            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    Program.currentLoggedIn = null;
                    Console.WriteLine("See you again.");
                    break;
            }

        }
    }
}
