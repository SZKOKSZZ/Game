using _project.game;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _project.model
{
    public class CenterBuilding : Building
    {
      
        public CenterBuilding(Player player, int id) : base(player, id)
        {

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