using Microsoft.VisualStudio.TestTools.UnitTesting;
using JodelAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Objects;

namespace JodelAPI.Tests
{
    [TestClass()]
    public class JodelTests
    {
        public Jodel GetJodelObject()
        {
            var location = Location.GetCoordinates("Baden, Aargau, Schweiz");
            Assert.IsNotNull(location);
            string accessToken = Account.GenerateAccessToken(location.Latitude, location.Longitude, "CH", "Baden").AccessToken;
            Assert.IsTrue(accessToken.Length > 0);
            User user = new User(accessToken, location.Latitude, location.Longitude, "CH", "Baden");
            Assert.IsNotNull(user);
            Jodel jodel = new Jodel(user);
            Assert.IsNotNull(jodel);
            return jodel;
        }

        [TestMethod()]
        public void GetAllJodelsTest()
        {
            List<Jodels> jodels = GetJodelObject().GetAllJodels();
            Assert.IsNotNull(jodels);
        }

        [TestMethod()]
        public async Task GetAllJodelsAsyncTest()
        {
            List<Jodels> jodels = await GetJodelObject().GetAllJodelsAsync();
            Assert.IsNotNull(jodels);
        }

        [TestMethod()]
        public void UpvoteTest()
        {
            Jodel jodel = GetJodelObject();
            jodel.Upvote(jodel.GetAllJodels()[0].PostId);
        }

        [TestMethod()]
        public void DownvoteTest()
        {
            Jodel jodel = GetJodelObject();
            jodel.Downvote(jodel.GetAllJodels()[1].PostId);
        }

        [TestMethod()]
        public void PostJodelTest()
        {
            string postid = GetJodelObject().PostJodel("Test");
            Assert.IsTrue(postid.Length > 0);
        }

        [TestMethod()]
        public void DeleteJodelTest()
        {
            Jodel jodel = GetJodelObject();
            string postid = jodel.PostJodel("Test");
            jodel.DeleteJodel(postid);
        }

        [TestMethod()]
        public void GetKarmaTest()
        {
            GetJodelObject().Account.GetKarma();
        }

        [TestMethod()]
        public void SetLocationTest()
        {
            var location = Location.GetCoordinates("Baden, Aargau, Schweiz");
            GetJodelObject().Account.SetUserLocation(Account.GenerateAccessToken(location.Latitude, location.Longitude, "CH", "Baden").AccessToken);
        }
    }
}