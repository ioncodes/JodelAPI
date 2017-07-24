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
        Jodel jodel = new Jodel("Zuerich Schweiz", "CH", "Zuerich");


        [TestMethod()]
        public void GetKarmaTest()
        {
            int karma = jodel.GetKarma();
        }

        [TestMethod()]
        public void GetRecommendedChannelsTest()
        {
            Assert.IsNotNull(jodel.GetRecommendedChannels());
        }

        [TestMethod()]
        public void GetPostLocationComboTest()
        {
            Assert.IsNotNull(jodel.GetPostLocationCombo());
        }

        [TestMethod()]
        public void ShareUrlProperty()
        {
            var combo = jodel.GetPostLocationCombo();
            string url = combo.RecentJodels[1].ShareUrl;
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            Assert.IsTrue(result);
        }
    }
}