using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace matchGame
{
    [Serializable]
    internal class Card
    {
        public ImageSource Image { get; set; }
        public bool IsFaceUp { get; set; }

        public Button button;
        public string Name { get; set; }

        public Card(ImageSource image, Button button)
        {
            Image = image;
            Name = image.ToString();
            IsFaceUp = false;
            this.button = button;
            button.Background = new SolidColorBrush(Colors.Gray);
        }

        public void Flip()
        {
            if (IsFaceUp)
            {
                button.Content = null;
                button.Background = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                button.Content = new Image() { Source = Image };
                button.Background = new SolidColorBrush(Colors.White);
            }
            IsFaceUp = !IsFaceUp;
        }
    }
}
