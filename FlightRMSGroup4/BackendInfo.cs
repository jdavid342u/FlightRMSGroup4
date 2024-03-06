using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightRMSGroup4
{
    public static class BackendInfo
    {
        public static List<Flight> Flights = Flight.LoadFlights();
        public static List<Airport> Airports = Airport.LoadAirports();

        public static string GetPath(string path)
        {
            List<string> DirectoryParts = AppDomain.CurrentDomain.BaseDirectory.Split(@"\").ToList();
            int projectIndex = DirectoryParts.FindLastIndex(x => x.ToLower() == "flightrmsgroup4");
            DirectoryParts.RemoveAll(x => DirectoryParts.IndexOf(x) > projectIndex);
            string output = "";
            foreach (string pathPart in DirectoryParts)
            {
                output += $@"{pathPart}\";
            }
            output += $@"{path}";

            return output;
        }

        public static List<Flight> QueryFlights(string airportOrigin, string airportDestination, string weekDay)
        {
            List<Flight> queriedFlights = BackendInfo.Flights.Where( f =>
                f.AirportOrigin == (airportOrigin == "ZZZ" ? f.AirportOrigin : airportOrigin)
                && f.AirportDestination == (airportDestination == "ZZZ" ? f.AirportDestination : airportDestination)
                && f.WeekDay == (weekDay == "Any" ? f.WeekDay : weekDay)
            ).ToList();

            return queriedFlights;
        }
    }
}
