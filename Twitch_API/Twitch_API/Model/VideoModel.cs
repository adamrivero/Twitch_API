using Newtonsoft.Json;

namespace Twitch_API.Model
{
    public class VideoModel
    {
        public Urls urls { get; set; }
        public bool success { get; set; }
    }
    public class Urls
    {
        [JsonProperty("480p")]
        public string _480p { get; set; }
        public string audio_only { get; set; }
        [JsonProperty("360p")]
        public string _360p { get; set; }
        [JsonProperty("1080p60")]
        public string _1080p60 { get; set; }
        [JsonProperty("720p60")]
        public string _720p60 { get; set; }
        [JsonProperty("160p")]
        public string _160p { get; set; }
        [JsonProperty("720p")]
        public string _720p { get; set; }
    }

}
