using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _project.game
{
    public enum materials { gold, silver,diamond,iron,}  //stb...
    public class Economy
    {
        public int Turn { get; set; }
        Random rnd = new Random();
        public materials mat;

        public void GenerateMaterial()
        {
            int random = rnd.Next(1, 100);          
            if(random<2)
            {
                mat = materials.diamond;
                MessageBox.Show("your found diamond!!!!");
                GetMoney(100000);
            }
            else if (random < 10)
            {
                mat = materials.gold;
                MessageBox.Show("your found gold!!!");
                GetMoney(1000);
            }
            else if (random < 15)
            {
                mat = materials.silver;
                MessageBox.Show("your found silver!!");
                GetMoney(100);
            }
            else if (random < 20)
            {
                mat = materials.iron;
                MessageBox.Show("your found iron!");
                GetMoney(10);
            }
            else { MessageBox.Show("your found nothing! sorry!"); }
            //
            //...
        }

        public int GetMoney(int found)
        {

            return found;
        }

        


    }
}
