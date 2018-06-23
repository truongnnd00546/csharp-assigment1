using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HL_Bank
{
    public class HL_Account
    {
        public enum ActiveStatus
        {
            INACTIVE = 0,
            ACTIVE = 1,
            LOCKED = 2
        }

        private string username;
        private string password;
        private string cpassword;
        private string salt;
        private string accountNumber;
        private decimal balance = 50000;
        private string identityCard;
        private string fullname;
        private string email;
        private string phoneNumber;
        private string address;
        private int gender; // 1. male | 2. female | 3. rather not say
        private string birthday;
        private string createdAt;
        private string updateAt;
        private ActiveStatus status;

        public HL_Account()
        {
        }

        private void generateAccountNumber()
        {
            this.accountNumber = Guid.NewGuid().ToString();  //unique
        }
        
        private void generateSalt()
        {
            this.salt = Guid.NewGuid().ToString().Substring(0,7);
        }
        
        public HL_Account(string username, string password, string cpassword,string identityCard, string fullname, string email, string birthday, string phoneNumber, string address,int gender)
        { 
            generateAccountNumber();
            this.username = username;
            this.password = password;
            this.cpassword = cpassword;
            generateSalt();
            this.identityCard = identityCard;
            this.fullname = fullname;
            this.email = email;
            this.birthday = birthday;
            this.phoneNumber = phoneNumber;
            this.address = address;
            this.gender = gender;
        }

        public HL_Account(string username, string password, string salt, string accountNumber, string identityCard,
           decimal balance, string phoneNumber, string email, string fullname, string createdAt, string updateAt,
           ActiveStatus status)
        {
            this.username = username;
            this.password = password;
            this.salt = salt;
            this.accountNumber = accountNumber;
            this.identityCard = identityCard;
            this.balance = balance;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.fullname = fullname;
            this.createdAt = createdAt;
            this.updateAt = updateAt;
            this.status = status;
        }

        public string Fullname
        {
            get => fullname;
            set => fullname = value;
        }

        public string Birthday
        {
            get =>birthday;
            set => birthday = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public string Cpassword
        {
            get => cpassword;
            set => cpassword = value;
        }

        public string AccountNumber
        {
            get => accountNumber;
            set => accountNumber = value;
        }

        public decimal Balance
        {
            get =>balance;
            set => balance = value;
        }

        public string IdentityCard
        {
            get => identityCard;
            set => identityCard = value;
        }

        public string Email
        {
            get => email;
            set =>email = value;
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => phoneNumber = value;
        }

        public string Address
        {
            get => address;
            set => address = value;
        }
        
        public int Gender
        {
            get => gender;
            set => gender = value;
        }
        
        public string CreatedAt
        {
            get =>createdAt;
            set => createdAt = value;
        }

        public string UpdateAt
        {
            get => updateAt;
            set => updateAt = value;
        }

        public ActiveStatus Status
        {
            get => status;
            set => status = value;
        }

        public string Salt
        {
            get => salt;
            set => salt = value;
        }
        
        public Dictionary<string, string> CheckValid()
        {
            var errors = new Dictionary<string, string>();
            
            if (string.IsNullOrEmpty(this.username))
            {
                errors.Add("username", "Username can not be null or empty.");
            }
             else if (this.username.Length < 6)
            {
                errors.Add("username", "Username is too short. At least 6 charecter.");
            }

            if (!this.password.Equals(this.cpassword))
            {
                errors.Add("password", "Confirm password does not match.");
            }
            else if (string.IsNullOrEmpty(this.password))
            {
                errors.Add("password", "Password can not be null or empty.");
            }
            return errors;
        }

        public Dictionary<string, string> ValidLoginInformation()
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(this.username))
            {
                errors.Add("username", "Username can not be null or empty.");
            }
            if (string.IsNullOrEmpty(this.password))
            {
                errors.Add("password", "Password can not be null or empty.");
            }
            return errors;
        }

        public bool CheckEncryptedPassword(string pass)
        {
            string checkPass = EncryptedString(pass, salt);
            return checkPass == password;
        }

        public string EncryptedString(string content, string salt)
        {
            string str_MD5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(content + salt);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            mang = md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_MD5 += b.ToString("X2");
            }
            return str_MD5;
        }

        public void GetMD5WithSalt()
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is null or empty.");
            }
            password = EncryptedString(password, salt);
        }
        
        public virtual decimal Deposit(decimal amount)
        {
            return this.Balance + amount;
        }
        
    }
   
}