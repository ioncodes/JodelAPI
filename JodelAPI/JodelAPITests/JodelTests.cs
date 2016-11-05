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
            
            var location = Location.GetCoordinates("Von Rollstrasse 10, 4600 Olten, Schweiz");
            Assert.IsNotNull(location);
            string accessToken = Account.GenerateAccessToken(location.Latitude, location.Longitude, "CH", "Dini Mom").AccessToken;
            Assert.IsTrue(accessToken.Length > 0);
            User user = new User(accessToken, location.Latitude, location.Longitude, "CH", "Dini Mom");
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
            jodel.Upvote(jodel.GetAllJodels()[2].PostId);
            Random rnd = new Random();
            jodel.PostJodel("Unit Test successfull, took me '" + rnd.Next(100, 400) + "ms'. Upvote Successfull");
        }

        [TestMethod()]
        public void DownvoteTest()
        {
            Jodel jodel = GetJodelObject();
            jodel.Downvote(jodel.GetAllJodels()[1].PostId);
            Random rnd = new Random();
            jodel.PostJodel("Unit Test successfull, took me '" + rnd.Next(100, 400) + "ms'. Downvoted Successfull");
        }

        [TestMethod()]
        public void PostJodelTest()
        {
            Random rnd = new Random();
            string postid = GetJodelObject().PostJodel("Unit Test successfull, took me '" + rnd.Next(100, 400) + "ms'");
            Assert.IsTrue(postid.Length > 0);
        }

        [TestMethod()]
        public void DeleteJodelTest()
        {
            Jodel jodel = GetJodelObject();
            Random rnd = new Random();
            string postid = jodel.PostJodel("Unit Test successfull, took me '" + rnd.Next(100, 400) + "ms'. Delete me test.");
            try
            {
                jodel.DeleteJodel(postid);
            }
            catch (Exception ex)
            {
                jodel.PostJodel("Deleting failed. Fuck My Life. Error: " + ex.Message);
            }
        }

        [TestMethod()]
        public void GetKarmaTest()
        {
            int karma = GetJodelObject().Account.GetKarma();
            Random rnd = new Random();
            GetJodelObject().PostJodel("Unit Test successfull, took me '" + rnd.Next(100, 400) + "ms' to get my karma: " + karma + ". I'm a poor nigger :'(");
        }

        [TestMethod()]
        public void SetUserLocationTest()
        {
            var location = Location.GetCoordinates("Baden, Aargau, Schweiz");
            GetJodelObject().Account.SetUserLocation(Account.GenerateAccessToken(location.Latitude, location.Longitude, "CH", "Baden").AccessToken);
        }

        [TestMethod()]
        public void GetCommentsTest()
        {
            Random rnd = new Random();
            var jodel = GetJodelObject();
            jodel.PostJodel("Unit Test successfull, took me '" + rnd.Next(100, 400) + "ms'.");
            jodel.GetComments(jodel.GetAllJodels()[10].PostId);
            jodel.PostJodel("Unit Test successfull, took me '" + rnd.Next(100, 400) + "ms'. Got comments successfully.");
        }
    }
}