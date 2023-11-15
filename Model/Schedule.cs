using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Schedule
    {
        public long? Id { get; set; }
        public long? IdTrain { get; set; }
        public DateTime Date { get; set; }
        public long Route { get; set; }
        public short Frequency { get; set; }

    }
}
