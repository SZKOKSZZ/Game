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
    abstract class Unit
    {
        protected int id;
        protected string name;
        protected int cost;
        protected int Step;
        protected int StepMax;
        protected int Count;
        protected int Health;  
        protected int amortization;
        protected int maintenanceCost;
        protected int[] buildArray;
        protected BitmapImage ImageSource;
        protected Rectangle cell;
        protected TextBlock toolTip;
            
    }
}
