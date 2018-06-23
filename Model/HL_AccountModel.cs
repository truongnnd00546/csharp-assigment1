using System;
using System.Transactions;
using MySql.Data.MySqlClient;

namespace HL_Bank.Model
{
    public class HL_AccountModel
    {
        public bool Save(HL_Account account)
        {
            DbConnection.Instance().OpenConnection();
            string cnnStr = "insert into `account`" +
                            " (accountNumber, username, password, balance, identityCard, fullname, email, birthday,phoneNumber, address, gender, status, salt)" +
                            " values (@accountNumber, @username, @password, @balance, @identityCard, @fullname, @email, @birthday, @phoneNumber, @address, @gender, @status, @salt)";

            var cmd = new MySqlCommand(cnnStr, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@accountNumber", account.AccountNumber);
            cmd.Parameters.AddWithValue("@username", account.Username);
            cmd.Parameters.AddWithValue("@password", account.Password);
            cmd.Parameters.AddWithValue("@balance", account.Balance);
            cmd.Parameters.AddWithValue("@identityCard", account.IdentityCard);
            cmd.Parameters.AddWithValue("@email", account.Email);
            cmd.Parameters.AddWithValue("@birthday", account.Birthday);
            cmd.Parameters.AddWithValue("@fullname", account.Fullname);
            cmd.Parameters.AddWithValue("@phoneNumber", account.PhoneNumber);
            cmd.Parameters.AddWithValue("@address", account.Address);
            cmd.Parameters.AddWithValue("@gender", account.Gender);
            cmd.Parameters.AddWithValue("@status", account.Status);
            cmd.Parameters.AddWithValue("@salt", account.Salt);

            cmd.ExecuteNonQuery();
            DbConnection.Instance().CloseConnection();
            return true;
        }

        public bool CheckUsername(string username)
        {
            DbConnection.Instance().OpenConnection();
            var cnnStr = "select * from `account` where `username` = @username";
            var cmd = new MySqlCommand(cnnStr, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@username", username);
            var reader = cmd.ExecuteReader();
            var isExist = reader.Read();
            reader.Close();
            DbConnection.Instance().CloseConnection();
            return isExist;
        }

        public HL_Account GetByUsername(string username)
        {
            HL_Account _account = null;
            DbConnection.Instance().OpenConnection();
            var cnnStr = "select * from `account` where `username` = @username and status IN (1,2)";
            var cmd = new MySqlCommand(cnnStr, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@username", username);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                _account = new HL_Account();
                _account.AccountNumber = reader.GetString("accountNumber");
                _account.Username = reader.GetString("username");
                _account.Password = reader.GetString("password");
                _account.Salt = reader.GetString("salt");
                _account.Balance = reader.GetInt32("balance");
                _account.IdentityCard = reader.GetString("identityCard");
                _account.Fullname = reader.GetString("fullname");
                _account.Email = reader.GetString("email");
                _account.PhoneNumber = reader.GetString("phoneNumber");
                _account.Address = reader.GetString("address");
                _account.Gender = reader.GetInt32("gender");
                var status = reader.GetInt32("status");
                switch (status)
                {
                    case 1:
                        _account.Status = HL_Account.ActiveStatus.ACTIVE;
                        break;
                    case 2:
                        _account.Status = HL_Account.ActiveStatus.LOCKED;
                        break;
                }
            }
            reader.Close();
            DbConnection.Instance().CloseConnection();
            return _account;
        }

        public HL_Account GetAccountByUserName(string username)
        {
            DbConnection.Instance().OpenConnection();
            var queryString = "select * from  `account` where username = @username and status = 1";
            var cmd = new MySqlCommand(queryString, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@username", username);
            var reader = cmd.ExecuteReader();
            HL_Account account = null;
            if (reader.Read())
            {
                var _username = reader.GetString("username");
                var password = reader.GetString("password");
                var salt = reader.GetString("salt");
                var accountNumber = reader.GetString("accountNumber");
                var identityCard = reader.GetString("identityCard");
                var balance = reader.GetDecimal("balance");
                var phone = reader.GetString("phoneNumber");
                var email = reader.GetString("email");
                var fullName = reader.GetString("fullName");
                var createdAt = reader.GetString("createdAt");
                var updatedAt = reader.GetString("updatedAt");
                var status = reader.GetInt32("status");
                account = new HL_Account(username, password, salt, accountNumber, identityCard, balance, phone, email,
                    fullName, createdAt, updatedAt, (HL_Account.ActiveStatus)status);
            }
            reader.Close();
            DbConnection.Instance().CloseConnection();
            return account;
        }

        public bool UpdateBalance(HL_Account account, HL_Transaction historyTransaction)
        {
            DbConnection.Instance().OpenConnection();

            /**
             * 1. Lấy thông tin số dư mới nhất của tài khoản.
             * 2. Kiểm tra kiểu transaction. Chỉ chấp nhận deposit và withdraw.
             *     2.1. Kiểm tra số tiền rút nếu kiểu transaction là withdraw.                 
             * 3. Update số dư vào tài khoản.
             *     3.1. Tính toán lại số tiền trong tài khoản.
             *     3.2. Update số tiền vào database.
             * 4. Lưu thông tin transaction vào bảng transaction.
             */

            // 1. Lấy thông tin số dư mới nhất của tài khoản.
            var queryBalance = "select `balance` from `account` where username = @username and status =1";
            MySqlCommand queryBalanceCommand = new MySqlCommand(queryBalance, DbConnection.Instance().Connection);
            queryBalanceCommand.Parameters.AddWithValue("@username", account.Username);
            var balanceReader = queryBalanceCommand.ExecuteReader();
            // Không tìm thấy tài khoản tương ứng, throw lỗi.
            if (!balanceReader.Read())
            {
                // Không tồn tại bản ghi tương ứng, lập tức rollback transaction, trả về false.
                throw new TransactionException("Invalid username");
            }

            var currentBalance = balanceReader.GetDecimal("balance");
            balanceReader.Close();

            // 2. Kiểm tra kiểu transaction. Chỉ chấp nhận deposit và withdraw. 
            if (historyTransaction.Type != HL_Transaction.TransactionType.DEPOSIT
                && historyTransaction.Type != HL_Transaction.TransactionType.WITHDRAW)
            {
                throw new TransactionException("Invalid transaction type!");
            }

            // 2.1. Kiểm tra số tiền rút nếu kiểu transaction là withdraw(rút).
            if (historyTransaction.Type == HL_Transaction.TransactionType.WITHDRAW &&
                historyTransaction.Amount > currentBalance)
            {
                throw new TransactionException("Not enough money!");
            }

            // 3.1. Tính toán lại số tiền trong tài khoản.
            if (historyTransaction.Type == HL_Transaction.TransactionType.DEPOSIT)
            {
                currentBalance += historyTransaction.Amount;
            }
            else if (historyTransaction.Type == HL_Transaction.TransactionType.WITHDRAW)
            {
                currentBalance -= historyTransaction.Amount;
            }

            Console.WriteLine(historyTransaction.Type);
            if (UpdateBalanceAndSaveTransaction(account, currentBalance, historyTransaction))
            {
                return true;
            }

            DbConnection.Instance().CloseConnection();
            return false;
        }

        public Boolean CheckExistUserName(string username)
        {
            return false;
        }


        public HL_Account GetByAccountNumber(string accountNumber)
        {
            HL_Account account = null;
            DbConnection.Instance().OpenConnection();
            var queryString = "select * from `account` where accountNumber = @accountNumber and status = 1";
            var cmd = new MySqlCommand(queryString, DbConnection.Instance().Connection);
            cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
            var reader = cmd.ExecuteReader();
            var isExist = reader.Read();
            if (isExist)
            {
                account = new HL_Account
                {
                    AccountNumber = reader.GetString("accountNumber"),
                    Username = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Salt = reader.GetString("salt"),
                    Fullname = reader.GetString("fullname"),
                    Balance = reader.GetInt32("balance")
                };
                reader.Close();
                return account;
            }
            DbConnection.Instance().CloseConnection();
            return null;
        }

        public bool TransferAmount(HL_Account account, HL_Transaction historyTransaction)
        {
            DbConnection.Instance().OpenConnection();

            var queryBalance = "select `balance` from `account` where username = @username and status = 1";
            MySqlCommand queryBalanceCommand = new MySqlCommand(queryBalance, DbConnection.Instance().Connection);
            queryBalanceCommand.Parameters.AddWithValue("@username", account.Username);
            var balanceReader = queryBalanceCommand.ExecuteReader();
            if (!balanceReader.Read())
            {
                throw new TransactionException("Invalid username");
            }
            var currentBalance = balanceReader.GetDecimal("balance");
            currentBalance -= historyTransaction.Amount;
            balanceReader.Close();

            if (UpdateBalanceAndSaveTransaction(account, currentBalance, historyTransaction))
            {
                return true;
            }

            DbConnection.Instance().CloseConnection();
            return false;
        }

        public bool UpdateBalanceAndSaveTransaction(HL_Account account, decimal currentBalance, HL_Transaction historyTransaction)
        {
            var transaction = DbConnection.Instance().Connection.BeginTransaction();
            try
            {
                // Update số dư vào database.
                var updateAccountResult = 0;
                var queryUpdateAccountBalance = "update `account` set balance = @balance where username = @username and status = 1";
                var cmdUpdateAccountBalance = new MySqlCommand(queryUpdateAccountBalance, DbConnection.Instance().Connection);
                cmdUpdateAccountBalance.Parameters.AddWithValue("@username", account.Username);
                cmdUpdateAccountBalance.Parameters.AddWithValue("@balance", currentBalance);
                updateAccountResult = cmdUpdateAccountBalance.ExecuteNonQuery();

                // Lưu thông tin transaction vào bảng transaction.
                var insertTransactionResult = 0;
                var queryInsertTransaction = "insert into `transaction` " +
                                             "(id, fromAccountNumber, amount, content, toAccountNumber, type, status) " +
                                             "values (@id, @fromAccountNumber, @amount, @content, @toAccountNumber, @type, @status)";
                var cmdInsertTransaction =
                    new MySqlCommand(queryInsertTransaction, DbConnection.Instance().Connection);
                cmdInsertTransaction.Parameters.AddWithValue("@id", historyTransaction.Id);
                cmdInsertTransaction.Parameters.AddWithValue("@fromAccountNumber",
                    historyTransaction.SenderAccountNumber);
                cmdInsertTransaction.Parameters.AddWithValue("@amount", historyTransaction.Amount);
                cmdInsertTransaction.Parameters.AddWithValue("@content", historyTransaction.Content);
                cmdInsertTransaction.Parameters.AddWithValue("@toAccountNumber",
                    historyTransaction.ReceiverAccountNumber);
                cmdInsertTransaction.Parameters.AddWithValue("@type", historyTransaction.Type);
                cmdInsertTransaction.Parameters.AddWithValue("@status", historyTransaction.Status);
                insertTransactionResult = cmdInsertTransaction.ExecuteNonQuery();

                // Kiểm tra lại câu lệnh
                if (updateAccountResult == 1 && insertTransactionResult == 1)
                {
                    transaction.Commit();
                    return true;
                }
            }
            catch (TransactionException e)
            {
                transaction.Rollback();
            }
            return false;
        }
    }
}