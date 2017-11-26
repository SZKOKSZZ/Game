using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace gameproject.Models
{
    public class User
    {
        public int Money { get; set; }
        public int Population { get; set; }
        public int Pollution { get; set; }
        public int Energy { get; set; }
        public int Science { get; set; }
        public int Production { get; set; }
        public int ArmyPower { get; set; }
        public SolidColorBrush UserColor { get; set; }
          

    }
}
