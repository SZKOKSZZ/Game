using _project.game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _project.model
{
    public class Board
    {
        public List<BoardPiece> Instances;
        public int Height;
        public int Width;
        public byte GridSize;
        public ScrollViewer Field;
        public Unit Selected;
        public int Split;
        public List<Rectangle> PathCells;
        bool drag = false;
        Point dragPoint;

        private Unit Point;

        public Board(object field, int width, int height)
        {
            Field = (ScrollViewer)field;
            Width = width;
            Height = height;
            GridSize = 30;
            fillGrid();
            Field.MouseMove += movePathDraw;
            Field.PreviewMouseWheel += zoomHandler;
            Field.MouseDown += dragTriggerDown;
            Field.MouseUp += dragTriggerUp;
            Field.MouseMove += dragHandler;
            Field.PreviewMouseRightButtonDown += moveCancel;

            Instances = new List<BoardPiece>();
            PathCells = new List<Rectangle>();
        }

        void zoomHandler(object sender, MouseWheelEventArgs e)
        {
            double z = GridSize;
            double ox = Field.HorizontalOffset + (Strategy.Window.ActualWidth / 2);
            double oy = Field.VerticalOffset + (Strategy.Window.ActualHeight / 2);

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
            Field.ScrollToHorizontalOffset(ox * z - (Strategy.Window.ActualWidth / 2));
            Field.ScrollToVerticalOffset(oy * z - (Strategy.Window.ActualHeight / 2));
        }
        void dragTriggerDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            { drag = true; dragPoint = new Point(e.GetPosition(Field).X, e.GetPosition(Field).Y); }
        }
        void dragTriggerUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released)
            { drag = false; }
        }
        void dragHandler(object sender, MouseEventArgs e)
        {
            if (!drag) return;
            ScrollViewer field = (Field.Parent as ScrollViewer);
            Point m = new Point(e.GetPosition(Field).X, e.GetPosition(Field).Y);
            Field.ScrollToVerticalOffset(Field.VerticalOffset + (dragPoint.Y - m.Y));
            Field.ScrollToHorizontalOffset(Field.HorizontalOffset + (dragPoint.X - m.X));
            dragPoint = new Point(m.X, m.Y);
        }
        void fillGrid()
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

        void moveCancel(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("SWAG-YOLO");
            if (Selected == null) return;
            Selected.Icon.Fill = Strategy.Players[0].UserColor;
            movePathClear();
            Selected = null;
        }
        void movePathClear()
        {
            foreach (var item in PathCells)
            {
                (Field.Content as Grid).Children.Remove(item);
            }
            PathCells.Clear();
        }
        void movePathDraw(object sender, MouseEventArgs e)
        {
            if (Selected == null) return;

            int x = (int)(e.GetPosition(Field).X + Field.HorizontalOffset);
            int y = (int)(e.GetPosition(Field).Y + Field.VerticalOffset);
            x = (x / GridSize) - Grid.GetColumn(Selected.Icon);
            y = (y / GridSize) - Grid.GetRow(Selected.Icon);

            movePathClear();

            int xPos = Grid.GetColumn(Selected.Icon); int yPos = Grid.GetRow(Selected.Icon);
            int step = 0;
            Rectangle r;
            while (x != 0 || y != 0)
            {
                if (Math.Abs(x) > 0) { xPos += Math.Sign(x); x -= Math.Sign(x); }
                if (Math.Abs(y) > 0) { yPos += Math.Sign(y); y -= Math.Sign(y); }
                r = new Rectangle();
                r.Margin = new Thickness(1, 1, 1, 1);//(GridSize / 4, GridSize / 4, GridSize / 4, GridSize / 4);
                r.Opacity = 0.4;
                Grid.SetColumn(r, xPos);
                Grid.SetRow(r, yPos);
                r.RadiusX = step;
                //Panel.SetZIndex(r,-1);

                if (step < Selected.Step) { r.Fill = Brushes.Green; r.MouseLeftButtonDown += moveUnit; }
                //else if (step < B.Step + B.Range) { r.Fill = Brushes.Yellow; r.MouseLeftButtonDown += r_Attack; }
                else r.Fill = Brushes.Red;
                step++;

                //r.MouseRightButtonDown += moveCancel;

                PathCells.Add(r);
                (Field.Content as Grid).Children.Add(r);
            }
            if (step == 0 || step > Selected.Step) return;
            r = PathCells[PathCells.Count - 1];
            Unit u = null;
            foreach (BoardPiece item in Strategy.Board.Instances)
            {
                if (Grid.GetColumn(item.Icon) == Grid.GetColumn(r) && Grid.GetRow(item.Icon) == Grid.GetRow(r))
                {
                    if (item is Building) { r.Fill = Brushes.Red; r.MouseLeftButtonDown -= moveUnit; }
                    u = item as Unit;
                }
            }
            if (u == null) return;
            this.Point = u;
            r.MouseLeftButtonDown -= moveUnit;
            r.OpacityMask = u.Icon.OpacityMask; r.Margin = new Thickness(1, 1, 1, 1); r.Opacity = 0.4;

            if (u.Owner == Strategy.Players[0])
            {
                if (Selected.ID == u.ID) { r.Fill = Brushes.White; r.MouseLeftButtonDown += interactReunite; }
            }
            else { r.Fill = Brushes.White; r.MouseLeftButtonDown += interactAttack; }
        }
        void moveUnit(object sender, MouseButtonEventArgs e)
        {
            Rectangle s = (sender as Rectangle);
            Unit u = Selected;

            if (Split > 0)
            {
                Unit u2 = new Unit(u.Owner, u.ID, Split);
                u2.setPosition(Grid.GetColumn(u.Icon), Grid.GetRow(u.Icon));
                u2.Step = u.Step;
                u.Count -= Split;
                u2.SetToolTip();
            }
            int x1 = Grid.GetColumn(s), y1 = Grid.GetRow(s);

            u.Step -= (int)s.RadiusX + 1;
            u.setPosition(x1, y1);

            u.SetToolTip();
            moveCancel(null, null);
        }
        void interactReunite(object sender, MouseButtonEventArgs e)
        {
            this.Point.Step = Math.Min((int)(Selected.Step - (PathCells[PathCells.Count - 1].RadiusX + 1)), this.Point.Step);
            this.Point.Count += Selected.Count;
            this.Point.SetToolTip();

            Selected.remove();
            Selected = null;
            movePathClear();
        }
        void interactAttack(object sender, MouseButtonEventArgs e)
        {
            Selected.setPosition(Grid.GetColumn(this.Point.Icon), Grid.GetRow(this.Point.Icon));
            Selected.Step -= (int)PathCells[PathCells.Count - 1].RadiusX + 1;

            Unit u1 = Selected;
            Unit u2 = this.Point;

            double val1 = (u1.Count * u1.Health) / u2.Damage;
            double val2 = (u2.Count * u2.Health) / u1.Damage;

            if (val1 < val2)
            {
                u2.Count -= (int)((val1 * u1.Damage) / u2.Health);
                u1.remove();
                u2.SetToolTip();
            }
            else
            {
                u1.Count -= (int)((val2 * u2.Damage) / u1.Health);
                u2.remove();
                if (u1.Count == 0) u1.remove();
                u1.SetToolTip();
            }

            moveCancel(null, null);
        }
    }
}
