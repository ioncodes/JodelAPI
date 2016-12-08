using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Shared
{
    public class JodelMainData
    {
        #region Fields and Properties

        public int Max { get; set; }

        public List<JodelPost> RecentJodels { get; set; }
        public List<JodelPost> RepliedJodels { get; set; }
        public List<JodelPost> VotedJodels { get; set; }

        #endregion

        #region Constructor

        public JodelMainData()
        {
            RecentJodels = new List<JodelPost>();
            RepliedJodels = new List<JodelPost>();
            VotedJodels = new List<JodelPost>();
        }

        #endregion
    }
}
