using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Ticket
    {
        public int Id { get; set; }
        public int IdPassenger { get; set; }
        public string? IdTrain { get; set; }
        public int IdVan { get; set; }
        public int SeatNumber { get; set; }
        public string? FormWhere { get; set; }
        public string? ToWhere { get; set; }
        public DateTime Date { get; set; }
    }
}
