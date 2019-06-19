using System;
using System.ComponentModel.DataAnnotations;
using System.Device.Location;

namespace BE
{
    public class Explosion : ICloneable
    {
        [Key]
        public int? Id { get; set; }
        public Event Event { get; set; }
        public double ApproxLatitude { get; set; }
        public double ApproxLongitude { get; set; }

        public object Clone()
        {
            return new Explosion()
            {
                Id = Id,
                Event = Event?.Clone() as Event,
                ApproxLatitude = ApproxLatitude,
                ApproxLongitude = ApproxLongitude
            };
        }
    }
}