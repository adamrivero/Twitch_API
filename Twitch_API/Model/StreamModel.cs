using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_API.Model
{
    class StreamModel
    {
        public List<UserData> data { get; set; }
        public class UserData : StreamModel
        {
            public string user_name { get; set; }
            public string title { get; set; }
            public int user_id { get; set; }
            public int viewer_count { get; set; }
        }
    }
}
