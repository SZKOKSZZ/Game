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
    public class Building:BoardPiece
    {
        public int[] BuildArray { get; set; }

        public override void SetToolTip()
        {
            throw new NotImplementedException();
        }
    }
}
