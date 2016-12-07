using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JodelAPI.Json.Request
{
    public abstract class JsonRequest
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
