using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Shared;

namespace JodelAPI
{
    /// <summary>
    /// Simulates real use of the JodelApp on Android
    /// </summary>
    public class JodelApp
    {
        #region Fileds and Properties

        public int Karma { get; private set; }
        public Jodel MyJodel { get; private set; }
        public JodelMainData JodelPosts { get; set; }

        #endregion

        #region Constructor

        public JodelApp(Jodel jodel)
        {
            Karma = 0;
            this.MyJodel = jodel;
        }

        #endregion

        #region Methods

        public void Start()
        {
            MyJodel.GetConfig();
            MyJodel.GetRecommendedChannels();
            this.Karma = MyJodel.GerKarma();
            JodelPosts = MyJodel.GetPostLocationCombo();
            MyJodel.GetFollowedChannelsMeta();
        }

        public bool RefreshToken()
        {
            return MyJodel.RefrashAccessToken();
        }

        public bool GenerateToken()
        {
            return MyJodel.GenerateAccessToken();
        }

        #endregion
    }
}
