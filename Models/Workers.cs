using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace gameproject.Models
{
    class Workers:Unit
    {

        public string WorkName { get; set; }

        public override void SetToolTip()
        {
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
            //if (Owner == Global.Players[0])
            //{
            //    textBlock.Inlines.Add(new Run("\n" + "👣 " + Step + "/" + StepMax));
            //}
            //else { }

            textBlock.Inlines.Add(new Run("\n" + "👣" + StepMax));
            textBlock.Inlines.Add(new Run("\n" + "🛡" + Health));
            textBlock.Inlines.Add(new Run("\n" + "🛠" + WorkName));

           



            if (Owner == null)
                textBlock.Inlines.Add(new Run("\n" + "Cost: " + cost + "$"));
            toolTip = textBlock;
            Cell.ToolTip = toolTip;
        }

    }
}
