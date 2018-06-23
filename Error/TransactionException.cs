using System;
using System.Collections.Generic;
using System.Text;

namespace HL_Bank.Error
{
    class TransactionException : Exception
    {
        public TransactionException(string message) : base(message)
        {
        }
    }
}
