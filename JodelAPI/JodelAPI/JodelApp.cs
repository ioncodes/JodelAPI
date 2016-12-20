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
            MyJodel.GetUserConfig();
            MyJodel.GetRecommendedChannels();
            MyJodel.SetLocation();
            this.Karma = MyJodel.GetKarma();
            JodelPosts = MyJodel.GetPostLocationCombo(stickies: true);
            MyJodel.GetFollowedChannelsMeta();
        }

        public bool RefreshToken()
        {
            return MyJodel.RefreshAccessToken();
        }

        public bool GenerateToken()
        {
            return MyJodel.GenerateAccessToken();
        }

        #region Reload

        public JodelMainData ReloadMain()
        {
            Karma = MyJodel.GetKarma();
            JodelPosts = MyJodel.GetPostLocationCombo();

            return JodelPosts;
        }

        /// <summary>
        /// Loads more Jodels
        /// </summary>
        /// <param name="postId"></param>
        /// <returns>The loaded Posts</returns>
        public IEnumerable<JodelPost> LoadMoreRecentPosts(string postId = "")
        {
            List<JodelPost> posts = MyJodel.GetRecentPostsAfter(string.IsNullOrWhiteSpace(postId) ? JodelPosts.RecentJodels.Last().PostId : postId).ToList();
            JodelPosts.RecentJodels.AddRange(posts);
            return posts;
        }

        #endregion

        #region Posts

        public void Upvote(string postId)
        {
            MyJodel.Upvote(postId);
        }

        public void Downvote(string postId)
        {
            MyJodel.Downvote(postId);
        }

        public string Post(string message, JodelPost.PostColor color = JodelPost.PostColor.Random, bool home = false)
        {
            string postId = MyJodel.Post(message, color: color, home: home);
            JodelPosts = MyJodel.GetPostLocationCombo();
            return postId;
        }

        #endregion

        #endregion
    }
}
