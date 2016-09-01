using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Objects
{
    public class ChannelJodel
    {
        public string Message { get; set; }
        public string PostId { get; set; }
        public bool IsOwn { get; set; }
        public int PinCount { get; set; }
        public int VoteCount { get; set; }
    }
}
