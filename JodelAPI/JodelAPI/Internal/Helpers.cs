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
    }
}
