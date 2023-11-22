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
        public string Frequency { get; set; } = string.Empty;

        public void SetFrequency(short frequency)
        {
            switch (frequency)
            {
                case 1: Frequency = "Каждый день"; break;
                case 2: Frequency = "Каждый нечетный день"; break;
                case 3: Frequency = "Каждый четный день"; break;
                case 4: Frequency = "Единожды"; break;
            }
        }
    }
}
