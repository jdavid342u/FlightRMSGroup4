using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightRMSGroup4
{
    public class Flight
    {
        public string Code { get; set; }
        public string Airline { get; set; }
        public string AirportOrigin { get; set; }
        public string AirportDestination { get; set; }
        public string WeekDay { get; set; }
        public string DepartureTime { get; set; }
        public int ReservationsLeft { get; set; }
        public double Cost { get; set; }
        public string Label { get; set; }
        public bool IsFullyBooked {
            get
            {
                if(ReservationsLeft <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Flight(string Code, string Airline, string AirportOrigin, string AirportDestination, string WeekDay, string DepartureTime, int ReservationsLeft, double Cost)
        {
            this.Code = Code;
            this.Airline = Airline;
            this.AirportOrigin = AirportOrigin;
            this.AirportDestination = AirportDestination;
            this.WeekDay = WeekDay;
            this.DepartureTime = DepartureTime;
            this.ReservationsLeft = ReservationsLeft;
            this.Cost = Cost;
            this.Label = $"{Code} ({WeekDay}): {AirportOrigin} -> {AirportDestination} by {Airline}";
        }

        public void ReserveSeat(Flight f)
        {
            f.ReservationsLeft--;
            BackendInfo.UpdateFlightsCsvFile();
        }

        public void AddSeat(Flight f)
        {
            f.ReservationsLeft++;
            BackendInfo.UpdateFlightsCsvFile();
        }
        
        public static List<Flight> LoadFlights()
        {
            List<Flight> output = new List<Flight>();
            string path = BackendInfo.GetPath(["Resources", "flights.csv"]);
            string[] FlightRawStrings = File.ReadAllLines(path);

            foreach(string rawStr in FlightRawStrings)
            {
                string[] flightAttributes = rawStr.Split(",");
                if(flightAttributes.Length != 8) { continue; }

                int reservationsLeft;
                double cost;

                if (!int.TryParse(flightAttributes[6], out reservationsLeft)) { continue; }
                if (!double.TryParse(flightAttributes[7], out cost)) { continue; }

                output.Add(new Flight(flightAttributes[0], flightAttributes[1], flightAttributes[2], flightAttributes[3], flightAttributes[4], flightAttributes[5], reservationsLeft, cost));
            }

            return output;
        }
    }
}
