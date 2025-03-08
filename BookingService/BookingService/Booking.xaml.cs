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
            string query = "SELECT DISTINCT MovieName, MovieImage FROM Movie;";  // Query to get all movies
            List<Movie> moviesData = sqliteAccess.ExecuteMovieQuery(query);

            //Debug
            foreach (Movie movie in moviesData)
            {
                Console.WriteLine($"Movie Name: {movie.MovieName}, Movie Image: {movie.ImagePath}");
            }

            // Bind the data to the 
            MovieListBox.ItemsSource = moviesData;
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
        }

        private void DateAndTimeChange(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchForSeats(object sender, RoutedEventArgs e)
        {
            if (DateAndTimesOptions.SelectedItem != null)
            {
                TimeAndDate = DateAndTimesOptions.SelectedItem.ToString();
                LoadSeatsForMovie();
            }
            else
            {
                MessageBox.Show("Please select a date and time.");
            }
        }

        private void MovieListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovieListBox.SelectedItem != null)
            {
                // Get the selected movie
                Movie selectedMovie = (Movie)MovieListBox.SelectedItem;

                MovieName = selectedMovie.MovieName;
                LoadDateAndTimesForMovie();
            }
            else
            {
                MessageBox.Show("Please select a movie.");
            }
        }
    }

    public class Movie
    {
        public string MovieName { get; set; }
        public string MovieImage { get; set; }

        // Assuming MovieImage holds the path to the image
        public string ImagePath => !string.IsNullOrEmpty(MovieImage) ? MovieImage : "default-image.jpg";

        public Movie(string title, string movieImage)
        {
            MovieName = title;
            MovieImage = movieImage;
        }
    }
}
