using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Objects
{
    public class ModerationQueue
    {
        // item.post_id, item.message, item.vote_count, item.color, item.user_handle, item.task_id, item.flag_count, new Tuple<string, int>(item.parent_id, item.flag_reason
        public string PostId { get; set; }

        public string Message { get; set; }

        public int VoteCount { get; set; }

        public string HexColor { get; set; }

        public string UserHandle { get; set; }

        public int TaskId { get; set; }

        public int FlagCount { get; set; }

        public string ParentId { get; set; }

        public int FlagReason { get; set; }
    }
}
