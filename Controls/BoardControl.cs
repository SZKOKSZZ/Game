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
using System.Windows.Controls;
using System.Windows;

namespace gameproject.Controls
{
	/// <summary>
	/// Description of BoardControl.
	/// </summary>
	public class BoardControl
	{
		public Board board;
        public Window window;
		public BoardControl(ScrollViewer field)
		{
            window = new Window();
			board=new Board(field, 30,140,140);
            FillGrid();
		}
		
		public void FillGrid()
        {
            BorderGrid g = new BorderGrid();
            g.IsHitTestVisible = true;
            for (int i = 0; i < board.Width; i++)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = new GridLength(board.GridSize, GridUnitType.Pixel);
                g.ColumnDefinitions.Add(c);
                for (int j = 0; j < board.Height; j++)
                {
                    if (i == 0)
                    {
                        RowDefinition r = new RowDefinition();
                        r.Height = new GridLength(board.GridSize, GridUnitType.Pixel);
                        g.RowDefinitions.Add(r);

                    }
                }
            }
            board.Viewer.Content = g;
        }
		public void ZoomHandler(object sender, MouseWheelEventArgs e)
        {
            double z = board.GridSize;
            double ox = board.Viewer.HorizontalOffset + (window.ActualWidth/2);
            double oy = board.Viewer.VerticalOffset + (window.ActualHeight/2);
            
            e.Handled = true;


            board.GridSize += (byte)(Math.Sign(e.Delta) * 10);
            board.GridSize = (byte)(Math.Min(Math.Max((int)board.GridSize, 10), 80));
            foreach (var item in (board.Viewer.Content as Grid).ColumnDefinitions)
            {
                item.Width = new GridLength(board.GridSize, GridUnitType.Pixel);
            }
            foreach (var item in (board.Viewer.Content as Grid).RowDefinitions)
            {
                item.Height = new GridLength(board.GridSize, GridUnitType.Pixel);
            }

            z = board.GridSize / z;
            board.Viewer.ScrollToHorizontalOffset(ox * z - (window.ActualWidth / 2));
            board.Viewer.ScrollToVerticalOffset(oy * z - (window.ActualHeight / 2));
        }
		
	}
}
