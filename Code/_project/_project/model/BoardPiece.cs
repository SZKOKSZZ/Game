using _project.game;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _project.model
{
    public class BoardPiece
    {

        public int id;
        public string name;
        public int cost;
        public int amortization;
        public int maintenanceCost;
        public BitmapImage ImageSource;
        public Player Owner;
        public Rectangle Icon;
        public TextBlock toolTip;

        public void setColor(SolidColorBrush color)
        {
            Icon.Fill = color;
        }
        public virtual void SetToolTip()
        {

        }
        public virtual void setPosition(int x, int y)
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
            (Strategy.Board.Field.Content as Grid).Children.Remove(Icon);
            Strategy.Board.Instances.Remove(this);
        }
    }
}
