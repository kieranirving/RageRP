using RageRP.Server.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool check = false;
            while(!check)
            {
                Console.Write("Encrypt or Decrypt | E/D :");
                string input = Console.ReadLine();
                Console.WriteLine("");
                switch(input.Trim().ToLower())
                {
                    case "e":
                        Console.Write("Enter Password: ");
                        string password = Console.ReadLine();
                        Console.WriteLine($"Encrypted Password {EncryptionService.EncryptPassword(password)}");
                        check = true;
                        break;
                    case "d":
                        Console.Write("Enter Encrypted String: ");
                        string encrypted = Console.ReadLine();
                        Console.WriteLine($"Encrypted Password {EncryptionService.DecryptPassword(encrypted)}");
                        break;
                    default:
                        Console.WriteLine("Invalid Selection");
                        break;
                }
                Console.ReadLine();
            }
        }
    }
}
