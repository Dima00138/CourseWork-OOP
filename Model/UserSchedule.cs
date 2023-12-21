using System;

namespace CourseWork.Model
{
    public class UserSchedule
    {
        public long? Id { get; set; }
        public long? IdTrain { get; set; }
        public string CategoryOfTrain { get; set; } = string.Empty;
        public string DeparturePoint { get; set; } = string.Empty;
        public string DepartureCity { get; set; } = string.Empty;
        public string ArrivalPoint { get; set; } = string.Empty;
        public string ArrivalCity { get; set; } = string.Empty;
        public long Distance { get; set; }
        public long Duration { get; set; }
        public DateTime Date { get; set; }
        public string Frequency { get; set; } = string.Empty;
        private bool IsForPassengers { get; set; }


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
