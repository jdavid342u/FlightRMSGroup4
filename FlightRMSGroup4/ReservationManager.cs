using System;
using System.Collections.Generic;

namespace FlightRMSGroup4
{
    public class ReservationManager
    {
        private static List<Reservation> _reservations = new List<Reservation>();

        public ReservationManager()
        {
            _reservations = new List<Reservation>(); // Initialize list
        }

        public static void MakeReservation(Flight flight, string name, string citizenship)
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

            string resCode = "123";

            flight.ReserveSeat(flight);

            var reservation = new Reservation(resCode, flight.Code, name, citizenship);
            _reservations.Add(reservation); // Add reservation to internal list
        }

        public List<Reservation> FindReservations(string reservationCode = null, string airline = null, string name = null)
        {
            var filteredReservations = _reservations.Where(r =>
                (string.IsNullOrEmpty(reservationCode) || r.ReservationCode == reservationCode) &&
                (string.IsNullOrEmpty(airline) || r.FlightCode == GetFlightByCode(r.FlightCode)?.Airline) &&
                (string.IsNullOrEmpty(name) || r.Name.Contains(name)));
            return filteredReservations.ToList();
        }

        // Helper method to retrieve Flight object based on FlightCode from Reservation
        private Flight GetFlightByCode(string flightCode)
        {
            // Need to impliment Flight data, well return null for now
            return null;
        }
    }
}
