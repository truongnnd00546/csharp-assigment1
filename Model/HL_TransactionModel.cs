using System;
using System.Transactions;
using MySql.Data.MySqlClient;

namespace HL_Bank.Model
{
    public class HL_TransactionModel
    {
    
        public HL_Transaction GetByUsername(string username)
        {
            DbConnection.Instance().OpenConnection();
            var cnnStr = "SELECT * FROM `transaction` INNER JOIN `account` ON transaction.fromAccountNumber = account.accountNumber WHERE username = @username";
            var cmd = new MySqlCommand(cnnStr, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@username", username);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string id = reader.GetString("id");
                string senderAccountNumber = reader.GetString("fromAccountNumber");
                decimal amount = reader.GetDecimal("amount");
                string content = reader.GetString("content");
                string receiverAccountNumber = reader.GetString("toAccountNumber");
                var type = reader.GetInt16("type");

                HL_Transaction transaction = new HL_Transaction(id, senderAccountNumber, amount, content, receiverAccountNumber, (HL_Transaction.TransactionType)type);

                return transaction;
            }
            reader.Close();
            DbConnection.Instance().CloseConnection();
            return null;
        }
    
        public HL_Transaction FindByDateTime(string createdAt)
        {
            DbConnection.Instance().OpenConnection();
            var cnnStr = "select * from `transaction` where `createdAt` = @createdAt";
            var cmd = new MySqlCommand(cnnStr, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@createdAt", createdAt);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string id = reader.GetString("id");
                string senderAccountNumber = reader.GetString("fromAccountNumber");
                decimal amount = reader.GetDecimal("amount");
                string content = reader.GetString("content");
                string receiverAccountNumber = reader.GetString("toAccountNumber");
                var type = reader.GetInt16("type");

                HL_Transaction transaction = new HL_Transaction(id, senderAccountNumber, amount, content, receiverAccountNumber, (HL_Transaction.TransactionType)type);
                
                return transaction;
            }
            reader.Close(); 
            DbConnection.Instance().CloseConnection();
            return null;
        }
    }
}