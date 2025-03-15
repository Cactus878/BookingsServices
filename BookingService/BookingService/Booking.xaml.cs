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
        private string DateAndTime;
        private string Seat;

        private string UserLoggedInEmail;

        public Booking(string UserLoggedInEmail)
        {
            this.UserLoggedInEmail = UserLoggedInEmail;

            InitializeComponent();

            sqliteAccess = new SQLiteAccess(@"C:\Users\yanni\OneDrive\Documents\Projects\BookingsServices\BookingService\BookingsDB.db");

            UserEmail.Content = UserLoggedInEmail + "\r\n";

            LoadMovies();
        }

        //Movie Selection
        private void LoadMovies()
        {
            string query = "SELECT DISTINCT MovieName, MovieImage FROM Movie;";
            List<Movie> moviesData = sqliteAccess.ExecuteMovieQuery(query);

            /*Debug
            foreach (Movie movie in moviesData)
            {
                Console.WriteLine($"Movie Name: {movie.MovieName}, Movie Image: {movie.ImagePath}");
            }*/
            MovieOptions.ItemsSource = moviesData;
        }

        private void MovieListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovieOptions.SelectedItem != null)
            {
                // Get the selected movie
                Movie selectedMovie = (Movie)MovieOptions.SelectedItem;

                MovieName = selectedMovie.MovieName;
                SeatOptions.ItemsSource = null;
                DateAndTimesOptions.ItemsSource = null;
                LoadDateAndTimesForMovie();
            }
        }

        //Date And Time Selection
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

        private void DateAndTimeChange(object sender, SelectionChangedEventArgs e)
        {
            if (DateAndTimesOptions.SelectedItem != null)
            {
                DateAndTime = DateAndTimesOptions.SelectedItem.ToString();
                SeatOptions.ItemsSource = null;
                LoadSeatsForMovie();
            }
        }

        //Seat Selection
        private void LoadSeatsForMovie()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@MovieName", MovieName },
                { "@TimeAndDate", DateAndTime }
            };

            string query = "SELECT * FROM Seats WHERE BookingEmail IS NULL AND BookedMovieName = @MovieName AND BookedTime = @TimeAndDate;"; // Query to get all movies
            List<string> seatData = sqliteAccess.ExecuteQuery(query, parameters);

            SeatOptions.ItemsSource = seatData;
        }

        private void SeatChange(object sender, SelectionChangedEventArgs e)
        {
            if(SeatOptions.SelectedItem != null)
            {
                Seat = SeatOptions.SelectedItem.ToString();
            }
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            if (SeatOptions.SelectedItem != null && MovieOptions != null && DateAndTimesOptions != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@Email", UserLoggedInEmail },
                    { "@Seat", Seat },
                    { "@MovieName", MovieName },
                    { "@TimeAndDate", DateAndTime }
                };


                MessageBox.Show("Email: " + UserLoggedInEmail + " Movie: " + MovieName + ", Date and Time: " + DateAndTime + ", Seat: " + Seat);

                string query = "UPDATE Seats SET BookingEmail = @Email WHERE SeatNumber = @Seat AND BookedMovieName = @MovieName AND BookedTime = @TimeAndDate;";
                sqliteAccess.ExecuteNonQuery(query, parameters);

                SeatOptions.SelectedItem = null;
                Seat = null;

                DateAndTimesOptions.SelectedItem = null;
                DateAndTime = null;

                MovieName = null;
            }
            else
            {
                MessageBox.Show("Please select all options");
            }
        }
    }

    public class Movie
    {
        public string MovieName { get; set; }
        public string MovieImage { get; set; }

        // Assuming MovieImage holds the path to the image
        public string ImagePath => !string.IsNullOrEmpty(MovieImage) ? MovieImage : "Images/default-image.jpg";

        public Movie(string title, string movieImage)
        {
            MovieName = title;
            MovieImage = movieImage;
        }
    }
}
