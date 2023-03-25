using System;
using System.Collections.Generic;
using System.IO;
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

namespace matchGame
{
    /// <summary>
    /// Interaction logic for registerWindow.xaml
    /// </summary>
    public partial class registerWindow : Window
    {

        private ProfileImageHandler profileImageHandler;
        public registerWindow()
        {
            InitializeComponent();
            profileImageHandler = new ProfileImageHandler();
            profileImage.Source = profileImageHandler.imgProfilePicture;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Check if the username already exists
            bool usernameExists = false;
            using (StreamReader sr = File.OpenText("user_data.csv"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    if (values[0] == txtUsername.Text)
                    {
                        usernameExists = true;
                        break;
                    }
                }
            }

            // Show prompt if the username already exists
            if (usernameExists)
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
            }
            else
            {   
                if(txtPassword.Password != txtConfirm.Password)
                {
                    MessageBox.Show("Passwords do not match. Please try again.");
                    return;
                }


                // Create new user registration data and append to CSV file
                var userData = new UserRegistrationData
                {
                    Username = txtUsername.Text,
                    Password = txtPassword.Password,
                    ProfilePicture = profileImageHandler.imgProfilePicture.UriSource.ToString()
                };
                using (StreamWriter sw = File.AppendText("user_data.csv"))
                {
                    sw.WriteLine(userData.Username + "," + userData.Password + "," + userData.ProfilePicture);
                }

                using (StreamWriter sw = File.AppendText("user_statistics.csv"))
                {
                    sw.WriteLine(userData.Username + "," + 0 + "," + 0);
                }

                MessageBox.Show("Registration successful!");
                Hide();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            profileImageHandler.NextProfilePicture();
            profileImage.Source = profileImageHandler.imgProfilePicture;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            profileImageHandler.PreviousProfilePicture();
            profileImage.Source = profileImageHandler.imgProfilePicture;
        }
    }
}
