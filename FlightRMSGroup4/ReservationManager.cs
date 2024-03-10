using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlightRMSGroup4
{
    public class ReservationManager
    {
        private static List<Reservation> _reservations = initializeReservationList();

        public ReservationManager()
        {
            _reservations = initializeReservationList(); // Initialize list
        }

        public static JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        
        public static void MakeReservation(string reservationCode, Flight flight, string name, string citizenship)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(flight), "Flight cannot be null");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be empty or null");
            }

            if (string.IsNullOrEmpty(citizenship))
            {
                throw new ArgumentException("Citizenship cannot be empty or null");
            }

            if (flight.IsFullyBooked)
            {
                throw new Exception("Flight is fully booked!");
            }

            Reservation reservation = new Reservation(reservationCode, flight.Code, name, citizenship);
            _reservations.Add(reservation); // Add reservation to list
            File.WriteAllText(BackendInfo.GetPath(["Resources", "reservations.json"]), JsonSerializer.Serialize(_reservations, _options));
            flight.ReserveSeat(flight);
        }

        public static List<Reservation> FindReservations(string reservationCode = null, string airline = null, string name = null)
        {
            var filteredReservations = _reservations.Where(r =>
                (string.IsNullOrEmpty(reservationCode) || r.ReservationCode == reservationCode) &&
                (string.IsNullOrEmpty(airline) || r.FlightCode == GetFlightByCode(r.FlightCode)?.Airline) &&
                (string.IsNullOrEmpty(name) || r.Name.Contains(name)));
            return filteredReservations.ToList();
        }

        // Helper method to retrieve Flight object based on FlightCode from Reservation
        private static Flight GetFlightByCode(string flightCode)
        {
            // Need to impliment Flight data, well return null for now
            return null;
        }

        public static string UniqueReservationCode()
        {
            string reservationCode;
            do
            {
                reservationCode = RandomReservationCode();
            } while (_reservations.Any(r => r.ReservationCode == reservationCode));

            return reservationCode;
        }

        private static string RandomReservationCode()
        {
            Random random = new Random();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string digits = "0123456789";

            char letter = letters[random.Next(letters.Length)];
            string randomNumber = "";
            for (int i = 0; i < 4; i++)
            {
                randomNumber += digits[random.Next(digits.Length)];
            }

            return letter.ToString() + randomNumber;
        }

        public static List<Reservation> initializeReservationList()
        {
            List<Reservation> reservations = new List<Reservation>();

            if (File.Exists(BackendInfo.GetPath(["Resources", "reservations.json"])))
            {
                string jsonStr = File.ReadAllText(BackendInfo.GetPath(["Resources", "reservations.json"]));
                
                List<Reservation> reservationsFromFile = JsonSerializer.Deserialize<List<Reservation>>(jsonStr);
                return reservationsFromFile;
            }

            return reservations;
        }
    }
}
