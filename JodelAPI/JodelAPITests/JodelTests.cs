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
        public void GetFollowedChannelsMetaTest()
        {
            Assert.IsTrue(jodel.GenerateAccessToken());
            Assert.IsNotNull(jodel.GetFollowedChannelsMeta());
        }

        [TestMethod()]
        public void GetPostLocationComboTest()
        {
            Assert.IsTrue(jodel.GenerateAccessToken());
            Assert.IsNotNull(jodel.GetPostLocationCombo());
        }
    }
}