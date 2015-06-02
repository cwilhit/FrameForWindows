using Microsoft.WindowsAzure.Mobile.Service;
using System;

namespace frameService.DataObjects
{
    public class MediaContent : EntityData
    {
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName="timestamp")]
        public DateTime Timestamp { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName="rating")]
        public int Rating { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName="flagcount")]
        public int FlagCount { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName="latitude")]
        public double Latitude { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName="longitude")]
        public double Longitude { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName="mediacontent")]
        public string mediaContent { get; set; }
    }
}