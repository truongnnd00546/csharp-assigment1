using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HL_Bank.Error
{
    class FileHandle
    {
        public static List<HL_Transaction> ReadTransactions()
        {
            var list = new List<HL_Transaction>();
            var lines = File.ReadAllLines("NeverEverGetBackTogether.txt");
            for (var i = 0; i < lines.Length; i += 1)
            {
                if (i == 0)
                {
                    continue;
                }

                var linesSplited = lines[i].Split("|");
                if (linesSplited.Length == 8)
                {
                    var tx = new HL_Transaction()
                    {
                        Id = linesSplited[0],
                        SenderAccountNumber = linesSplited[1],
                        ReceiverAccountNumber = linesSplited[2],
                        Type = (HL_Transaction.TransactionType)Int32.Parse(linesSplited[3]),
                        Amount = Decimal.Parse(linesSplited[4]),
                        Content = linesSplited[5],
                        CreatedAt = linesSplited[6],
                        Status = (HL_Transaction.ActiveStatus)Int32.Parse(linesSplited[7])
                    };
                    list.Add(tx);
                }
            }

            return list;
        }

        public static Dictionary<string, HL_Account> ReadAccounts()
        {
            var dictionary = new Dictionary<string, HL_Account>();
            var lines = File.ReadAllLines("ForgetMeNot.txt");
            for (var i = 0; i < lines.Length; i += 1)
            {
                if (i == 0)
                {
                    continue;
                }

                var linesSplited = lines[i].Split("|");
                if (linesSplited.Length == 6)
                {
                    var acc = new HL_Account()
                    {
                        AccountNumber = linesSplited[0],
                        Username = linesSplited[1],
                        Fullname = linesSplited[2],
                        Balance = Decimal.Parse(linesSplited[3]),
                        Salt = linesSplited[4],
                        Status = (HL_Account.ActiveStatus)Int32.Parse(linesSplited[5])
                    };
                    if (dictionary.ContainsKey(acc.AccountNumber))
                    {
                        continue;
                    }

                    dictionary.Add(acc.AccountNumber, acc);
                }
            }

            return dictionary;
        }
    }
}
