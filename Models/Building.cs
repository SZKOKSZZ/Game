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
        public override Rectangle Cell { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void SetPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void SetSize(int width, int height)
        {
            throw new NotImplementedException();
        }

        public override void SetToolTip()
        {
            throw new NotImplementedException();
        }
    }
}
