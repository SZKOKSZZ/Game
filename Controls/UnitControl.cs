/*
 * Created by SharpDevelop.
 * User: szzua
 * Date: 2017. 11. 29.
 * Time: 15:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using gameproject.Models;

namespace gameproject.Controls
{
    public class UnitControl:BoardControl
	{
		Unit unit;
		public UnitControl(ScrollViewer field) : base(field)
        {
			unit = new Unit();
		}
		public void dragTriggerDown(object sender, MouseButtonEventArgs e)
		{
			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				unit.drag = true;
				unit.dragPoint = new Point(e.GetPosition(board.Viewer).X, e.GetPosition(board.Viewer).Y);
			}
		}
		public void dragTriggerUp(object sender, MouseButtonEventArgs e)
		{
			if (e.MiddleButton == MouseButtonState.Released)
			{
				unit.drag = false;
			}
		}
		public void dragHandler(object sender, MouseEventArgs e)
		{
			if (!unit.drag)
				return;
			ScrollViewer field = (board.Viewer.Parent as ScrollViewer);
			Point m = new Point(e.GetPosition(board.Viewer).X, e.GetPosition(board.Viewer).Y);
            board.Viewer.ScrollToVerticalOffset(board.Viewer.VerticalOffset + (unit.dragPoint.Y - m.Y));
            board.Viewer.ScrollToHorizontalOffset(board.Viewer.HorizontalOffset + (unit.dragPoint.X - m.X));
			unit.dragPoint = new Point(m.X, m.Y);
		}
		public void moveCancel(object sender, MouseButtonEventArgs e)
		{   
			//MessageBox.Show("SWAG-YOLO");
			if (board.Unitp == null)
				return;
            board.Unitp.Cell.Fill = Brushes.Aquamarine;
			movePathClear();
            board.Unitp = null;
		}
		public void movePathClear()
		{
			foreach (var item in board.CellsPath)
			{
				(board.Viewer.Content as Grid).Children.Remove(item);
			}
			board.CellsPath.Clear();
		}
		public void movePathDraw(object sender, MouseEventArgs e)
		{
			if (board.Unitp == null)
				return;

			int x = (int)(e.GetPosition(board.Viewer).X + board.Viewer.HorizontalOffset);
			int y = (int)(e.GetPosition(board.Viewer).Y + board.Viewer.VerticalOffset);
			x = (x / board.GridSize) - Grid.GetColumn(board.Unitp.Cell);
			y = (y / board.GridSize) - Grid.GetRow(board.Unitp.Cell);

			movePathClear();

			int xPos = Grid.GetColumn(board.Unitp.Cell);
			int yPos = Grid.GetRow(board.Unitp.Cell);
			int step = 0;
			Rectangle r;
			while (x != 0 || y != 0)
			{
				if (Math.Abs(x) > 0)
				{
					xPos += Math.Sign(x);
					x -= Math.Sign(x);
				}
				if (Math.Abs(y) > 0)
				{
					yPos += Math.Sign(y);
					y -= Math.Sign(y);
				}
				r = new Rectangle();
				r.Margin = new Thickness(1, 1, 1, 1);//(GridSize / 4, GridSize / 4, GridSize / 4, GridSize / 4);
				r.Opacity = 0.4;
				Grid.SetColumn(r, xPos);
				Grid.SetRow(r, yPos);
				r.RadiusX = step;
				//Panel.SetZIndex(r,-1);

				if (step < board.Unitp.Step)
				{
					r.Fill = Brushes.Green;
					r.MouseLeftButtonDown += moveUnit;
				}
                //else if (step < B.Step + B.Range) { r.Fill = Brushes.Yellow; r.MouseLeftButtonDown += r_Attack; }
                else
					r.Fill = Brushes.Red;
				step++;

				//r.MouseRightButtonDown += moveCancel;

				board.CellsPath.Add(r);
				(board.Viewer.Content as Grid).Children.Add(r);
			}
			if (step == 0 || step > board.Unitp.Step)
				return;
			r = board.CellsPath[board.CellsPath.Count - 1];
			Unit u = null;
			foreach (BoardPiece item in board.boardPieces)
			{
				if (Grid.GetColumn(item.Cell) == Grid.GetColumn(r) && Grid.GetRow(item.Cell) == Grid.GetRow(r))
				{
					if (item is Building)
					{
						r.Fill = Brushes.Red;
						r.MouseLeftButtonDown -= moveUnit;
					}
					u = item as Unit; 
				} 
			}
			if (u == null)
				return;
            //
            board.Unitp = u;
            //
            r.MouseLeftButtonDown -= moveUnit;
			r.OpacityMask = u.Cell.OpacityMask;
			r.Margin = new Thickness(1, 1, 1, 1);
			r.Opacity = 0.4;

			//if (u.Owner == Global.Players[0])
			//{
			//	if (Selected.ID == u.ID)
			//	{
			//		r.Fill = Brushes.White;
			//		r.MouseLeftButtonDown += interactReunite;
			//	}
			//} else
			//{
			//	r.Fill = Brushes.White;
			//	r.MouseLeftButtonDown += interactAttack;
			//}           
		}

		public void moveUnit(object sender, MouseButtonEventArgs e)
		{
			Rectangle s = (sender as Rectangle);
			Unit unit1 = board.Unitp;

			if (board.Split > 0)
			{
				Unit unit2 = new Unit();
				unit2.SetPosition(Grid.GetColumn(unit1.Cell), Grid.GetRow(unit1.Cell));
				unit2.Step = unit1.Step;
				unit1.Count -= board.Split;
				unit2.SetToolTip();
			}
			int x1 = Grid.GetColumn(s), y1 = Grid.GetRow(s);

			unit1.Step -= (int)s.RadiusX + 1;
			unit1.SetPosition(x1, y1);

			unit1.SetToolTip();
			moveCancel(null, null);
		}
        
	}
}
