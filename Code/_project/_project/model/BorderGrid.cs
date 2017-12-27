using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _project.model
{
    public class BorderGrid : Grid
    {
        protected override void OnRender(DrawingContext dc)
        {
            double leftOffset = 0;
            double topOffset = 0;
            Pen pen = new Pen(Brushes.Gray, 1);
            pen.Freeze();

            foreach (RowDefinition row in this.RowDefinitions)
            {
                dc.DrawLine(pen, new Point(0, topOffset), new Point(this.ActualWidth, topOffset));
                topOffset += row.ActualHeight;
            }
            dc.DrawLine(pen, new Point(0, topOffset), new Point(this.ActualWidth, topOffset));

            foreach (ColumnDefinition column in this.ColumnDefinitions)
            {
                dc.DrawLine(pen, new Point(leftOffset, 0), new Point(leftOffset, this.ActualHeight));
                leftOffset += column.ActualWidth;
            }
            dc.DrawLine(pen, new Point(leftOffset, 0), new Point(leftOffset, this.ActualHeight));

            base.OnRender(dc);
        }
    }
}
