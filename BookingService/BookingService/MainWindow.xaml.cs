using System;
using System.Collections.Generic;
using System.Data;
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
            string query = "SELECT * FROM Movie";  // Query to get all movies
            DataTable moviesData = sqliteAccess.ExecuteQuery(query);

            // Bind the data to the DataGrid
            MoviesDataGrid.ItemsSource = moviesData.DefaultView;
        }
    }
}
