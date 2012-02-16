using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WinCompare
{
    public class Hash : IHash
    {
        public string GetHash(string path)
        {
            string md5Result;
            var sb = new StringBuilder();
            var md5Hasher = MD5.Create();
            using (var fs = File.OpenRead(path))
            {
                foreach (var b in md5Hasher.ComputeHash(fs))
                {
                    sb.Append(b.ToString("x2").ToLower());
                }                
            }
            md5Result = sb.ToString();
            return md5Result;
        }    
    }
}