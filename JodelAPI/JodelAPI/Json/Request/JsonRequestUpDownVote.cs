using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI.Json.Request
{
    internal class JsonRequestUpDownVote:JsonRequest
    {
        #region

        public int reason_code { get; set; } = -1;

        #endregion
    }
}
