using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordHashing
{
    class Program
    {
        static readonly Dictionary<string, User> userDB = new();

        static string HashPassword(string password)
        {
            using var sha = SHA256.Create();

            var asBytes = Encoding.Default.GetBytes(password);

            var hashed = sha.ComputeHash(asBytes);

            return Convert.ToBase64String(hashed);
        }

        static (string Username, string Password) GetInfo()
        {
            Console.Write("Username? ");
            var username = Console.ReadLine();

            Console.Write("Password? ");
            var password = Console.ReadLine();

            return (username, password);

        }

        static void Register()
        {
            var info = GetInfo();

            Random rand = new();

            int salt = rand.Next();

            string saltedPW = $"{info.Password}{salt}";

            Console.WriteLine(saltedPW);

            var hashedPW = HashPassword(saltedPW);

            Console.WriteLine($"Registering {info} - {hashedPW}");

            userDB.Add(info.Username, new User { Username = info.Username, PasswordHash = hashedPW, Salt = salt });
        }

        static void Login()
        {
            var info = GetInfo();

            User user = userDB.GetValueOrDefault(info.Username);

            var hashedPW = HashPassword($"{info.Password}{user.Salt}");

            Console.WriteLine($"Attempting login for {info} = {hashedPW}");


            if(user?.PasswordHash == hashedPW)
                Console.WriteLine($"Found {user}");
            else
                Console.WriteLine("No one.");
        }

        static void Main()
        {
            while(true)
            {
                Console.Write("[R]egister or [L]ogin? ");

                char option = Console.ReadLine().ToUpper().FirstOrDefault(); ;

                switch (option)
                {
                    case 'R': Register(); break;
                    case 'L': Login(); break;
                    default: return;
                }
            }
        }
    }
}
