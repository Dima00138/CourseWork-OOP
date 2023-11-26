using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public long? IdTrain { get; set; }
        public DateTime Date { get; set; }
        public long Route { get; set; }
        private string _frequency = string.Empty;

        public void SetFrequency(short frequency)
        {
            switch (frequency)
            {
                case 1: _frequency = "Каждый день"; break;
                case 2: _frequency = "Каждый нечетный день"; break;
                case 3: _frequency = "Каждый четный день"; break;
                case 4: _frequency = "Единожды"; break;
            }
        }

        public short GetFrequency()
        {
            switch (_frequency)
            {
                case "Каждый день": return 1;
                case "Каждый нечетный день": return 2;
                case "Каждый четный день": return 3;
                case "Единожды": return 4;
            }
            return -1;
        }
    }
}
