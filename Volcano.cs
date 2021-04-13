using System.Collections.Generic;

namespace VolcanoAPI
{
    public class Location
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Volcano
    {
        public string VolcanoName { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public Location Location { get; set; }
        public int? Elevation { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string LastKnownEruption { get; set; }
        public string id { get; set; }
        public string _rid { get; set; }
        public string _self { get; set; }
        public string _etag { get; set; }
        public string _attachments { get; set; }
        public int _ts { get; set; }
    }
}