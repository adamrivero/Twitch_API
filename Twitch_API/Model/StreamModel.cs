using System;
using System.Collections;
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
        public List<Datum> data { get; set; }
        public Pagination pagination { get; set; }
    }
    public class Datum
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public int game_id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public int viewer_count { get; set; }
        public DateTime started_at { get; set; }
        public string language { get; set; }
        public string thumbnail_url { get; set; }
        public List<string> tag_ids { get; set; }
    }

    public class Pagination
    {
        public string cursor { get; set; }
    }
}
