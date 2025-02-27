using Konscious.Security.Cryptography;
using System.Text;

namespace Utils
{
    public static class SecurityExtensions
    {
        private static readonly byte[] StandardSalt = Encoding.ASCII.GetBytes("963f6e38c94289c75c5efc6db5b810fe");
        private static int DegreeOfParallelism = 16;
        private static int MemorySize = 1024;
        private static int Iterations = 20;
        private static int HashByteSize = 128;

        public static async Task<byte[]> GetPasswordHash(string userName, string password, string userId, string? customSalt = null)
        {
            var argon2 = new Argon2d(Encoding.ASCII.GetBytes(password));
            argon2.Iterations = Iterations;
            argon2.DegreeOfParallelism = DegreeOfParallelism;
            argon2.MemorySize = MemorySize;
            argon2.AssociatedData = Encoding.ASCII.GetBytes(userName);
            argon2.KnownSecret = Encoding.ASCII.GetBytes(userId);
            argon2.Salt = String.IsNullOrEmpty(customSalt) ? StandardSalt : Encoding.ASCII.GetBytes(customSalt);
            return await argon2.GetBytesAsync(HashByteSize);
        }
        public static async Task<string> GetPasswordHashString(string userName, string password, string userId, string? customSalt = null)
        {
            return Convert.ToBase64String(await GetPasswordHash(userName, password, userId, customSalt));
        }

        public static async Task<bool> ComparePasswords(byte[] hash, string userName, string password, string userId, string? customSalt = null)
        {
            var byteHash = await GetPasswordHash(userName, password, userId, customSalt);
            return byteHash.SequenceEqual(hash);
        }

        public static async Task<bool> ComparePasswords(string hashString, string userName, string password, string userId, string? customSalt = null)
        {
            var byteHash = await GetPasswordHash(userName, password, userId, customSalt);
            return (Convert.ToBase64String(byteHash) == hashString);
        }
    }
}
