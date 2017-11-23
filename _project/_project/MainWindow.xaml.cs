using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            globalizing();
            initalizing();
        }

        /*private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            int xval = int.Parse((sender as Button).Content.ToString().Split(',')[0]);
            int yval = int.Parse((sender as Button).Content.ToString().Split(',')[1]);
            field.ScrollToVerticalOffset(field.VerticalOffset + yval * 5);
            field.ScrollToHorizontalOffset(field.HorizontalOffset + xval * 5);
        }*/
        private void globalizing()
        {
            Global.Grid = grd_Main;
            Global.Board = new Board(field, 140, 140);
            Global.StatusBar = txt_stats;
            Global.Window = this;
        }

        private void initalizing()
        {
            Player human = new Player(Brushes.LightSkyBlue); human.setMoney(1000);
            Player cpu = new Player(Brushes.GreenYellow);
            Player cpu2 = new Player(Brushes.Plum);

            Unit u;
            u = new Unit(human, 0, 10); u.setPosition(5, 2);
            u = new Unit(human, 0, 10); u.setPosition(4, 4);
            u = new Unit(cpu, 0, 12); u.setPosition(8, 4);

            u = new Unit(cpu2, 0, 3); u.setPosition(12, 3);

            Building b;
            b = new Building(human, 0); b.setPosition(7, 2);
        }

        private void btn_nextRound_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Global.Board.Instances)
            {
                if (item is Unit) (item as Unit).Step = (item as Unit).StepMax;
                item.setToolTip();
            }
            MessageBox.Show("All steps has been reset.");
        }
    }

    class Global
    {
        public static Board Board;
        public static Database Database = new Database();
        public static List<Player> Players = new List<Player>();
        public static Grid Grid;
        public static TextBlock StatusBar;
        public static Window Window;
    }

    public class BoardPiece
    {
        public Player Owner;
        public Rectangle Icon;
        public TextBlock ToolTip;

        public void setColor(SolidColorBrush color)
        {
            Icon.Fill = color;
        }
        public virtual void setToolTip()
        {

        }
        public void setPosition(int x, int y)
        {
            Grid.SetColumn(Icon, x);
            Grid.SetRow(Icon, y);
        }
        public void setSize(int width, int height)
        {
            Grid.SetColumnSpan(Icon, width);
            Grid.SetRowSpan(Icon, height);
        }
        public void remove()
        {
            (Global.Board.Field.Content as Grid).Children.Remove(Icon);
            Global.Board.Instances.Remove(this);
        }
    }

    public class Unit : BoardPiece
    {
        public int Step; public int ID;
        public int Count, StepMax, Damage, Health;
        public string Name; public int Cost;

        public Unit(Player player, int id, int count)
        {
            Icon = new Rectangle();
            Global.Database.readUnit(this, id);
            Step = StepMax;
            Count = count;
            ID = id;
            Owner = player;

            Icon.Margin = new Thickness(1, 1, 1, 1);
            if (player != null)
            { 
                Icon.Fill = player.Color;
                (Global.Board.Field.Content as Grid).Children.Add(Icon);
                Global.Board.Instances.Add(this);
            }
            else Icon.Fill = Brushes.White;

            if (player == Global.Players[0])
            {
                Icon.MouseLeftButtonDown += unitSelect;
                //setToolTip();
            }
            setToolTip();
        }

        private void unitSelect(object sender, MouseButtonEventArgs e)
        {
            Global.Board.Split = 0;
            if (Count >= 2 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                Global.Board.Split = Count / 2;
                (sender as Rectangle).Fill = Brushes.SlateGray;
            }
            else (sender as Rectangle).Fill = Brushes.White;
            Global.Board.Selected = this;
        }

        public override void setToolTip()
        {
            TextBlock tt = new TextBlock();
            tt.Inlines.Clear();
            if (Owner == null) tt.Inlines.Add(new Run(Name) { Foreground = Brushes.White });
                else { tt.Inlines.Add(new Run(Name) { Foreground = Owner.Color }); tt.Inlines.Add(new Run(" x" + Count)); }
            if (Owner == Global.Players[0]) tt.Inlines.Add(new Run("\n" + "👣 " + Step + "/" + StepMax));
            else tt.Inlines.Add(new Run("\n" + "👣 " + StepMax));
            tt.Inlines.Add(new Run("\n" + "🛡 " + Health));
            tt.Inlines.Add(new Run("\n" + "⚔ " + Damage) { Foreground = Brushes.White });
            if (Owner == null) tt.Inlines.Add(new Run("\n" + "Cost: " + Cost + "$"));
            this.ToolTip = tt;
            Icon.ToolTip = this.ToolTip;
        }
    }

    public class Building : BoardPiece
    {
        public string Name; public int ID;
        public int[] buildArray; public int Cost;

        public Building(Player player, int id)
        {
            Icon = new Rectangle();
            Global.Database.readBuilding(this, id);
            ID = id;
            Owner = player;
            Icon.Fill = player.Color;
            Icon.Margin = new Thickness(1, 1, 1, 1);
            setToolTip();

            (Global.Board.Field.Content as Grid).Children.Add(Icon);
            Global.Board.Instances.Add(this);

            if (player == Global.Players[0]) Icon.MouseLeftButtonDown += buildingSelect;
        }

        public override void setToolTip()
        {
            TextBlock tt = new TextBlock();
            tt.Inlines.Clear();
            if (Owner == null) { tt.Inlines.Add(new Run(Name) { Foreground = Brushes.White }); tt.Inlines.Add(new Run("\n" + "Cost: " + Cost + "$")); }
            else  tt.Inlines.Add(new Run(Name) { Foreground = Owner.Color });
            this.ToolTip = tt;
            Icon.ToolTip = this.ToolTip;
        }
        void buildingSelect(object sender, MouseButtonEventArgs e)
        {
            Border b = new Border();
            b.BorderThickness = new Thickness(0, 1, 1, 0);
            b.Margin = new Thickness(0, 0, 0, -1);
            b.BorderBrush = Brushes.DarkGray;
            b.HorizontalAlignment = HorizontalAlignment.Left;
            b.VerticalAlignment = VerticalAlignment.Bottom;
            b.Background = Brushes.DimGray;
            Grid.SetRow(b, 0);

            Grid g = new Grid(); b.Child = g;
            g.Height = 42;

            Unit u;
            for (int i = 0; i < buildArray.Length; i++)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = new GridLength(g.Height, GridUnitType.Pixel);
                g.ColumnDefinitions.Add(c);

                u = new Unit(null, 8, 0);
                u.setPosition(i, 1);
                g.Children.Add(u.Icon);
            }


            b.Name = "unitsArray";
            Global.Grid.Children.Add(b);
            Global.Board.Field.PreviewMouseDown += panelRemove;
        }
        void panelRemove(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < Global.Grid.Children.Count; i++)
            {
                if (Global.Grid.Children[i] is Border && (Global.Grid.Children[i] as Border).Name == "unitsArray") Global.Grid.Children.Remove(Global.Grid.Children[i]);
            }
            Global.Board.Field.PreviewMouseDown -= panelRemove;
        }
    }

    public class Board
    {
        public List<BoardPiece> Instances;
        public int Height;
        public int Width;
        public byte GridSize;
        public ScrollViewer Field;
        public Unit Selected;
        public int Split;
        public List<Rectangle> Path;
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
            Path = new List<Rectangle>();
        }

        void zoomHandler(object sender, MouseWheelEventArgs e)
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
            dragPoint = new Point(m.X,m.Y);
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
            Selected.Icon.Fill = Global.Players[0].Color;
            movePathClear();
            Selected = null;
        }
        void movePathClear()
        {
            foreach (var item in Path)
            {
                (Field.Content as Grid).Children.Remove(item);
            }
            Path.Clear();
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

                Path.Add(r);
                (Field.Content as Grid).Children.Add(r);
            }
            if (step == 0 || step > Selected.Step) return;
            r = Path[Path.Count - 1];
            Unit u = null;
            foreach (BoardPiece item in Global.Board.Instances)
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

            if (u.Owner == Global.Players[0])
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
                u2.setToolTip();
            }
            int x1 = Grid.GetColumn(s), y1 = Grid.GetRow(s);

            u.Step -= (int)s.RadiusX + 1;
            u.setPosition(x1, y1);

            u.setToolTip();
            moveCancel(null, null);
        }
        void interactReunite(object sender, MouseButtonEventArgs e)
        {
            this.Point.Step = Math.Min((int)(Selected.Step - (Path[Path.Count - 1].RadiusX + 1)), this.Point.Step);
            this.Point.Count += Selected.Count;
            this.Point.setToolTip();

            Selected.remove();
            Selected = null;
            movePathClear();
        }
        void interactAttack(object sender, MouseButtonEventArgs e)
        {
            Selected.setPosition(Grid.GetColumn(this.Point.Icon), Grid.GetRow(this.Point.Icon));
            Selected.Step -= (int)Path[Path.Count - 1].RadiusX + 1;

            Unit u1 = Selected;
            Unit u2 = this.Point;

            double val1 = (u1.Count * u1.Health) / u2.Damage;
            double val2 = (u2.Count * u2.Health) / u1.Damage;

            if (val1 < val2)
            {
                u2.Count -= (int)((val1 * u1.Damage) / u2.Health);
                u1.remove();
                u2.setToolTip();
            }
            else
            {
                u1.Count -= (int)((val2 * u2.Damage) / u1.Health);
                u2.remove();
                if (u1.Count == 0) u1.remove();
                u1.setToolTip();
            }

            moveCancel(null, null);
        }
    }

    public class Player
    {
        public SolidColorBrush Color;
        int Money;

        public Player(SolidColorBrush color)
        {
            Color = color;
            Money = 0;
            Global.Players.Add(this);
        }

        public int getMoney()
        {
            return Money;
        }
        public void setMoney(int money)
        {
            Money = money;
            Global.StatusBar.Text = Money.ToString() + "$";
        }
    }

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

    public class Database
    {
        public void readUnit(Unit u, int id)
        {
            StreamReader fin = new StreamReader(Directory.GetCurrentDirectory() + @"\DB\unit.dbt");
            string[] str; bool res; int ind = -1;
            do
            {
                str = fin.ReadLine().Split(';');
                res = int.TryParse(str[0], out ind);
                if (!res) ind = -1;
            } while (ind != id && !(fin.EndOfStream));

            u.Name = str[1];
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\icons\" + str[2])) u.Icon.OpacityMask = new ImageBrush { ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\icons\error.png")) };
            else u.Icon.OpacityMask = new ImageBrush { ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\icons\" + str[2])) };
            u.Cost = int.Parse(str[3]);
            u.StepMax = int.Parse(str[4]);
            u.Health = int.Parse(str[5]);
            u.Damage = int.Parse(str[6]);

            fin.Close();
        }
        public void readBuilding(Building b, int id)
        {
            StreamReader fin = new StreamReader(Directory.GetCurrentDirectory() + @"\DB\building.dbt");
            string[] str; bool res; int ind = -1;
            do
            {
                str = fin.ReadLine().Split(';');
                res = int.TryParse(str[0], out ind);
                if (!res) ind = -1;
            } while (ind != id && !(fin.EndOfStream));

            b.Name = str[1];
            if (!File.Exists(Directory.GetCurrentDirectory() + @"\icons\" + str[2])) b.Icon.OpacityMask = new ImageBrush { ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\icons\error.png")) };
            else b.Icon.OpacityMask = new ImageBrush { ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\icons\" + str[2])) };
            b.Cost = int.Parse(str[4]);
            b.setSize(int.Parse(str[3]), int.Parse(str[3]));

            b.buildArray = new int[int.Parse(str[5])];
            for (int i = 0; i < int.Parse(str[5]); i++)
            {
                b.buildArray[i] = int.Parse(str[6 + i]);
            }

            fin.Close();
        }
    }
}