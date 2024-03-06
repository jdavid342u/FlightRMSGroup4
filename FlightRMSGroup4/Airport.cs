using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightRMSGroup4
{
    public class Airport
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public Airport(string Code, string Name)
        {
            this.Code = Code;
            this.Name = Name;
        }

        public static List<Airport> LoadAirports()
        {
            List<Airport> output = new List<Airport>();
            string path = BackendInfo.GetPath(@"Resources\airports.csv");
            string[] AirportRawStrings = File.ReadAllLines(path);

            foreach (string rawStr in AirportRawStrings)
            {
                string[] airportAttributes = rawStr.Split(",");
                if (airportAttributes.Length != 2) { continue; }

                output.Add(new Airport(airportAttributes[0], airportAttributes[1]));
            }

            return output;
        }
    }
}
