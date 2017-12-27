using _project.game;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace gameproject.Models
{
    public class MilitaryBase:Building
    {
        public class CenterBuilding:Building
    {
        public override void SetToolTip()
        {
            public string BuildingName { get; set; }
            
            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Clear();
            
            if (Owner == null)
            {
                textBlock.Inlines.Add(new Run(name)
                {
                    Foreground = Brushes.White
                });
            }
            
            else
            {
                textBlock.Inlines.Add(new Run(name)
                {
                    Foreground = Owner.UserColor
                });
                textBlock.Inlines.Add(new Run(" x" + Count));
            }
            
            textBlock.Inlines.Add(new Run("\n" + "🛡" + amortization));
            textBlock.Inlines.Add(new Run("\n" + "🛡" + maintenanceCost));
            textBlock.Inlines.Add(new Run("\n" + "🛠" + BuildingName));

            if (Owner == null)
                textBlock.Inlines.Add(new Run("\n" + "Cost: " + cost + "$"));
            toolTip = textBlock;
            Cell.ToolTip = toolTip;
        }
    }
    }
}
