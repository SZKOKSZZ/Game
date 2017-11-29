/*
 * Created by SharpDevelop.
 * User: szzua
 * Date: 2017. 11. 29.
 * Time: 15:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Input;
using gameproject.Models;

namespace gameproject.Controls
{
	/// <summary>
	/// Description of BoardControl.
	/// </summary>
	public class BoardControl
	{
		Board board;
		public BoardControl()
		{
			board=new Board(viewer,30,140,140);
		}
		
		public void fillGrid()
        {
            BorderGrid g = new BorderGrid();
            g.IsHitTestVisible = true;
            for (int i = 0; i < Width; i++)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = new GridLength(GridSize, GridUnitType.Pixel);
                g.ColumnDefinitions.Add(c);
                for (int j = 0; j < Height; j++)
                {
                    if (i == 0)
                    {
                        RowDefinition r = new RowDefinition();
                        r.Height = new GridLength(GridSize, GridUnitType.Pixel);
                        g.RowDefinitions.Add(r);

                    }
                }
            }
            Field.Content = g;
        }
		public void zoomHandler(object sender, MouseWheelEventArgs e)
        {
            double z = GridSize;
            double ox = Field.HorizontalOffset + (Global.Window.ActualWidth/2);
            double oy = Field.VerticalOffset + (Global.Window.ActualHeight/2);
            
            e.Handled = true;

            
            GridSize += (byte)(Math.Sign(e.Delta) * 10);
            GridSize = (byte)(Math.Min(Math.Max((int)GridSize, 10), 80));
            foreach (var item in (Field.Content as Grid).ColumnDefinitions)
            {
                item.Width = new GridLength(GridSize, GridUnitType.Pixel);
            }
            foreach (var item in (Field.Content as Grid).RowDefinitions)
            {
                item.Height = new GridLength(GridSize, GridUnitType.Pixel);
            }

            z = GridSize / z;
            Field.ScrollToHorizontalOffset(ox * z - (Global.Window.ActualWidth / 2));
            Field.ScrollToVerticalOffset(oy * z - (Global.Window.ActualHeight / 2));
        }
		
	}
}
