using System;
using HL_Bank.Model;

namespace HL_Bank.Controller
{
    public class TransactionController
    {
        private static HL_TransactionModel _transactionModel = new HL_TransactionModel();

        public bool GetTransaction()
        {
            HL_Transaction transaction = _transactionModel.GetByUsername(Program.currentLoggedIn.Username);
            if (transaction == null)
            {
                Console.WriteLine("loi");
            }

            Console.WriteLine("ng gui: " + transaction.SenderAccountNumber);
            Console.WriteLine("tien: " + transaction.Amount);
            Console.WriteLine("nd: " + transaction.Content);

            Console.WriteLine("ng nhan: " + transaction.ReceiverAccountNumber);

            return true;
        }
    }
}