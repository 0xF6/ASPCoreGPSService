namespace gps_service.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Core;

    public class ProcessedGeo
    {
        public ProcessedGeo(GeoPosition p1, GeoPosition p2) => Pairs = (p1, p2);
        public ProcessedGeo(GeoPosition p1, GeoPosition p2, string sid) : this(p1, p2) => SessionID = sid;

        public (GeoPosition pos1, GeoPosition pos2) Pairs { get; set; }
        public float Distance => Geo.Distance(Pairs.pos1, Pairs.pos2);
        public string SessionID { get; set; }
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
    }
}