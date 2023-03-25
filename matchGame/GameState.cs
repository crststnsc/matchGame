using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace matchGame
{
    [Serializable]
    internal class GameState
    {
        public string Username { get; set; }
        public Card[,] Cards { get; set; }
        public int TimeLeft { get; set; }
        public GameState(string username, Card[,] cards, int timeleft)
        {
            Username = username;
            Cards = cards;
            TimeLeft = timeleft;
        }
    }
}
