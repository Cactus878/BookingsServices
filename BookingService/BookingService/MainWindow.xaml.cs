using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
        public MainWindow()
        {
            InitializeComponent();
            // Initialize the SQLiteHelper with your database file path
            sqliteAccess = new SQLiteAccess(@"C:\Users\yanni\OneDrive\Documents\Projects\BookingsServices\BookingService\BookingsDB.db");
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Email", emailInputBox.Text },
                { "@Password", passwordInputBox.Text }
            };

            string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password;";
            List<string> usersFound = sqliteAccess.ExecuteQuery(query, parameters);

            if (usersFound.Count() > 0)
            {
                string UserLoggedInEmail = usersFound[0];
                Booking booking = new Booking(UserLoggedInEmail);
                booking.ShowDialog();
            }
            else
            {
                MessageBox.Show("Email or Password is incorrect");
            }
        }

        private void signInButton_Click(object sender, RoutedEventArgs e)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Email", emailInputBox.Text },
                { "@Password", passwordInputBox.Text }
            };

            string query = "SELECT * FROM Users WHERE Email = @Email";
            List<string> usersFound = sqliteAccess.ExecuteQuery(query, parameters);

            if (usersFound.Count() > 0)
            {
                MessageBox.Show("Email is already in use");
            }
            else
            {
                query = "INSERT INTO Users VALUES (@Email, @Password);";
                sqliteAccess.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Account has been created");
            }
        }
    }
}
