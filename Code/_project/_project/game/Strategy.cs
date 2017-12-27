using _project.DataBase;
using _project.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _project.game
{
    public class Strategy
    {
        public static Board Board;
        public static Database Database = new Database();
        public static List<Player> Players = new List<Player>();
        public static Grid Grid;
        public static TextBlock StatusBar;
        public static Window Window;
    }
}
