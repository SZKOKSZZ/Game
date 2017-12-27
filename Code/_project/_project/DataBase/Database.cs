using _project.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _project.DataBase
{
    public class Database
    {
        public void readUnit(Unit u, int id)
        {
            StreamReader fin = new StreamReader(Directory.GetCurrentDirectory() + @"\DB\unit.dbt");
            string[] str; 
bool res; int ind = -1;
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
            b.cost = int.Parse(str[4]);
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
