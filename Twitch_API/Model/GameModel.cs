using System.Collections.Generic;

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
