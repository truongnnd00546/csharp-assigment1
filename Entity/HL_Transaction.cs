using System;

namespace HL_Bank
{
    public class HL_Transaction 
     {
        public enum ActiveStatus
        {
            PROCESSING = 1,
            DONE = 2,
            REJECT = 0,
            DELETED = -1,
        }

        public enum TransactionType
        {
            DEPOSIT = 1,
            WITHDRAW = 2,
            TRANSFER = 3
        }

         private string id;
         private decimal amount;
         private string content;
         private string senderAccountNumber;
         private string receiverAccountNumber;
         private TransactionType type;
         private string createdAt;
         private ActiveStatus status; 


         public HL_Transaction()
         {
         }

         public HL_Transaction(string id,string senderAccountNumber, decimal amount, string content, string receiverAccountNumber, TransactionType type)
         {
             this.id = id;
             this.senderAccountNumber = senderAccountNumber;
             this.amount = amount;
             this.content = content;
             this.receiverAccountNumber = receiverAccountNumber;
         }

        public HL_Transaction(string id, decimal amount, string content, string senderAccountNumber, string receiverAccountNumber, TransactionType type, string createdAt, ActiveStatus status)
        {
            this.id = id;
            this.amount = amount;
            this.content = content;
            this.senderAccountNumber = senderAccountNumber;
            this.receiverAccountNumber = receiverAccountNumber;
            this.type = type;
            this.createdAt = createdAt;
            this.status = status;            
        }

        public string Id
         {
             get => id;
             set => id = value;
         }

         public decimal Amount
         {
             get => amount;
             set => amount = value;
         }

         public string Content
         {
             get => content;
             set => content = value;
         }

         public string SenderAccountNumber
         {
             get => senderAccountNumber;
             set => senderAccountNumber = value;
         }

         public string ReceiverAccountNumber
         {
             get => receiverAccountNumber;
             set => receiverAccountNumber = value;
         }

         public TransactionType Type
         {
             get => type;
             set => type = value;
         }

         public string CreatedAt
         {
             get => createdAt;
             set => createdAt = value;
         }

        public ActiveStatus Status
         {
             get => status;
             set => status = value;
         }
     }
}