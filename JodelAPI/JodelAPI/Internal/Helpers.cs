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

        public static string GetRandomDeviceUid()
        {
            byte[] tokenData = new byte[32];
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenData);
            }

            using (SHA256 hash = SHA256.Create())
            {
                byte[] result = hash.ComputeHash(tokenData);
                return BitConverter.ToString(result).Replace("-", "").ToLower();
            }
        }
    }
}
