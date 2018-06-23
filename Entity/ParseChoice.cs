using System;

namespace HL_Bank
{
    public class ParseChoice
    {
        // yêu cầu ng dùng nhập số. Nhập sai y/c nhập laị.
        public static int GetInputNumber()
        {
            var number = 0;
            while (true)
            {
                try
                {
                    number = Int32.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Please enter number.");
                    throw;
                }
            }
            return number;
        }
        
        public static decimal GetDecimalNumber()
        {
            decimal number = 0;
            while (true)
            {
                try
                {
                    number = Decimal.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Please enter number.");
                    throw;
                }
            }
            return number;
        }

//        public static string GetMD5WithSalt(string password, string salt)
//        {
//            string str_MD5 = "";
//            byte[] mang = System.Text.Encoding.UTF8.GetBytes(password + salt);
//            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
//            mang = md5.ComputeHash(mang);
//
//            foreach (byte b in mang)
//            {
//                str_MD5 += b.ToString("X2");
//            }
//            return str_MD5;
//        }
        
        
    }
}