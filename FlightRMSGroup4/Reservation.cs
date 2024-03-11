using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightRMSGroup4
{
    public class Reservation
    {
        [System.Text.Json.Serialization.JsonPropertyName("reservationCode")]
        public string ReservationCode { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("flightCode")]
        public string FlightCode { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("name")]
        public string Name { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("citizenship")]
        public string Citizenship { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
        public string Label { get; set; }

        public Reservation(string ReservationCode, string FlightCode, string Name, string Citizenship)
        {
            this.ReservationCode = ReservationCode;
            this.FlightCode = FlightCode;
            this.Name = Name;
            this.Citizenship = Citizenship;
            this.IsActive = true; /* Active by default */
            this.Label = $"Reservation {ReservationCode} of {Name} ({Citizenship}) for Flight {FlightCode} by {ReservationManager.GetFlightByCode(FlightCode).Airline}";
        }
    }
}
