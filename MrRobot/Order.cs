using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrRobot
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Url { get; set; }
        public byte Force { get; set; }
        public byte Interval { get; set; }
        public byte StartHour { get; set; }
        public byte EndHour{ get; set; }
        public Guid NextId { get;   set; }
    }
}
