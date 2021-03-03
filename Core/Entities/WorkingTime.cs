using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class WorkingTime
    {
        [Key]
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public int AtrractionId { get; set; }
        public Attraction Atrraction { get; set; }
    }
}
