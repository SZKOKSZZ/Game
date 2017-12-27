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
        public void SetBuilding(Building b, int id)
        {
            string projectpath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            b.Icon.OpacityMask = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(projectpath+@"\images\center.png"))
            };
            b.Name = "Center";
            b.buildArray = new int[3];
            b.cost = 1000;
            b.setSize(1, 1);
            for (int i = 0; i < 3; i++)
            {
                b.buildArray[i] = 3;
            }
        }


        public CenterBuilding(Player player, int id) : base(player, id)
        {
            SetBuilding(this, id);
        }

        public override void SetToolTip()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Clear();

            if (Owner == null)
            {
                textBlock.Inlines.Add(new Run(Name)
                {
                    Foreground = Brushes.White
                });
            }

            else
            {
                textBlock.Inlines.Add(new Run(Name)
                {
                    Foreground = Owner.UserColor
                });
                //textBlock.Inlines.Add(new Run(" x" + Count));
            }

            textBlock.Inlines.Add(new Run("\n" + "🛡" + amortization));
            textBlock.Inlines.Add(new Run("\n" + "🛡" + maintenanceCost));
            textBlock.Inlines.Add(new Run("\n" + "🛠" + Name));

            if (Owner == null)
                textBlock.Inlines.Add(new Run("\n" + "Cost: " + cost + "$"));
            toolTip = textBlock;
            Icon.ToolTip = toolTip;
        }
    }
}