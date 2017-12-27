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
        public int[] buildArray;

        public Building(Player player, int ID)
        {
            Icon = new Rectangle();
            //Strategy.Database.readBuilding(this, id);
            id = ID;
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
            if (Owner == null) { tt.Inlines.Add(new Run(Name) { Foreground = Brushes.White }); tt.Inlines.Add(new Run("\n" + "Cost: " + cost + "$")); }
            else tt.Inlines.Add(new Run(Name) { Foreground = Owner.UserColor });
            this.toolTip = tt;
            Icon.ToolTip = this.toolTip;
        }
        void buildingSelect(object sender, MouseButtonEventArgs e)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(0, 1, 1, 0);
            border.Margin = new Thickness(0, 0, 0, -1);
            border.BorderBrush = Brushes.DarkGray;
            border.HorizontalAlignment = HorizontalAlignment.Left;
            border.VerticalAlignment = VerticalAlignment.Bottom;
            border.Background = Brushes.DimGray;
            Grid.SetRow(border, 0);

            Grid grid = new Grid();
            border.Child = grid;
            grid.Height = 42;

            Unit unit;
            for (int i = 0; i < buildArray.Length; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(grid.Height, GridUnitType.Pixel);
                grid.ColumnDefinitions.Add(column);

                unit = new Unit(null, 8, 0);
                unit.setPosition(i, 1);
                grid.Children.Add(unit.Icon);
            }


            border.Name = "unitsArray";
            Strategy.Grid.Children.Add(border);
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
