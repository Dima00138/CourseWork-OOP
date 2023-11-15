using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    //!TODO Pattern State
    public class Payment
    {
        public int Id { get; set; }
        public int IdTicket { get; set; }
        public DateTime DatePay { get; set; }
        public int PaymentAmount { get; set; }
        public char Status { get; set; }
    }
}
