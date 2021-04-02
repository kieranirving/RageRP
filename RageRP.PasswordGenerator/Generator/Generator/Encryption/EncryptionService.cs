using System;
using System.Collections.Generic;
using System.Text;

using RageRP.Server.Encryption.Rijndael;

namespace RageRP.Server.Encryption
{
    public static class EncryptionService
    {
        private readonly static string hash = "wr!ohyq£$%Y";
        private readonly static string PasswordHash = "£Q$%O:&HQJY£h";
        public static string Encrypt(string text)
        {
            return Rijndael.Encryption.Encrypt(text, hash);
        }

        public static string Decrypt(string text)
        {
            return Rijndael.Encryption.Decrypt(text, hash);
        }

        public static string EncryptID(long id)
        {
            return Rijndael.Encryption.Encrypt(id.ToString(), hash);
        }

        public static long DecryptID(string id)
        {
            return long.Parse(Rijndael.Encryption.Decrypt(id, hash));
        }

        public static string EncryptPassword(string text)
        {
            return Rijndael.Encryption.Encrypt(text, PasswordHash);
        }

        public static string DecryptPassword(string text)
        {
            return Rijndael.Encryption.Decrypt(text, PasswordHash);
        }
    }
}