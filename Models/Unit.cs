using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public override Rectangle Cell { get ; set; }

        public bool drag = false;
        public Point dragPoint;

        public Unit()
        {
            Cell = new Rectangle();
        }

        public override void SetToolTip(){}
        public override void SetPosition(int x, int y)
        {
            Grid.SetColumn(Cell, x);
            Grid.SetRow(Cell, y);
        }
        public override void SetSize(int width, int height)
        {
            Grid.SetColumnSpan(Cell, width);
            Grid.SetRowSpan(Cell, height);
        }
    }
}
