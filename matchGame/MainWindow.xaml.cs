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

namespace matchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }    

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var users = new CsvHandler<UserRegistrationData>("user_data.csv").ReadData();
            var user = users.FirstOrDefault(x => x.Username == txtUsername.Text && x.Password == txtPassword.Password);
            if (user != null)
            {
                UserData.Username = txtUsername.Text;
                UserData.Password = txtPassword.Password;
                UserData.ProfilePicture = user.ProfilePicture;
                menu menu = new menu();
                Hide();
                menu.Show();
            }
            else
            {
                MessageBox.Show("Username or password is incorrect. Please try again.");
            }

            //MatchingGameWindow matchingGameWindow = new MatchingGameWindow();
            //Hide();
            //matchingGameWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //show the registerWindow
            registerWindow register = new registerWindow();
            register.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
