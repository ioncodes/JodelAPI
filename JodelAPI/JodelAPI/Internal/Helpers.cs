using System;
using System.Security.Cryptography;
using System.Text;
using JodelAPI.Shared;

namespace JodelAPI.Internal
{
    internal static class Helpers
    {

        //TODO: Neu implementieren
        public static string GetColor(JodelPost.PostColor color)
        {
            switch (color)
            {
                case JodelPost.PostColor.Red:
                    return "DD5F5F";
                case JodelPost.PostColor.Orange:
                    return "FF9908";
                case JodelPost.PostColor.Yellow:
                    return "FFBA00";
                case JodelPost.PostColor.Blue:
                    return "DD5F5F";
                case JodelPost.PostColor.Bluegreyish:
                    return "8ABDB0";
                case JodelPost.PostColor.Green:
                    return "9EC41C";
                case JodelPost.PostColor.Random:
                    return "FFFFFF";
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        public static string GetRandomDeviceUid(int size = 5, bool lowerCase = true)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 1; i < size + 1; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            string value = lowerCase ? builder.ToString().ToLower() : builder.ToString();

            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
