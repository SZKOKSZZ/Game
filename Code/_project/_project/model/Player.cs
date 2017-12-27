using _project.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _project.model
{
    public class Player
    {
        public SolidColorBrush UserColor;
        int Money;

        public Player(SolidColorBrush color)
        {
            UserColor = color;
            Money = 0;
            Strategy.Players.Add(this);
        }

        public int getMoney()
        {
            return Money;
        }
        public void setMoney(int money)
        {
            Money = money;
            Strategy.StatusBar.Text = Money.ToString() + "$";
        }
    }
}
