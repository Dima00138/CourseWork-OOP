using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Route
    {
        public int Id { get; set; }
        public int DeparturePoint { get; set; }
        public int ArrivalPoint { get; set; }
        public int Distance { get; set; }
        public int Duration { get; set; }
    }
}
