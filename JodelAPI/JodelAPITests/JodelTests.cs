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
            string accessToken = Account.GenerateAccessToken(location.Latitude, location.Longitude, "CH", "Baden").AccessToken;
            User user = new User(accessToken, location.Latitude, location.Longitude, "CH", "Baden");
            return new Jodel(user);
        }

        [TestMethod()]
        public void GetAllJodelsTest()
        {
            GetJodelObject().GetAllJodels();
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
            jodel.Downvote(jodel.GetAllJodels()[0].PostId);
        }

        [TestMethod()]
        public void PostJodelTest()
        {
            GetJodelObject().PostJodel("Test");
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
    }
}