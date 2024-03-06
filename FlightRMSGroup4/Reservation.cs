using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightRMSGroup4
{
    public class Reservation
    {
        public string ReservationCode { get; set; }
        public string FlightCode { get; set; }
        public string Name { get; set; }
        public string Citizenship { get; set; }
        public bool IsActive { get; set; }

        public Reservation(string FlightCode, string Name, string Citizenship)
        {
            this.ReservationCode = null;
            this.FlightCode = FlightCode;
            this.Name = Name;
            this.Citizenship = Citizenship;
        }
    }
}
