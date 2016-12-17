using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Shared
{
    public class User
    {
        #region Fields and Properties


        public Location Place { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public AccessToken Token { get; set; }

        #region UserProperties

        public bool Moderator { get; set; }
        public object UserType { get; set; }
        public List<Experiment> Experiments { get; set; } = new List<Experiment>();
        public List<Channel> FollowedChannels { get; set; } = new List<Channel>();
        public List<string> FollowedHashtags { get; set; } = new List<string>();
        public int ChannelsFollowLimit { get; set; }
        public bool TripleFeedEnabled { get; set; }
        public string HomeName { get; set; }
        public bool HomeSet { get; set; }
        public string Location { get; set; }
        public bool Verified { get; set; }

        #endregion

        #endregion

        #region Constructor

        public User()
        {
            this.Token = new AccessToken(this);
        }

        public User(string deviceUid, string accessToken)
        {
            this.Token = new AccessToken(this, deviceUid, accessToken);
        }

        public User(AccessToken token)
        {
            this.Token = token;
        }

        #endregion

        #region UserClasses

        public class Experiment
        {
            #region Fields and Properties

            public string Name { get; private set; }
            public string Group { get; private set; }
            public List<string> Features { get; private set; }

            #endregion

            #region Constructor

            internal Experiment(string name, string group, List<string> features)
            {
                this.Name = name;
                this.Group = group;
                this.Features = features;
            }

            #endregion
        }

        #endregion
    }
}
