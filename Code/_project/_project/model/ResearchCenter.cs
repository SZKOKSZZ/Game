using _project.game;
using _project.model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _project.model
{
    public class ResearchCenter : Building
    {
        public void SetBuilding(Building b, int id)
        {
            string projectpath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            b.Icon.OpacityMask = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(projectpath + @"\images\construction.png"))
            };
            b.Name = "ResearchCenter";
            b.buildArray = new int[3];
            b.cost = 1000;
            b.setSize(1, 1);
            for (int i = 0; i < 3; i++)
            {
                b.buildArray[i] = 3;
            }
            SetToolTipBuilding(b.Name, b.cost, b.maintenanceCost, b.amortization);
        }
        public ResearchCenter(Player player, int id) : base(player, id)
        {
            SetBuilding(this, id);
        }
    }
}
