using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteAccess sqliteAccess;
        private string MovieName;
        private string TimeAndDate;

        public MainWindow()
        {
            InitializeComponent();
            // Initialize the SQLiteHelper with your database file path
            sqliteAccess = new SQLiteAccess(@"C:\Users\yanni\OneDrive\Documents\Projects\BookingsServices\BookingService\BookingsDB.db");
            LoadMovies();
        }

        // Method to load movies from the database into the DataGrid
        private void LoadMovies()
        {
            string query = "SELECT DISTINCT MovieName FROM Movie;";  // Query to get all movies
            List<string> moviesData = sqliteAccess.ExecuteQuery(query);

            // Bind the data to the DataGrid
            //MoviesDataGrid.ItemsSource = moviesData.DefaultView;
            listBox1.ItemsSource = moviesData;
        }

        private void LoadDateAndTimesForMovie()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@MovieName", MovieName }
            };

            string query = "SELECT DISTINCT MovieTime FROM Movie WHERE MovieName = @MovieName;";  // Query to get all movies
            //DataTable moviesData = sqliteAccess.ExecuteQuery(query, parameters);

            // Bind the data to the DataGrid
            //MoviesDataGrid.ItemsSource = moviesData.DefaultView;
        }

        private void LoadSeatsForMovie()
        {
            var parameters = new Dictionary<string, object>
            {
                { "@MovieName", MovieName },
                { "@MovieName", TimeAndDate }
            };

            string query = "SELECT * FROM Seats WHERE BookingId IS NULL AND BookedMovieName = @MovieName AND BookedTime = @TimeAndDate;"; // Query to get all movies
            //DataTable moviesData = sqliteAccess.ExecuteQuery(query, parameters);

            // Bind the data to the DataGrid
            //MoviesDataGrid.ItemsSource = moviesData.DefaultView;
        }
    }
}
