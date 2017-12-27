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
            u = new Worker(human, 1, 10);
            u.setPosition(5, 2);
            u = new Unit(human, 0, 10);
            u.setPosition(4, 4);
            u = new Unit(cpu, 0, 12);
            u.setPosition(8, 4);

            CenterBuilding cen= new CenterBuilding(human, 0);
            cen.setPosition(1, 2);
            ConstructionPlant con= new ConstructionPlant(human, 0);
            con.setPosition(1, 3);
            CommercialArea com= new CommercialArea(human, 0);
            com.setPosition(1, 4);
            Factory fac= new Factory(human, 0);
            com.setPosition(1, 5);
            HousingEstate hou= new HousingEstate(human, 0);
            hou.setPosition(1, 6);
            MilitaryBase mil= new MilitaryBase(human, 0);
            mil.setPosition(1, 7);

            ProcessingPlant pro = new ProcessingPlant(human,0);
            pro.setPosition(1, 9);

            RecyclingCenter rec = new RecyclingCenter(human, 0);
            rec.setPosition(2, 8);

            ResearchCenter res = new ResearchCenter(human,0);
            res.setPosition(5, 6);

            ScienceCenter sci = new ScienceCenter(human, 0);
            sci.setPosition(8, 8);
        }

        private void btn_nextRound_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Strategy.Board.Instances)
            {
                if (item is Unit) (item as Unit).Step = (item as Unit).StepMax;
                item.SetToolTip();
            }
            MessageBox.Show("All steps has been reset.");
        }
    }
}