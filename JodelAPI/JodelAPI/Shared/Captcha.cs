using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Shared
{
    public class Captcha
    {
        public Captcha(byte[] image, string key, string imageUrl)
        {
            Image = image;
            Key = key;
            ImageUrl = imageUrl;
        }

        public Captcha(byte[] image, string key, string imageUrl, int imageSize)
        {
            Image = image;
            Key = key;
            ImageUrl = imageUrl;
            ImageSize = imageSize;
        }

        public byte[] Image { get; }
        public string Key { get; }
        public string ImageUrl { get; }
        public int ImageSize { get; }
    }
}
