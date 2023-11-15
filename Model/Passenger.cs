using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Passenger
    {
        public int Id { get; set; }
        public string? Passport { get; set; }
        public short Benefits { get; set; }
        public string? FullName { get; set; }
    }
}
