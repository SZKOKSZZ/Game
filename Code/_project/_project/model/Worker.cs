using _project.game;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;

namespace _project.model
{
    public class Worker : Unit
    {
        ComboBox Type;
        Button b;
        public Worker(Player player, int id, int count) : base(player, id, count)
        {
            Button b = new Button();
            b.Content = "make building";
            b.Click += new RoutedEventHandler(MyBtn_Click);
            

            Type = new ComboBox();
            Type.VerticalAlignment = VerticalAlignment.Bottom;
            (Strategy.Board.Field.Content as Grid).Children.Add(Type);
            Type.Items.Add(b);
            Type.Items.Add("work");
            Type.Margin = new Thickness(0, 0, 0, 0);
        }

        private void MyBtn_Click(object sender, RoutedEventArgs e)     
        {
            Player human = new Player(Brushes.LightSkyBlue);
            MilitaryBase pro = new MilitaryBase(human, 0);
            pro.setPosition(2,6 );  
        }

        public override void setPosition(int x, int y)
        {
            base.setPosition(x, y);
            Grid.SetColumn(Type, x);
            Grid.SetRow(Type, y);
        }

        public void Select()
        {
            
        }

    }
}
