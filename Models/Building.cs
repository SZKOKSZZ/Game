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
    abstract class Building
    {
        protected int id;
        protected string name;      
        protected int cost;
        protected int amortization;
        protected int maintenanceCost;

        protected int[] buildArray;
        protected BitmapImage ImageSource;
        protected Rectangle cell;
        protected TextBlock toolTip;

        public abstract void AmortizationCounter();
        public abstract void MaintenanceCostCounter();
    }
}
