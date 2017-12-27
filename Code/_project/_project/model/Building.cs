using _project.game;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _project.model
{
    public class Building : BoardPiece
    {
        public string Name;
        public int ID;
        public int[] buildArray;
        public int Cost;



        public Building(Player player, int id)
        {
            Icon = new Rectangle();
            Strategy.Database.readBuilding(this, id);
            ID = id;
            Owner = player;
            Icon.Fill = player.UserColor;
            Icon.Margin = new Thickness(1, 1, 1, 1);
            SetToolTip();

            (Strategy.Board.Field.Content as Grid).Children.Add(Icon);
            Strategy.Board.Instances.Add(this);

            if (player == Strategy.Players[0]) Icon.MouseLeftButtonDown += buildingSelect;
        }

        public override void SetToolTip()
        {
            TextBlock tt = new TextBlock();
            tt.Inlines.Clear();
            if (Owner == null) { tt.Inlines.Add(new Run(Name) { Foreground = Brushes.White }); tt.Inlines.Add(new Run("\n" + "Cost: " + Cost + "$")); }
            else tt.Inlines.Add(new Run(Name) { Foreground = Owner.UserColor });
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
            Strategy.Grid.Children.Add(b);
            Strategy.Board.Field.PreviewMouseDown += panelRemove;
        }
        void panelRemove(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < Strategy.Grid.Children.Count; i++)
            {
                if (Strategy.Grid.Children[i] is Border && (Strategy.Grid.Children[i] as Border).Name == "unitsArray") Strategy.Grid.Children.Remove(Strategy.Grid.Children[i]);
            }
            Strategy.Board.Field.PreviewMouseDown -= panelRemove;
        }
    }
}
