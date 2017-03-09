using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Shared
{
    public class Channel
    {
        #region Fields and Properties

        public string ChannelName { get; private set; }

        public bool Following { get; private set; }

        public string ImageUrl { get; set; }

        public int Followers { get; set; }

        public bool Sponsored { get; set; }

        public bool Unread { get; set; }

        #endregion

        #region Constructor


        public Channel(string channelName, bool following = false)
        {
            this.ChannelName = channelName;
            this.Following = following;
        }

        #endregion

        #region Methods

        internal Channel UpdateProperties(string imageUrl, int followers)
        {
            this.ImageUrl = imageUrl;
            this.Followers = followers;
            return this;
        }
        internal Channel UpdateProperties(int followers, bool sponsored, bool unread)
        {
            this.Unread = unread;
            this.Followers = followers;
            this.Sponsored = sponsored;
            return this;
        }

        #endregion
    }
}
