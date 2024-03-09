using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace FlightRMSGroup4
{
    public partial class MainPage : ContentPage
    {
        bool flightSelected = false;
        int currFlightReservationsLeft = 0;

        ObservableCollection<Airport> airports = new ObservableCollection<Airport>(BackendInfo.Airports);
        ObservableCollection<string> weekDays = new ObservableCollection<string> { "Any", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        ObservableCollection<Flight> queriedFlights = new ObservableCollection<Flight>();
        public MainPage()
        {
            InitializeComponent();
            
            airports.Insert(0, new Airport("ZZZ", "Any"));

            airportOrigin_pck.ItemsSource = airports;
            airportOrigin_pck.ItemDisplayBinding = new Binding("Name");
            airportOrigin_pck.SelectedIndex = 0;

            airportDestination_pck.ItemsSource = airports;
            airportDestination_pck.ItemDisplayBinding = new Binding("Name");
            airportDestination_pck.SelectedIndex = 0;

            weekDay_pck.ItemsSource = weekDays;
            weekDay_pck.SelectedIndex = 0;

            queriedFlights_pck.ItemsSource = queriedFlights;
            queriedFlights_pck.ItemDisplayBinding = new Binding("Label");
        }

        private void ChangeFlightInfo(
            bool flightSelectedPrm = false, string flightCodePrm = "-", string flightAirlinePrm = "-"
            , string flightDayPrm = "-", string flightTimePrm = "-", string flightCostPrm = "-"
            , string reserveForPrm = "-"
        )
        {
            flightSelected = flightSelectedPrm;
            flightCode_lbl.Text = flightCodePrm;
            flightAirline_lbl.Text = flightAirlinePrm;
            flightDay_lbl.Text = flightDayPrm;
            flightTime_lbl.Text = flightTimePrm;
            flightCost_lbl.Text = flightCostPrm;
            reserveFor_lbl.Text = $"Reserve For ({reserveForPrm} Reservations Available):";
            name_etr.Text = "";
            citizenship_etr.Text = "";
            name_etr.IsEnabled = flightSelectedPrm;
            citizenship_etr.IsEnabled = flightSelectedPrm;
            reserve_btn.BackgroundColor = Color.FromArgb("#b5b5b5");
        }

        private void OnSelectedFlight(object sender, EventArgs e)
        {
            Flight selectedFlight = ((Picker)sender).SelectedItem as Flight;
            if(selectedFlight is null) { return; }
            currFlightReservationsLeft = selectedFlight.ReservationsLeft;
            ChangeFlightInfo(true, selectedFlight.Code, selectedFlight.Airline, selectedFlight.WeekDay, selectedFlight.DepartureTime, selectedFlight.Cost.ToString(), selectedFlight.ReservationsLeft.ToString());
        }

        private void OnClick_QueryFlights(object sender, EventArgs e)
        {
            if (queriedFlights_pck.ItemsSource != null) { queriedFlights_pck.ItemsSource.Clear(); }

            string airportOrigin = ((Airport) airportOrigin_pck.SelectedItem).Code;
            string airportDestiny = ((Airport) airportDestination_pck.SelectedItem).Code;
            string weekDay = (string) weekDay_pck.SelectedItem;

            queriedFlights_pck.ItemsSource = new ObservableCollection<Flight>(BackendInfo.QueryFlights(airportOrigin, airportDestiny, weekDay));
            ChangeFlightInfo(flightSelectedPrm: false);
            currFlightReservationsLeft = 0;
            DisplayAlert("", "List updated", "Ok");
        }

        private void OnClick_FlightMenu(object sender, EventArgs e)
        {
            if(flightNReservation_grd.IsVisible == true && Reservation_grd.IsVisible == false)
            {
                Reservation_grd.IsVisible = true;
                flightMenu_btn.BackgroundColor = Color.FromArgb("#28004F");
                reservationMenu_btn.BackgroundColor = Color.FromArgb("#28004F");
                flightMenu_btn.TextColor = Color.FromArgb("#E9C84F");
                reservationMenu_btn.TextColor = Color.FromArgb("#E9C84F");
                return;
            }

            flightNReservation_grd.IsVisible = true;
            Reservation_grd.IsVisible = false;
            flightMenu_btn.BackgroundColor = Color.FromArgb("#6993ff");
            reservationMenu_btn.BackgroundColor = Color.FromArgb("#b5b5b5");
            if (flightMenu_btn.TextColor != Color.FromArgb("#000")) { flightMenu_btn.TextColor = Color.FromArgb("#000"); }
            if (reservationMenu_btn.TextColor != Color.FromArgb("#000")) { reservationMenu_btn.TextColor = Color.FromArgb("#000"); }
        }

        private void OnClick_ReservationMenu(object sender, EventArgs e)
        {
            if (Reservation_grd.IsVisible == true && flightNReservation_grd.IsVisible == false)
            {
                flightNReservation_grd.IsVisible = true;
                flightMenu_btn.BackgroundColor = Color.FromArgb("#28004F");
                reservationMenu_btn.BackgroundColor = Color.FromArgb("#28004F");
                flightMenu_btn.TextColor = Color.FromArgb("#E9C84F");
                reservationMenu_btn.TextColor = Color.FromArgb("#E9C84F");
                return;
            }

            flightNReservation_grd.IsVisible = false;
            Reservation_grd.IsVisible = true;
            flightMenu_btn.BackgroundColor = Color.FromArgb("#b5b5b5");
            reservationMenu_btn.BackgroundColor = Color.FromArgb("#6993ff");
            if (flightMenu_btn.TextColor != Color.FromArgb("#000")) { flightMenu_btn.TextColor = Color.FromArgb("#000"); }
            if (reservationMenu_btn.TextColor != Color.FromArgb("#000")) { reservationMenu_btn.TextColor = Color.FromArgb("#000"); }
        }

        private void OnClick_Reserve(object sender, EventArgs e)
        {
            if (flightSelected == false)
            {
                DisplayAlert("Alert", "Must select a flight before attempting to make a reservation.", "Ok");
                return;
            }
            else if (currFlightReservationsLeft <= 0)
            {
                DisplayAlert("Alert", "Can not reserve. Flight does not have any reservations available.", "Ok");
                return;
            }
            else if (String.IsNullOrEmpty(name_etr.Text.Trim()) || String.IsNullOrEmpty(citizenship_etr.Text.Trim()) || citizenship_etr.Text.Length < 3)
            {
                DisplayAlert("Alert", "Must fill 'Name' and 'Citizenship' fields in order to attempt reservation.", "Ok");
                return;
            }

            DisplayAlert("", "Successful reservation.", "Ok");
            string reservationCode = GenerateReservationCode();
            reservation_code.Text = reservationCode;
        }

        private string GenerateReservationCode()
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

            string reservation_code = letter.ToString() + randomNumber;
            return reservation_code;
        }

        private void OnClick_FindReservation(object sender, EventArgs e)
        {
            DisplayAlert("Reservation Finder", "Found all matching reservations", "Ok");
        }

        private void OnReservationInfoTyped(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(name_etr.Text.Trim()) && !String.IsNullOrEmpty(citizenship_etr.Text.Trim()) && citizenship_etr.Text.Length >= 3)
            {
                if(reserve_btn.BackgroundColor != Color.FromArgb("#6993ff"))
                {
                    reserve_btn.BackgroundColor = Color.FromArgb("#6993ff");
                }
            }
            else if(reserve_btn.BackgroundColor != Color.FromArgb("#b5b5b5"))
            {
                reserve_btn.BackgroundColor = Color.FromArgb("#b5b5b5");
            }
        }
    }
}
