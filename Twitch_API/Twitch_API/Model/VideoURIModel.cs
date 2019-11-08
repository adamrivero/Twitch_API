using Newtonsoft.Json;
using System;

namespace Twitch_API.Model
{
    class VideoURIModel
    {
        [JsonProperty("urls")]
        public Urls urls { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        public partial class Urls
        {
            [JsonProperty("480p")]
            public Uri The480P { get; set; }

            [JsonProperty("audio_only")]
            public Uri AudioOnly { get; set; }

            [JsonProperty("360p")]
            public Uri The360P { get; set; }

            [JsonProperty("1080p60")]
            public Uri The1080P60 { get; set; }

            [JsonProperty("720p60")]
            public Uri The720P60 { get; set; }

            [JsonProperty("160p")]
            public Uri The160P { get; set; }

            [JsonProperty("720p")]
            public Uri The720P { get; set; }
        }

    }
}
