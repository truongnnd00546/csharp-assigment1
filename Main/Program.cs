using System;
using System.Collections.Generic;
using HL_Bank.Controller;
using HL_Bank.Error;
using HL_Bank.view;

namespace HL_Bank
{
    class Program
    {
        public static HL_Account currentLoggedIn;
        public static AccountController controller = new AccountController();
        public static TransactionController controller1 = new TransactionController();

        static void Main(string[] args)
        {
            Menu menu = new Menu();

            while (true)
            {
                if (Program.currentLoggedIn != null)
                {
                    menu.GenerateCustomerMenu();
                }
                else
                {
                    menu.GenerateDefaultMenu();
                }
            }
            // ShowCase("NgoVanTuan");
            //            MainView.GenerateMenu();
            //            Dictionary<string, decimal> dictionaryTransaction = new Dictionary<string, decimal>();
            //            var listTransactions = FileHandle.ReadTransactions();
            //            foreach (var transaction in listTransactions)
            //            {
            //                if (dictionaryTransaction.ContainsKey(transaction.SenderAccountNumber))
            //                {
            //                    dictionaryTransaction[transaction.SenderAccountNumber] += transaction.Amount;
            //                }
            //                else
            //                {
            //                    dictionaryTransaction.Add(transaction.SenderAccountNumber, transaction.Amount);
            //                }
            //            }
            //
            //            var accountDictionary = FileHandle.ReadAccounts();
            //            foreach (var accountItem in accountDictionary)
            //            {
            //                if (dictionaryTransaction.ContainsKey(accountItem.Value.AccountNumber))
            //                {
            //                    // Tạm thời coi số dư là tổng số tiền giao dịch.
            //                    accountItem.Value.Balance = dictionaryTransaction[accountItem.Value.AccountNumber];
            //                }
            //            }
            //
            //            foreach (var account in accountDictionary.Values)
            //            {
            //                Console.WriteLine(account.Username + " - " + account.Fullname + " - " + account.Salt + " - " + account.Balance);
            //            }
            //        }
            //
            //
            //        private static void ShowCase(string name)
            //        {
            //            string reverse = Reverse(name);
            //            Console.WriteLine(reverse);
            //            char[] charArray = reverse.ToCharArray();
            //            List<char> en = new List<char>();
            //            List<char> sa = new List<char>();
            //            for (int i = 0; i < charArray.Length; i++)
            //            {
            //                if (i % 2 == 0)
            //                {
            //                    en.Add(charArray[i]);
            //                }
            //                else
            //                {
            //                    sa.Add(charArray[i]);
            //                }
            //            }
            //
            //            foreach (var e in en)
            //            {
            //                Console.Write(e);
            //            }
            //
            //            Console.WriteLine("");
            //            foreach (var e in en)
            //            {
            //                Console.Write(e + " ");
            //            }
            //
            //            Console.WriteLine("");
            //            foreach (var s in sa)
            //            {
            //                Console.Write(s);
            //            }
            //
            //            Console.WriteLine("");
            //            foreach (var s in sa)
            //            {
            //                Console.Write(" " + s);
            //            }
            //        }
            //
            //        private static string Reverse(string s)
            //        {
            //            var charArray = s.ToCharArray();
            //            Array.Reverse(charArray);
            //            return new string(charArray);
            //        }
        }
}
}