using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gameproject.Models
{
    public class Unit : BoardPiece
    {
        public int Step { get; set; }
        public int StepMax { get; set; }
        public int Count { get; set; }
        public int Health { get; set; }
        public User Owner { get; set; }

        public Unit()
        {
            cell = new Rectangle();
        }

        public override void SetToolTip(){}

    }
}
