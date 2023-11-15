using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class StationsRoute
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int StationId { get; set; }
        public int StationOrder { get; set; }
    }
}
