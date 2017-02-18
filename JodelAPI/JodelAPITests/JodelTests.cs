using Microsoft.VisualStudio.TestTools.UnitTesting;
using JodelAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Shared;

namespace JodelAPI.Tests
{
    [TestClass()]
    public class JodelTests
    {
        Jodel jodel = new Jodel("Baden Aargau Schweiz", "CH", "Baden");

        [TestMethod()]
        public void GenerateAccessTokenTest()
        {
            Assert.IsTrue(jodel.GenerateAccessToken());
        }

        [TestMethod()]
        public void RefreshAccessTokenTest()
        {
            Assert.IsTrue(jodel.GenerateAccessToken());
            Assert.IsTrue(jodel.RefreshAccessToken());
        }

        [TestMethod()]
        public void GetKarmaTest()
        {
            Assert.IsTrue(jodel.GenerateAccessToken());
            int karma = jodel.GetKarma();
        }

        [TestMethod()]
        public void GetRecommendedChannelsTest()
        {
            Assert.IsTrue(jodel.GenerateAccessToken());
            Assert.IsNotNull(jodel.GetRecommendedChannels());
        }

        [TestMethod()]
        public void GetPostLocationComboTest()
        {
            Assert.IsTrue(jodel.GenerateAccessToken());
            Assert.IsNotNull(jodel.GetPostLocationCombo());
        }

        [TestMethod()]
        public void GetCaptcha()
        {
            Assert.IsNotNull(jodel.GetCaptcha());
            Console.WriteLine(jodel.GetCaptcha().ImageUrl + ":" + jodel.GetCaptcha().Key);
        }

        [TestMethod()]
        public void SolveCaptcha()
        {
            var captcha = jodel.GetCaptcha();
            Assert.IsNotNull(captcha);
            Console.WriteLine(captcha.ImageUrl + ":" + captcha.Key);
            Assert.IsInstanceOfType(jodel.SolveCaptcha(captcha, new[] {1}), typeof(bool));
        }
    }
}