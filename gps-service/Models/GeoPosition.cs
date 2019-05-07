namespace gps_service.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class GeoPosition
    {
        [Key]
        public Guid UID { get; set; }


        public GeoPosition() { }

        public GeoPosition(float lat, float lon)
        {
            this.Latitude = lat;
            this.Longitude = lon;
        }
        public GeoPosition(float lat, float lon, string sid) : this(lat, lon) => this.SessionID = sid;


        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string SessionID { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public StateOf State { get; set; } = StateOf.NotProcessed;


        public static implicit operator GeoPosition((float lat, float lon) raw) => new GeoPosition(raw.lat, raw.lon);
    }

    public enum StateOf
    {
        NotProcessed,
        Complete,
        Rejected
    }
}