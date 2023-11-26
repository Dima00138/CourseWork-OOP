using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Train
    {
        public int Id { get; set; }
        public string? CategoryOfTrain { get; set; }
        public bool IsForPassengers { get; set; }
        public string? Vans { get; set; }
        public int ParkingTime { get; set; }
        public int CountOfVans { get; set; }
    }
}
