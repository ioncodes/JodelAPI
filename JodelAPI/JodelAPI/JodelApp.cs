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
        #region Fields and Properties

        public int Karma { get; private set; }
        public Jodel MyJodel { get; private set; }
        public JodelMainData JodelPosts { get; set; }

        #endregion

        #region Constructor

        public JodelApp(Jodel jodel)
        {
            Karma = 0;
            MyJodel = jodel;
        }

        #endregion

        #region Methods

        public void Start()
        {
            MyJodel.GetUserConfig();
            MyJodel.GetRecommendedChannels();
            Karma = MyJodel.GetKarma();
            JodelPosts = MyJodel.GetPostLocationCombo(stickies: true);
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



        #endregion

        #endregion
    }
}
