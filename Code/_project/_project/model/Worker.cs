using _project.game;
using System.Windows;
using System.Windows.Controls;

namespace _project.model
{
    public class Worker : Unit
    {
        ComboBox Type;
        public Worker(Player player, int id, int count) : base(player, id, count)
        {
            Type = new ComboBox();
            Type.VerticalAlignment = VerticalAlignment.Bottom;
            (Strategy.Board.Field.Content as Grid).Children.Add(Type);
            Type.Items.Add("0120312412");
            Type.Items.Add("12301301224123012");
            Type.Margin = new Thickness(0, 0, 0, 0);
        }

        public override void setPosition(int x, int y)
        {
            base.setPosition(x, y);
            Grid.SetColumn(Type, x);
            Grid.SetRow(Type, y);
        }
    }
}
