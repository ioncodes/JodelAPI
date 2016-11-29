using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Objects;

namespace JodelAPI
{
    /// <summary>
    /// Simulates real use of the JodelApp on Android
    /// </summary>
    public class JodelApp
    {
        public Jodel Jodel { get; private set; }
        public int Karma { get; private set; }

        public JodelApp(string accessToken, string longitude, string latitude, string city, string countryCode, string googleApiToken = "")
            : this(new User(accessToken, latitude, longitude, countryCode, city, googleApiToken)) { }

        public JodelApp(User user)
        {
            Jodel = new Jodel(user);
        }

        /// <summary>
        /// Does all what Jodel does on opening
        /// </summary>
        public void StartJodel()
        {
            //GetUserConfig
            Jodel.GetUserConfig();

            //LoadFollowedChannels
            Jodel.LoadFollowedChannels();

            //LoadRecommendedChannels
            Jodel.GetRecommendedChannels();

            //LoadKarma
            Karma = Jodel.Account.GetKarma();

            //LoadFirstJodels
            Jodel.GetFirstJodels();
        }

        public List<Jodels> ReloadInMain()
        {
            //LoadFirstJodels
            var jodels = Jodel.GetFirstJodels();

            //LoadKarma
            Karma = Jodel.Account.GetKarma();

            return jodels;
        }

        public List<MyJodels> ReloadInMyJodels()
        {
            //LoadMine
            var jodels = Jodel.GetMyJodels();

            //LoadKarma
            Karma = Jodel.Account.GetKarma();

            return jodels;
        }

        public List<MyComments> ReloadInMyComments()
        {
            //LoadMyComments
            var comments = Jodel.GetMyComments();

            //LoadKarma
            Karma = Jodel.Account.GetKarma();

            return comments;
        }

        public List<MyVotes> ReloadInMyVotes()
        {
            //LoadMyResponses
            var votes = Jodel.GetMyVotes();

            //LoadKarma
            Karma = Jodel.Account.GetKarma();

            return votes;
        }

        public List<MyPins> ReloadInMyPins()
        {
            //LoadMyResponses
            var pins = Jodel.GetMyPins();

            //LoadKarma
            Karma = Jodel.Account.GetKarma();

            return pins;
        }

        public List<ChannelJodel> ReloadInChannel(Jodel.Channel channel)
        {
            //LoadKarma
            Karma = Jodel.Account.GetKarma();

            //LoadChannelPosts
            return channel.GetJodels();
        }
    }
}
