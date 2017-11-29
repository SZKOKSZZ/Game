using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gameproject.Models
{
    public abstract class BoardPiece
    {
        protected int id;
        protected string name;
        protected int cost;
        protected int amortization;
        protected int maintenanceCost;
        protected BitmapImage ImageSource;
        protected TextBlock toolTip;
        protected Rectangle cell;

        public abstract void SetToolTip();

    }
}
