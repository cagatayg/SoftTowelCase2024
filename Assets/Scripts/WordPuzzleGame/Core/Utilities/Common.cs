using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace STGames
{
    public static class CommonExt
    {
        public static float Range(this Random source, float start, float end)
        {
            return UnityEngine.Random.Range(start, end);
        }

        public static int Range(this Random source, int start, int end)
        {
            return UnityEngine.Random.Range(start, end);
        }
    }

    public static class Common
    {
        public static Random Random = new Random();
        public static long Timestamp => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        public static string CalculateMd5Hash(byte[] inputBytes)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string CalculateMd5Hash(string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
