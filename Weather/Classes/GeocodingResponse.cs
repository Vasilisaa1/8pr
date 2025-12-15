using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Weather.Models
{
    public class GeocodingResponse
    {
        [JsonProperty("response")]
        public ResponseData Response { get; set; }
    }

    public class ResponseData
    {
        [JsonProperty("GeoObjectCollection")]
        public GeoObjectCollection GeoObjectCollection { get; set; }
    }

    public class GeoObjectCollection
    {
        [JsonProperty("featureMember")]
        public List<FeatureMember> FeatureMember { get; set; }
    }

    public class FeatureMember
    {
        [JsonProperty("GeoObject")]
        public GeoObject GeoObject { get; set; }
    }

    public class GeoObject
    {
        [JsonProperty("Point")]
        public Point Point { get; set; }
    }

    public class Point
    {
        [JsonProperty("pos")]
        public string Position { get; set; }
    }
}