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

namespace BookingService
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {

        public Menu(string UserLoggedInEmail)
        {
            this.UserLoggedInEmail = UserLoggedInEmail;

            InitializeComponent();
        }

        public string UserLoggedInEmail { get; }

        private void Book_Button_Click(object sender, RoutedEventArgs e)
        {
            Booking booking = new Booking(UserLoggedInEmail);
            booking.ShowDialog();
        }

        private void History_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
