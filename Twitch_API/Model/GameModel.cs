using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_API.Model
{
    class GameModel
    {
        public List<Datum> data { get; set; }
        public class Datum
        {
            public string id { get; set; }
            public string name { get; set; }
            public string box_art_url { get; set; }
        }

    }
}
