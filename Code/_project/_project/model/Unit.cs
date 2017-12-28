using _project.game;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _project.model
{
    public class Unit : BoardPiece
    {
        public int Step;
        public int ID;
        public int Count;
        public int StepMax;
        public int Damage;
        public int Health;
        public string UnitName;
        public int Cost;

        public Unit(Player player, int id, int count)
        {
            Icon = new Rectangle();
            Strategy.Database.readUnit(this, id);
            Step = StepMax;
            Count = count;
            ID = id;
            Owner = player;
            Icon.Margin = new Thickness(1, 1, 1, 1);

            if (player != null)
            {
                Icon.Fill = player.UserColor;
                (Strategy.Board.Field.Content as Grid).Children.Add(Icon);
                Strategy.Board.Instances.Add(this);
            }
            else Icon.Fill = Brushes.White;

            if (player == Strategy.Players[0])
            {
                Icon.MouseLeftButtonDown += unitSelect;
                //setToolTip();
            }
            SetToolTip();
        }

        private void unitSelect(object sender, MouseButtonEventArgs e)
        {
            Strategy.Board.Split = 0;
            if (Count >= 2 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                Strategy.Board.Split = Count / 2;
                (sender as Rectangle).Fill = Brushes.SlateGray;
            }
            else (sender as Rectangle).Fill = Brushes.White;
            Strategy.Board.Selected = this;
        }

        public override void SetToolTip()
        {
            TextBlock tt = new TextBlock();
            tt.Inlines.Clear();
            if (Owner == null)
                tt.Inlines.Add(new Run(UnitName)
                {
                    Foreground = Brushes.White
                });
            else
            {
                tt.Inlines.Add(new Run(UnitName)
                {
                    Foreground = Owner.UserColor
                });
                tt.Inlines.Add(new Run(" x" + Count));
            }
            if (Owner == Strategy.Players[0])
                tt.Inlines.Add(new Run("\n" + "👣 " + Step + "/" + StepMax));
            else
                tt.Inlines.Add(new Run("\n" + "👣 " + StepMax));
            tt.Inlines.Add(new Run("\n" + "🛡 " + Health));
            tt.Inlines.Add(new Run("\n" + "⚔ " + Damage)
            {
                Foreground = Brushes.White
            });
            if (Owner == null)
                tt.Inlines.Add(new Run("\n" + "Cost: " + Cost + "$"));
            this.toolTip = tt;
            Icon.ToolTip = this.toolTip;
        }
    }
}
