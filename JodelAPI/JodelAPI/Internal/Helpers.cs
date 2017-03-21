using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using static JodelAPI.Shared.JodelPost;

namespace JodelAPI.Internal
{
    internal static class Helpers
    {
        static Dictionary<PostColor, string> Colors = new Dictionary<PostColor, string>
        {
            { PostColor.Red, "DD5F5F" },
            { PostColor.Orange, "FF9908" },
            { PostColor.Yellow, "FFBA00" },
            { PostColor.Blue, "DD5F5F" },
            { PostColor.Bluegreyish, "8ABDB0" },
            { PostColor.Green, "9EC41C" },
            { PostColor.Random, "FFFFFF" }
        };

        public static string GetColor(PostColor color) => Colors[color];

        public static string GetRandomDeviceUid()
        {
            byte[] tokenData = new byte[64];
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
