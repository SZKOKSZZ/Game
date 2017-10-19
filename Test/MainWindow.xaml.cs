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

namespace _gridTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Board B;
        public static List<BoardPiece> BP = new List<BoardPiece>();
        public MainWindow()
        {
            InitializeComponent();
            B = new Board(field);
            BoardPiece bp = new BoardPiece();
            bp.setPosition(5, 3,1,1);
            BP.Add(bp);
        }

        private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            int xval = int.Parse((sender as Button).Content.ToString().Split(',')[0]);
            int yval = int.Parse((sender as Button).Content.ToString().Split(',')[1]);
            field.ScrollToVerticalOffset(field.VerticalOffset + yval*5);
            field.ScrollToHorizontalOffset(field.HorizontalOffset + xval*5);
        }
    }

    public class BoardPiece
    {
        public Rectangle Icon;
        TextBlock ToolTip;

        public int Step = 10;

        public BoardPiece()
        {
            ImageBrush icon = new ImageBrush { ImageSource = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\icons\" + "00" + ".png")) };
            Rectangle r = new Rectangle { Fill = Brushes.Yellow, OpacityMask = icon };
            r.MouseDown += r_MouseDown;
            Icon = r;
            (MainWindow.B.field.Content as Grid).Children.Add(r);
            //setPosition(x,y);
            setTooltip();
        }

        public void setPosition(int x,int y,int w, int h)
        {
            Grid.SetColumn(Icon, x);
            Grid.SetRow(Icon, y);

            Grid.SetColumnSpan(Icon,w);
            Grid.SetRowSpan(Icon, h);
        }

        private void r_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Rectangle).Fill = Brushes.White;
            MainWindow.B.Selected = this;
        }

        private void setTooltip()
        {
            TextBlock tt = new TextBlock();
            tt.Inlines.Clear();
            tt.Inlines.Add(new Run("Unit Name") { Foreground = Brushes.Yellow });
            tt.Inlines.Add(new Run("\n" + "Step: "+Step));
            tt.Inlines.Add(new Run("\n" + "Damage: y") { Foreground = Brushes.White });
            ToolTip = tt;
            Icon.ToolTip = ToolTip;
        }
    }

    public class Board
    {
        public byte height = 30; public byte width = 60; public byte gridSize = 32;
        public ScrollViewer field;
        public BoardPiece Selected;
        public List<Rectangle> Path;
        //public List<Button> buttonsUsed;
        //public List<Button> buttonsFree;
        //public List<ProgressBar> bars;

        public Board(object field)
        {
            this.field = (ScrollViewer)field;
            fillGrid();
            //this.field.MouseDown += g_MouseDown;
            //this.field.PreviewMouseRightButtonDown += g_MouseDown;
            this.field.MouseMove += g_MouseOver;
            Path = new List<Rectangle>();
        }

        void fillGrid()
        {
            //buttonsFree = new List<Button>();
            //buttonsUsed = new List<Button>();
            //bars = new List<ProgressBar>();
            BorderGrid g = new BorderGrid();
            //g.Background = Brushes.Black;
            g.IsHitTestVisible = true;
            for (int i = 0; i < width; i++)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = new GridLength(gridSize, GridUnitType.Pixel);
                g.ColumnDefinitions.Add(c);
                for (int j = 0; j < height; j++)
                {
                    if (i == 0)
                    {
                        RowDefinition r = new RowDefinition();
                        r.Height = new GridLength(gridSize, GridUnitType.Pixel);
                        g.RowDefinitions.Add(r);

                    }
                    /*Border b = new Border();
                    b.BorderThickness = new Thickness(1);
                    Grid.SetColumn(b, i);
                    Grid.SetRow(b, j);
                    g.Children.Add(b);*/
                    /*Button btn = new Button();
                    Grid.SetColumn(btn, i);
                    Grid.SetRow(btn, j);
                    btn.Width =32; btn.Height = 32;
                    //btn.Content = "C";
                    g.Children.Add(btn);*/
                    //buttonsFree.Add(btn);
                }
            }
            field.Content = g;
        }

        void g_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("ASD");
            /*for (int i = 0; i < MainWindow.BP.Count; i++)
            {
                MainWindow.BP[i].Icon.Fill = Brushes.Yellow;
            }*/
            Rectangle s = (sender as Rectangle);
            if (s.Fill != Brushes.Green) return;
            BoardPiece b = MainWindow.B.Selected;

            //s = MainWindow.B.Path[MainWindow.B.Path.IndexOf(s)];

            int x1 = Grid.GetColumn(sender as Rectangle);
            int y1= Grid.GetRow(sender as Rectangle);
            int x2 = Grid.GetColumn(s);
            int y2 = Grid.GetRow(s);


            b.Step -= MainWindow.B.Path.IndexOf(s)+1;
            b.setPosition(x1, y1, 1, 1); 
            

            //5-3
            //8-2

            g_RightMouseDown(null,null);
            MainWindow.B.Selected = b;
        }

        void g_RightMouseDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("ASD");
            /*for (int i = 0; i < MainWindow.BP.Count; i++)
            {
                MainWindow.BP[i].Icon.Fill = Brushes.Yellow;
            }*/
            if (MainWindow.B.Selected == null) return;
            MainWindow.B.Selected.Icon.Fill = Brushes.Yellow;
            if (MainWindow.B.Path != null)
            foreach (var item in MainWindow.B.Path)
            {
                (MainWindow.B.field.Content as Grid).Children.Remove(item);
            }
            MainWindow.B.Selected = null;
        }

        void g_MouseOver(object sender, MouseEventArgs e)
        {
            if (MainWindow.B.Selected == null) return;
            BoardPiece B = MainWindow.B.Selected;

            int x = (int)(e.GetPosition(field).X + field.HorizontalOffset);
            int y = (int)(e.GetPosition(field).Y + field.VerticalOffset);
            x=(x/gridSize)-Grid.GetColumn(B.Icon);
            y=(y/gridSize)-Grid.GetRow(B.Icon);
            
            ((MainWindow)System.Windows.Application.Current.MainWindow).txt_stats.Text = "x:" + x + " y:" + y;

            if (MainWindow.B.Path!=null)
            foreach (var item in MainWindow.B.Path)
            {
                (MainWindow.B.field.Content as Grid).Children.Remove(item);
            }


            int xPos= Grid.GetColumn(B.Icon); int yPos= Grid.GetRow(B.Icon);
            int step = 0;
            while (x!=0 || y!=0)
            {
                if (Math.Abs(x) > 0) { xPos += Math.Sign(x); x -= Math.Sign(x); }
                if (Math.Abs(y) > 0) { yPos += Math.Sign(y); y -= Math.Sign(y); }
                Rectangle r = new Rectangle();
                r.Margin = new Thickness(1, 1, 1, 1);
                Grid.SetColumn(r, xPos);
                Grid.SetRow(r, yPos);

                if (step < MainWindow.B.Selected.Step) r.Fill = Brushes.Green;
                else r.Fill = Brushes.Red;
                step++;

                r.PreviewMouseRightButtonDown += g_RightMouseDown;
                r.PreviewMouseLeftButtonDown += g_LeftMouseDown;
                MainWindow.B.Path.Add(r);
                (MainWindow.B.field.Content as Grid).Children.Add(r);
            }
            
            /*for (int i = 0; i < ; i++)
            {
                for (int j = 0; j < ; j++)
                {
                    Rectangle r = new Rectangle();
                    Grid.SetColumn(r,)
                }
            }*/
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
            // draw last line at the bottom
            dc.DrawLine(pen, new Point(0, topOffset), new Point(this.ActualWidth, topOffset));

            foreach (ColumnDefinition column in this.ColumnDefinitions)
            {
               dc.DrawLine(pen, new Point(leftOffset, 0), new Point(leftOffset, this.ActualHeight));
               leftOffset += column.ActualWidth;
            }
            // draw last line on the right
            dc.DrawLine(pen, new Point(leftOffset, 0), new Point(leftOffset, this.ActualHeight));

            base.OnRender(dc);
        }
    }
}
