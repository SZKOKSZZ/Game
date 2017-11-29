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
		public UnitControl()
		{
			unit = new Unit();
		}
		public void dragTriggerDown(object sender, MouseButtonEventArgs e)
		{
			if (e.MiddleButton == MouseButtonState.Pressed)
			{
				unit.drag = true;
				unit.dragPoint = new Point(e.GetPosition(Field).X, e.GetPosition(Field).Y);
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
			ScrollViewer field = (Field.Parent as ScrollViewer);
			Point m = new Point(e.GetPosition(Field).X, e.GetPosition(Field).Y);
			Field.ScrollToVerticalOffset(Field.VerticalOffset + (unit.dragPoint.Y - m.Y));
			Field.ScrollToHorizontalOffset(Field.HorizontalOffset + (unit.dragPoint.X - m.X));
			unit.dragPoint = new Point(m.X, m.Y);
		}
		public void moveCancel(object sender, MouseButtonEventArgs e)
		{
			//MessageBox.Show("SWAG-YOLO");
			if (Selected == null)
				return;
			Selected.Icon.Fill = Global.Players[0].Color;
			movePathClear();
			Selected = null;
		}
		public void movePathClear()
		{
			foreach (var item in Path)
			{
				(Field.Content as Grid).Children.Remove(item);
			}
			Path.Clear();
		}
		public void movePathDraw(object sender, MouseEventArgs e)
		{
			if (Selected == null)
				return;

			int x = (int)(e.GetPosition(Field).X + Field.HorizontalOffset);
			int y = (int)(e.GetPosition(Field).Y + Field.VerticalOffset);
			x = (x / GridSize) - Grid.GetColumn(Selected.Icon);
			y = (y / GridSize) - Grid.GetRow(Selected.Icon);

			movePathClear();

			int xPos = Grid.GetColumn(Selected.Icon);
			int yPos = Grid.GetRow(Selected.Icon);
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

				if (step < Selected.Step)
				{
					r.Fill = Brushes.Green;
					r.MouseLeftButtonDown += moveUnit;
				}
                //else if (step < B.Step + B.Range) { r.Fill = Brushes.Yellow; r.MouseLeftButtonDown += r_Attack; }
                else
					r.Fill = Brushes.Red;
				step++;

				//r.MouseRightButtonDown += moveCancel;

				Path.Add(r);
				(Field.Content as Grid).Children.Add(r);
			}
			if (step == 0 || step > Selected.Step)
				return;
			r = Path[Path.Count - 1];
			Unit u = null;
			foreach (BoardPiece item in Global.Board.Instances)
			{
				if (Grid.GetColumn(item.Icon) == Grid.GetColumn(r) && Grid.GetRow(item.Icon) == Grid.GetRow(r))
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
			this.Point = u;
			r.MouseLeftButtonDown -= moveUnit;
			r.OpacityMask = u.Icon.OpacityMask;
			r.Margin = new Thickness(1, 1, 1, 1);
			r.Opacity = 0.4;

			if (u.Owner == Global.Players[0])
			{
				if (Selected.ID == u.ID)
				{
					r.Fill = Brushes.White;
					r.MouseLeftButtonDown += interactReunite;
				}
			} else
			{
				r.Fill = Brushes.White;
				r.MouseLeftButtonDown += interactAttack;
			}           
		}

		public void moveUnit(object sender, MouseButtonEventArgs e)
		{
			Rectangle s = (sender as Rectangle);
			Unit unit1 = Selected;

			if (Split > 0)
			{
				Unit unit2 = new Unit(unit1.Owner, unit1.ID, Split);
				unit2.setPosition(Grid.GetColumn(unit1.Icon), Grid.GetRow(unit1.Icon));
				unit2.Step = unit1.Step;
				unit1.Count -= Split;
				unit2.setToolTip();
			}
			int x1 = Grid.GetColumn(s), y1 = Grid.GetRow(s);

			unit1.Step -= (int)s.RadiusX + 1;
			unit1.setPosition(x1, y1);

			unit1.setToolTip();
			moveCancel(null, null);
		}
        
	}
}
