using System;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;


namespace BE
{
    public class Report : ICloneable
    {
        [Key]
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Time { get; set; }
        public int NumOfExplosions { get; set; }
        public Event Event { get; set; }
        public int ClusterId { get; set; }

        public GeoCoordinate GetCoordinate()
        {
            return new GeoCoordinate(Latitude, Longitude);
        }

        public Report()
        {
            Time = DateTime.Now;
        }

        public object Clone()
        {
            return new Report()
            {
                Id = Id,
                Name = Name,
                Address = Address,
                Latitude = Latitude,
                Longitude = Longitude,
                Time = new DateTime(Time.Ticks),
                NumOfExplosions = NumOfExplosions,
                Event = Event?.Clone() as Event,
                ClusterId = ClusterId,
            };
        }
    }
}
