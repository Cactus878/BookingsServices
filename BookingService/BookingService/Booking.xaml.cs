using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace BookingService
{
    /// <summary>
    /// Interaction logic for Booking.xaml
    /// </summary>
    public partial class Booking : Window
    {
        private SQLiteAccess sqliteAccess;

        private string MovieName;
        private string TimeAndDate;

        private string UserLoggedInEmail;

        public Booking(string UserLoggedInEmail)
        {
            this.UserLoggedInEmail = UserLoggedInEmail;

            InitializeComponent();

            sqliteAccess = new SQLiteAccess(@"C:\Users\yanni\OneDrive\Documents\Projects\BookingsServices\BookingService\BookingsDB.db");

            UserEmail.Content = UserLoggedInEmail + "\r\n";

            LoadMovies();
        }

        private void LoadMovies()
        {
            string query = "SELECT DISTINCT MovieName FROM Movie;";  // Query to get all movies
            List<string> moviesData = sqliteAccess.ExecuteQuery(query);

            // Bind the data to the 
            MovieOptions.ItemsSource = moviesData;
        }

        private void LoadDateAndTimesForMovie()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@MovieName", MovieName }
            };

            string query = "SELECT DISTINCT MovieTime FROM Movie WHERE MovieName = @MovieName;";  // Query to get all movies
            List<string> dateAndTimeData = sqliteAccess.ExecuteQuery(query, parameters);

            DateAndTimesOptions.ItemsSource = dateAndTimeData;
        }

        private void LoadSeatsForMovie()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@MovieName", MovieName },
                { "@TimeAndDate", TimeAndDate }
            };

            string query = "SELECT * FROM Seats WHERE BookingEmail IS NULL AND BookedMovieName = @MovieName AND BookedTime = @TimeAndDate;"; // Query to get all movies
            List<string> seatData = sqliteAccess.ExecuteQuery(query, parameters);

            SeatOptions.ItemsSource = seatData;
        }

        private void MovieChange(object sender, SelectionChangedEventArgs e)
        {
            MovieName = MovieOptions.SelectedItem.ToString();
            LoadDateAndTimesForMovie();
        }

        private void DateAndTimeChange(object sender, SelectionChangedEventArgs e)
        {
            TimeAndDate = DateAndTimesOptions.SelectedItem.ToString();
            LoadSeatsForMovie();
        }

        private void SeatChange(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
