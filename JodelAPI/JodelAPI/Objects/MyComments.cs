using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Objects
{
    public class MyComments
    {
        public string PostId { get; set; }

        public string Message { get; set; }

        public string UserHandle { get; set; }

        public int VoteCount { get; set; }
    }
}
