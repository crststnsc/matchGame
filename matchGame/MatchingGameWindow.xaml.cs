using CsvHelper.Configuration.Attributes;
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
using System.Windows.Threading;

namespace matchGame
{
    /// <summary>
    /// Interaction logic for MatchingGameWindow.xaml
    /// </summary>
    public partial class MatchingGameWindow : Window
    {
        private Card[,] cards;
        private DispatcherTimer timer;
        private int timeLeft; // total time in seconds
        private GameState gameState;

        public int TimeLeft
        {
            get { return timeLeft; }
            set
            {
                timeLeft = value;
                // update the timer display
                timerTextBlock.Text = $"{timeLeft / 60:00}:{timeLeft % 60:00}";
                // end the game if time runs out
                if (timeLeft <= 0)
                {
                    EndGame(" You Lost!");
                }
            }
        }

        public MatchingGameWindow(int gameSize)
        {
            InitializeComponent();
            Closing += Window_Closing;
            var userStatistics = new CsvHandler<UserStats>("user_statistics.csv").ReadData();

            //add a game played in csv to the correct user
            foreach (var user in userStatistics)
            {
                if (user.Username == UserData.Username)
                {
                    user.GamesPlayed++;
                }
            }

            CsvHandler<UserStats> userCSV = new CsvHandler<UserStats>("user_statistics.csv");
            userCSV.WriteData(userStatistics);

            int numRows = gameSize;
            int numCols = gameSize;
            cards = new Card[numRows, numCols];

            List<ImageSource> images = new List<ImageSource>();

            var imagePaths = new ProfileImageHandler().GetImagePaths();
            foreach(var image in imagePaths)
            {
                images.Add(new BitmapImage(new Uri(image)));
            }

            Random rand = new Random();
            List<ImageSource> imageList = new List<ImageSource>();
            for (int i = 0; i < numRows * numCols / 2; i++)
            {
                ImageSource image = images[i];
                imageList.Add(image);
                imageList.Add(image.Clone());
            }
            imageList = imageList.OrderBy(x => rand.Next()).ToList();

            for (int i = 0; i < numRows; i++)
            {
                gameBoard.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                for (int j = 0; j < numCols; j++)
                {
                    gameBoard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                    Button button = new Button();
                    button.Height = 100;
                    button.Width = 100;
                    button.Click += Button_Click;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    gameBoard.Children.Add(button);
                    cards[i, j] = new Card(imageList[i * numCols + j], button);
                }
            }

            gameState = new GameState(UserData.Username, cards, 60);
            //gameBoard.HorizontalAlignment = HorizontalAlignment.Center;
            //gameBoard.VerticalAlignment = VerticalAlignment.Center;
            StartGame(60);
        }
        
        internal MatchingGameWindow(GameState g)
        {
            InitializeComponent();
            Closing += Window_Closing;

            int numRows = g.Cards.GetLength(0);
            int numCols = g.Cards.GetLength(1);

            for (int i = 0; i < numRows; i++)
            {
                gameBoard.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                for (int j = 0; j < numCols; j++)
                {
                    gameBoard.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
                    Button button = g.Cards[i, j].button;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    gameBoard.Children.Add(button);
                }
            }
            //gameBoard.HorizontalAlignment = HorizontalAlignment.Center;
            //gameBoard.VerticalAlignment = VerticalAlignment.Center;
            StartGame(g.TimeLeft);
        }

        private Card firstCard = null;
        private Card secondCard = null;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Card card = cards[Grid.GetRow(button), Grid.GetColumn(button)];

            if (card.IsFaceUp) // The card is already flipped, do nothing
            {
                return;
            }

            card.Flip();

            if (firstCard == null)
            {
                firstCard = card;
            }
            else if (secondCard == null && firstCard != null)
            {
                secondCard = card;

                if (firstCard.Name == secondCard.Name)
                {
                    firstCard = null;
                    secondCard = null;
                }
                else
                {
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += (s, args) =>
                    {
                        firstCard.Flip();
                        secondCard.Flip();
                        firstCard = null;
                        secondCard = null;
                        timer.Stop();
                    };
                    timer.Start();
                }
            }

            gameState.Cards = cards;

            if (GameWon())
            {
                var userStatistics = new CsvHandler<UserStats>("user_statistics.csv").ReadData();
                foreach(var user in userStatistics)
                {
                    if(user.Username == UserData.Username)
                    {
                        user.GamesWon++;
                    }
                }
                CsvHandler<UserStats> userCSV = new CsvHandler<UserStats>("user_statistics.csv");
                userCSV.WriteData(userStatistics);
                EndGame(" You Won!");
            }
        }

        private void StartGame(int time)
        {
            // ...
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            TimeLeft = time; // set initial time
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLeft--;
            gameState.TimeLeft = TimeLeft;
        }

        private void EndGame(params string[] message)
        {
            timer.Stop();
            MessageBox.Show("Game Over!" + message[0]);
            Hide();
            menu menuWindow = new menu();
            menuWindow.Show();
        }

        private bool GameWon()
        {
            foreach(var card in cards)
            {
                if (!card.IsFaceUp)
                {
                    return false;
                }
            }
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (!GameWon())
            //{
            //    FileStream fileStream = new FileStream("gameState.dat", FileMode.Create);
            //    BinaryFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(fileStream, new GameState(gameState.Username, gameState.Cards, gameState.TimeLeft));
            //    fileStream.Close();
            //}
        }
    }
}
