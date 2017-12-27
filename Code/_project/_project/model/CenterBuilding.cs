using _project.game;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _project.model
{
    public class CenterBuilding : Building
    {
        public string bName { get; set; }
        public void SetBuilding(Building b, int id)
        {
            string projectpath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            b.Icon.OpacityMask = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(projectpath+@"\images\center.png"))
            };
            b.Name = "CenterBuilding";
            b.buildArray = new int[3];
            b.cost = 1000;
            b.maintenanceCost = 10;
            b.amortization = 50;
            b.setSize(1, 1);
            for (int i = 0; i < 3; i++)
            {
                b.buildArray[i] = 3;
            }
            SetToolTipBuilding(b.Name,b.cost,b.maintenanceCost,b.amortization);
        }
        public CenterBuilding(Player player, int id) : base(player, id)
        {
            SetBuilding(this, id);
            
        }
    }
}