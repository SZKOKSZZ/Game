using _project.game;
using _project.model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            globalizing();
            initalizing();
        }
        /*private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            int xval = int.Parse((sender as Button).Content.ToString().Split(',')[0]);
            int yval = int.Parse((sender as Button).Content.ToString().Split(',')[1]);
            field.ScrollToVerticalOffset(field.VerticalOffset + yval * 5);
            field.ScrollToHorizontalOffset(field.HorizontalOffset + xval * 5);
        }*/
        private void globalizing()
        {
            Strategy.Grid = grd_Main;
            Strategy.Board = new Board(field, 140, 140);
            Strategy.StatusBar = txt_stats;
            Strategy.Window = this;
        }

        private void initalizing()
        {
            Player human = new Player(Brushes.LightSkyBlue); human.setMoney(1000);
            Player cpu = new Player(Brushes.GreenYellow);

            Unit u;
            u = new Worker(human, 1, 10); u.setPosition(5, 2);
            u = new Unit(human, 0, 10); u.setPosition(4, 4);
            u = new Unit(cpu, 0, 12); u.setPosition(8, 4);

            Building b;
            b = new Building(human, 0); b.setPosition(7, 2);
        }

        private void btn_nextRound_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Strategy.Board.Instances)
            {
                if (item is Unit) (item as Unit).Step = (item as Unit).StepMax;
                item.setToolTip();
            }
            MessageBox.Show("All steps has been reset.");
        }
    }
}