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
            : this(image, key, imageUrl)
        {
            ImageSize = imageSize;
        }

        public byte[] Image { get; }
        public string Key { get; }
        public string ImageUrl { get; }
        public int ImageSize { get; }

        // Example extracted from OJOC. This array can be used for autosolving the captcha. Fill it with your values. it's MD5 & int[] with the answer
        public static Dictionary<string, int[]> Solutions = new Dictionary<string, int[]>()
        {
             {"4d97884c3806a531ddb7288bf0eab418", new[]{1,3,5}},
             {"08116dcafc684462ea1948819475a81c", new[]{7,8}},
             {"389aa660266f0a8f76b5ef21c60cf6fd", new[]{1,2}},
             {"42c904d3cd20f55405a64fcf8032b92a", new[]{2,6,8}},
             {"2a819973e9e6e22eeb445f548201ab40", new[]{0,5}},
             {"4d9a9b459f0d3c67581c4990bda3257a", new[]{0,2,3}},
             {"2be5ec6995af4925299ed2fa635e4782", new[]{1,5,7}},
             {"61e0c2f52d510cc89b7432da01494a68", new[]{0,4}},
             {"2ea52cb78ba770b72149daa428331e98", new[]{4}},
             {"55bd1a0cc31c4d57654d927ca05b81a4", new[]{2,4,5}},
             {"681f0615747ba54f97040ef36dd2e6a0", new[]{1}},
             {"4fed27abf3b4fa6dad5cf1d852114a1e", new[]{2,7}},
             {"549f069a0189e73f43640a10f7be0de2", new[]{5,6,8}},
             {"d09f368da26b9ed9d583d61f0dd4b1dd", new[]{3}},
             {"2224eef78d48f63536bc7e0730ebfd54", new[]{1,6,7}},
             {"5055db4cab5e09eeeac0293ca44ebf65", new[]{1,2,7}},
             {"76a3a9ced6474f3db148568d2f396dd6", new[]{1,5,8}},
             {"50abf6c375ea3115168da3be0acc5485", new[]{5}},
             {"9329c0fecaece67da26a740d3519970b", new[]{0,1,8}},
             {"b04955d8598980df71c7b69ea3a8e7a2", new[]{2,7,8}},
             {"2ba5296ea4cb4bcd302f5a3b624ecf82", new[]{1,7,8}},
             {"93af8a552ecf9729493b5c9fea98c748", new[]{3,4,6}},
             {"5b9a9ae117ebe53e71d236ea3952b974", new[]{1,4,6}},
             {"b435d7145639469b151a6b01a0bfe1c6", new[]{2,5,8}},
             {"0635a32edc11e674f48dbbfbae98c969", new[]{3,7,8}},
             {"18eaa52fcf87e47edd684c8696aa1798", new[]{0,4,6}},
             {"49a857ed6a90225b7de5b9ed22ee2c8a", new[]{3,4}},
             {"3f86e8960a64f884aa45ecb696890f5c", new[]{0,1,8}},
             {"e785f87dec2b23818dbb8892ea48f91d", new[]{4,5}}
        };
    }
}