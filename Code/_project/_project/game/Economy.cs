using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _project.game
{
    public enum materials { gold, silver, diamond, iron, food }  //stb...
    public class Economy
    {
        Random rnd = new Random();
        public int Money { get; set; }
        public int Population { get; set; }

        public int Pollution { get; set; }
        public int Energy { get; set; }
        public int Science { get; set; }
        public int Production { get; set; }
        public int ArmyPower { get; set; }
        public int Turn { get; set; }
        public materials mat;

        public Economy()
        {
            SetMoney(10);
            SetPopulation(954);
        }

        public void GenerateMaterial()
        {
            int random = rnd.Next(1, 100);
            if (0 < random && random < 2)
            {
                mat = materials.diamond;
                MessageBox.Show("your found diamond!!!!");
                SetMoney(100000);
            }
            else if (2 < random && random < 10)
            {
                mat = materials.gold;
                MessageBox.Show("your found gold!!!");
                SetMoney(1000);
            }
            else if (10 < random && random < 25)
            {
                mat = materials.silver;
                MessageBox.Show("your found silver!!");
                SetMoney(100);
            }
            else if (25 < random && random < 45)
            {
                mat = materials.iron;
                MessageBox.Show("your found iron!");
                SetMoney(10);
            }
            else if (45 < random && random < 99)
            {
                mat = materials.food;
                MessageBox.Show("your found food!");
                SetMoney(1);
            }
            else
            {
                MessageBox.Show("your found nothing! sorry!");
            }
            //
            //...
        }

        public void SetMoney(int money)
        {
            Money = Money + money;
            Strategy.StatusBar.Text = "Global Money: " + Money.ToString() + " $ ";
        }
        public void SetPopulation(int pop)
        {
            Population = Population + pop;
            Strategy.StatusBar.Text = Strategy.StatusBar.Text + "Population: " + Population.ToString() + " f ";
        }



         
      



    }
}
