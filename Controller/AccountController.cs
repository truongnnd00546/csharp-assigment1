using System;
using System.Security.Cryptography;
using HL_Bank.Model;

namespace HL_Bank.Controller
{
    public class AccountController
    {
        private static HL_AccountModel _accountModel = new HL_AccountModel();
        private static HL_Account account;

        public bool AddAccount()
        {
            Console.WriteLine("----Please enter information----");
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            if (_accountModel.CheckUsername(username))
            {
                Console.WriteLine("Username existed.");
                return false;
            }
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();
            Console.WriteLine("Enter confirm password:");
            string cpassword = Console.ReadLine();
            Console.WriteLine("Enter identityCard:");
            string identityCard = Console.ReadLine();
            Console.WriteLine("Enter fullname:");
            string fullname = Console.ReadLine();
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter birthday:");
            string birthday = Console.ReadLine();
            Console.WriteLine("Enter phoneNumber:");
            string phoneNumber = Console.ReadLine();
            Console.WriteLine("Enter address:");
            string address = Console.ReadLine();
            Console.WriteLine("Enter gender");
            int gender = ParseChoice.GetInputNumber();

            account = new HL_Account(username, password, cpassword, identityCard, fullname, email, birthday, phoneNumber, address, gender);
            
            var errors = account.CheckValid();
            if (errors.Count == 0)
            {
                account.GetMD5WithSalt();
                if (_accountModel.Save(account))
                {
                    Console.WriteLine("Save success!!!");
                }
                else
                {
                    Console.WriteLine("Not success. Please try again.");
                }
            }
            else
            {
                Console.Error.WriteLine("Please fix following errors and try again.");
                foreach (var messagErrorsValue in errors.Values)
                {
                    Console.Error.WriteLine(messagErrorsValue);
                }
            }
            return true;
        }

        
        public bool CheckLogin()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string pass = Console.ReadLine();
            HL_Account account1 = _accountModel.GetByUsername(username);
            if (account1 == null)
            {
                Console.WriteLine("Invalid account info.");
                return false;
            }

            if (account1.CheckEncryptedPassword(pass) == false)
            {
                Console.WriteLine("Invalid account info.");
                return false;
            }

            Program.currentLoggedIn = account1;
            return true;
        }


        public void ShowAccountInformation()
        {
            var currentAccount = _accountModel.GetAccountByUserName(Program.currentLoggedIn.Username);
            if (currentAccount == null)
            {
                Program.currentLoggedIn = null;
                Console.WriteLine("AccountInfor fail or locked");
                return;
            }
            Console.WriteLine("AccountNumber : " + Program.currentLoggedIn.AccountNumber);
            Console.WriteLine("Balance : " + Program.currentLoggedIn.Balance);
            Console.WriteLine("Status: " + Program.currentLoggedIn.Status);
        }


        public void Withdraw()
        {
            Console.WriteLine("Withdraw.");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please enter amount to Withdraw: ");
            var amount = ParseChoice.GetDecimalNumber();
            Console.WriteLine("Please enter message content: ");
            var content = Console.ReadLine();
            
            var historyTransaction = new HL_Transaction
            {
                Id = Guid.NewGuid().ToString(),
                Type = HL_Transaction.TransactionType.WITHDRAW,
                Amount = amount,
                Content = content,
                SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                ReceiverAccountNumber = Program.currentLoggedIn.AccountNumber,
                Status = HL_Transaction.ActiveStatus.DONE
            };
            if (_accountModel.UpdateBalance(Program.currentLoggedIn, historyTransaction))
            {
                Console.WriteLine("Transaction success!");
            }
            else
            {
                Console.WriteLine("Transaction fails, please try again!");
            }
            Program.currentLoggedIn = _accountModel.GetAccountByUserName(Program.currentLoggedIn.Username);
            Console.WriteLine("Current balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine("Press enter to continue!");
            Console.ReadLine();
        }

        
        public void Deposit()
        {
            Console.WriteLine("Deposit.");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Please enter amount to deposit: ");
            var amount = ParseChoice.GetDecimalNumber();
            Console.WriteLine("Please enter message content: ");
            var content = Console.ReadLine();

            var historyTransaction = new HL_Transaction
            {
                Id = Guid.NewGuid().ToString(),
                Type = HL_Transaction.TransactionType.DEPOSIT,
                Amount = amount,
                Content = content,
                SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                ReceiverAccountNumber = Program.currentLoggedIn.AccountNumber,
                Status = HL_Transaction.ActiveStatus.DONE
            };
            if (_accountModel.UpdateBalance(Program.currentLoggedIn, historyTransaction))
            {
                Console.WriteLine("Transaction success!");
            }
            else
            {
                Console.WriteLine("Transaction fails, please try again!");
            }
            Program.currentLoggedIn = _accountModel.GetAccountByUserName(Program.currentLoggedIn.Username);
            Console.WriteLine("Current balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine("Press enter to continue!");
            Console.ReadLine();
        }               
               
        
        public void Transfer()
        {
            Console.WriteLine(Program.currentLoggedIn.Status);

            Console.WriteLine("Transfer.");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Enter accountNumber to transfer: ");
            string accountNumber = Console.ReadLine();
            var account = _accountModel.GetByAccountNumber(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Invalid account info");
                return;
            }
            Console.WriteLine("You are doing transaction with account: " + account.Fullname);

            Console.WriteLine("Enter amount to transfer: ");
            var amount = ParseChoice.GetDecimalNumber();
            if (amount > Program.currentLoggedIn.Balance)
            {
                Console.WriteLine("Amount not enough to perform transaction.");
                return;
            }

            Console.WriteLine("Please enter message content: ");
            var content = Console.ReadLine();
            Console.WriteLine("Are you sure you want to make a transaction with your account ? (y/n)");
            var choice = Console.ReadLine();
            if (choice.Equals("n"))
            {
                return;
            }
            
            var historyTransaction = new HL_Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Type = HL_Transaction.TransactionType.TRANSFER,
                Amount = amount,
                Content = content,
                SenderAccountNumber = Program.currentLoggedIn.AccountNumber,
                ReceiverAccountNumber = account.AccountNumber,
                Status = HL_Transaction.ActiveStatus.DONE
            };

            if (_accountModel.TransferAmount(Program.currentLoggedIn, historyTransaction))
            {
                Console.WriteLine("Transaction success!");
            }
            else
            {
                Console.WriteLine("Transaction fails, please try again!");
            }
            
            Program.currentLoggedIn = _accountModel.GetByUsername(Program.currentLoggedIn.Username);
            Console.WriteLine("Current balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine("Press enter to continue!");
            Console.ReadLine();
        }

        
        public void CheckBalance() 
        {
            Program.currentLoggedIn = _accountModel.GetAccountByUserName(Program.currentLoggedIn.Username);
            Console.WriteLine("Account Information");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Full name: " + Program.currentLoggedIn.Fullname);
            Console.WriteLine("Account number: " + Program.currentLoggedIn.AccountNumber);
            Console.WriteLine("Balance: " + Program.currentLoggedIn.Balance);
            Console.WriteLine("Press enter to continue!");
            Console.ReadLine();
        }
    }
}