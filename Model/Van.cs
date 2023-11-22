using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Van
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public int Capacity { get; set; }
        public bool IsFree { get; set; }

        public Van this[int i]
        {
            get { return this[i]; }
            set { this[i] = value; }
        }
    }
}
