using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace BE
{
    public class Event : ICloneable
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Explosion> Explosions { get; set; } = new List<Explosion>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();

        public Event() { }

        public Event(DateTime startTime)
        {
            StartTime = new DateTime(startTime.Ticks);
            EndTime = StartTime.AddMinutes(10);
        }

        public Object Clone()
        {
            return new Event(StartTime)
            {
                Id = Id,
                EndTime = new DateTime(EndTime.Ticks),
                Explosions = (from explosion in Explosions select explosion?.Clone() as Explosion).ToList(),
                Reports = (from report in Reports select report?.Clone() as Report).ToList(),
            };
        }

        public override string ToString()
        {
            return Id + ": " + StartTime;
        }
    }
}
