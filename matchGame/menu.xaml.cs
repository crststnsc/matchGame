using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
    /// Interaction logic for menu.xaml
    /// </summary>
    public partial class menu : Window
    {
        public menu()
        {
            InitializeComponent();

            BitmapImage bitmapImage = new BitmapImage(new Uri(UserData.ProfilePicture));
            profileImage.Source = bitmapImage;
        }

        private int numberOfPairs;

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            MatchingGameWindow matchingGameWindow = new MatchingGameWindow(numberOfPairs);
            matchingGameWindow.Show();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            numberOfPairs = (int)slider.Value;
        }

        private void continueGameButton_Click(object sender, RoutedEventArgs e)
        {
            FileStream fileStream = new FileStream("gameState.dat", FileMode.Open);

            BinaryFormatter formatter = new BinaryFormatter();

            GameState gameState = (GameState)formatter.Deserialize(fileStream);

            fileStream.Close();

            //if the username in the gamestate is the same as the username in the current gamestate
            if (gameState.Username == UserData.Username)
            {
                //start the game
                Hide();
                MatchingGameWindow matchingGameWindow = new MatchingGameWindow(gameState);
                matchingGameWindow.Show();
            }
            else
            {
                MessageBox.Show("You can only continue a game that you started.");
            }
            
        }
        
        private void statisticsButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Statistics statistics= new Statistics();
            statistics.Show();
        }
    }
}
