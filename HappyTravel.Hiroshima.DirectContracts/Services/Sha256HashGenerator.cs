using System.Security.Cryptography;
using System.Text;

namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public class Sha256HashGenerator: ISha256HashGenerator
    {
        public string Generate(string origin) 
            => string.IsNullOrEmpty(origin) 
            ? string.Empty 
            : Generate(Encoding.UTF8.GetBytes(origin));


        public string Generate(byte[] bytes)
        {
            byte[] hash;
            lock (Locker)
            {
                hash = _sha256Managed.ComputeHash(bytes);
            }
            var stringBuilder = new StringBuilder();  
            foreach (var b in hash)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
        
        
        private static readonly object Locker = new object();
        private readonly SHA256Managed _sha256Managed = new SHA256Managed();
    }
}