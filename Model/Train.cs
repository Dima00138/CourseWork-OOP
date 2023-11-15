using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    class Train
    {
        public string? Id { get; set; }
        public string? CategoryOfTrain { get; set; }
        public char IsForPassengers { get; set; }
        public Van[]? Vans { get; set; }
        public int ParkingTime { get; set; }
        public int CountOfVans { get; set; }
    }
}
