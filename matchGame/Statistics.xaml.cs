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

namespace matchGame
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        public Statistics()
        {
            InitializeComponent();
            profileName.Text = UserData.Username;
            BitmapImage bitmapImage = new BitmapImage(new Uri(UserData.ProfilePicture));
            profileImage.Source = bitmapImage;

            var users = new CsvHandler<UserStats>("user_statistics.csv").ReadData();
            var user = users.FirstOrDefault(x => x.Username == UserData.Username);
            if (user != null)
            {
                gamesPlayedTxt.Text = user.GamesPlayed.ToString();
                gamesWonTxt.Text = user.GamesWon.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            menu menu = new menu();
            menu.Show();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            //delete the user from the user data and user statistics
            var users = new CsvHandler<UserRegistrationData>("user_data.csv").ReadData();
            var user = users.FirstOrDefault(x => x.Username == UserData.Username);
            if (user != null)
            {
                users.Remove(user);
                new CsvHandler<UserRegistrationData>("user_data.csv").WriteData(users);
            }
            
            var usersStats = new CsvHandler<UserStats>("user_statistics.csv").ReadData();
            var userStats = usersStats.FirstOrDefault(x => x.Username == UserData.Username);
            if (userStats != null)
            {
                usersStats.Remove(userStats);
                new CsvHandler<UserStats>("user_statistics.csv").WriteData(usersStats);
            }

            //go back to the login screen
            Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
