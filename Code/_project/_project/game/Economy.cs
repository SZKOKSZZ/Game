using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _project.game
{
    public enum Event { CapableEngaging, BirthEngageDisengage, GetPregnant, ChildrenCount, TimeChildren, Die }
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

        List<Human> population;
        public Economy()
        {
            population = new List<Human>();
            SetMoney(10);
            for (int i = 0; i < 100; i++)
            {
                Male male = new Male(18);
                Female female = new Female(18);
                population.Add(male);
                population.Add(female);
            }

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
            Strategy.StatusBar.Text = " Global Money: " + Money.ToString() + " $ ";

        }
        public void SetPopulation(int p)
        {
            Population = Population + p;
            Strategy.StatusBar.Text = " Population: " + Population.ToString() + " f ";

        }

        public void PopulationGrowth()
        {
            Simulation sim = new Simulation(population);
            sim.Execute();
            MessageBox.Show(" population " + sim.humans.Count.ToString());
        }
    }

    public class Simulation
    {
        public List<Human> humans { get; set; }
        public int Time { get; set; }
        private int _currentTime;
        private readonly Dictionary<Event, IDistribution> _distributions;


        public Simulation(IEnumerable<Human> population)
        {
            humans = new List<Human>(population);
            // Time = time;
            _distributions = new Dictionary<Event, IDistribution>
            {
                { Event.CapableEngaging, new Poisson(18) },
                { Event.BirthEngageDisengage, new ContinuousUniform() },
                { Event.GetPregnant, new Normal(28, 8) },
                { Event.ChildrenCount, new Normal(2, 6) },
                { Event.TimeChildren, new Exponential(8) },
                { Event.Die, new Poisson(70) },};

            foreach (Human Human in humans)
            {
                Human.LifeTime = ((Poisson)_distributions[Event.Die]).Sample();

                Human.RelationAge = ((Poisson)_distributions[Event.CapableEngaging]).Sample();

                if (Human is Female)
                {
                    (Human as Female).PregnancyAge = ((Normal)_distributions[Event.GetPregnant]).Sample();
                    (Human as Female).ChildrenCount = ((Normal)_distributions[Event.ChildrenCount]).Sample();
                }
            }
        }
        public void Execute()
        {

            for (int i = 0; i < humans.Count; i++)
            {
                Human human = humans[i];

                if (human is Female && (human as Female).IsPregnant)
                    humans.Add((human as Female).GiveBirth(_distributions,
                      _currentTime));

                if (human.SuitableRelation())
                    human.FindPartner(humans, _currentTime, _distributions);

                if (human.Engaged)
                {

                    if (human.EndRelation(_distributions))
                        human.Disengage();

                    if (human is Female &&
                    (human as Female).SuitablePregnancy(_currentTime))
                        (human as Female).IsPregnant = true;
                }

                if (human.Age.Equals(human.LifeTime))
                {

                    if (human.Engaged)
                        human.Disengage();
                    humans.RemoveAt(i);
                }
                human.Age++;

            }
        }
    }



    public abstract class Human
    {
        public int Age { get; set; }
        public int RelationAge { get; set; }
        public int LifeTime { get; set; }
        public double TimeChildren { get; set; }
        public Human Couple { get; set; }

        public Human(int age)
        {
            Age = age;
        }
        public bool SuitableRelation()
        {
            return Age >= RelationAge && Couple == null;
        }

        public bool SuitablePartner(Human human)
        {
            return ((human is Male && this is Female) ||
            (human is Female && this is Male)) && Math.Abs(human.Age - Age) <= 5;
        }

        public bool Engaged
        {
            get { return Couple != null; }
        }

        public void Disengage()
        {
            Couple.Couple = null;
            Couple = null;
            TimeChildren = 0;
        }

        public bool EndRelation(Dictionary<Event, IDistribution> distributions)
        {
            double sample = ((ContinuousUniform)distributions[Event.BirthEngageDisengage]).Sample();

            if (Age >= 14 && Age <= 20 && sample <= 0.7)
                return true;
            if (Age >= 21 && Age <= 28 && sample <= 0.5)
                return true;
            if (Age >= 29 && sample <= 0.2)
                return true;
            return false;
        }

        public void FindPartner(IEnumerable<Human> population, int currentTime, Dictionary<Event, IDistribution> distributions)
        {
            foreach (Human candidate in population)
            {
                if (SuitablePartner(candidate) &&
                candidate.SuitableRelation() &&
                ((ContinuousUniform)distributions[Event.BirthEngageDisengage]).Sample() <= 0.5)
                {
                    candidate.Couple = this;
                    Couple = candidate;
                    double childTime = ((Exponential)distributions[Event.TimeChildren]).Sample() * 100;
                    candidate.TimeChildren = currentTime + childTime;
                    TimeChildren = currentTime + childTime;
                    break;
                }
            }
        }
    }
    public class Male : Human
    {
        public Male(int age) : base(age) { }
    }
    public class Female : Human
    {
        public bool IsPregnant { get; set; }
        public double PregnancyAge { get; set; }
        public double ChildrenCount { get; set; }
        public Female(int age) : base(age) { }
        public bool SuitablePregnancy(int currentTime)
        {
            return Age >= PregnancyAge && currentTime <= TimeChildren && ChildrenCount > 0;
        }

        public Human GiveBirth(Dictionary<Event, IDistribution> distributions, int currentTime)
        {
            double sample = ((ContinuousUniform)distributions[Event.BirthEngageDisengage]).Sample();
            Human child = sample > 0.5 ? (Human)new Male(0) : new Female(0);
            ChildrenCount--;
            child.LifeTime = ((Poisson)distributions[Event.Die]).Sample();
            child.RelationAge = ((Poisson)distributions[Event.CapableEngaging]).Sample();
            if (child is Female)
            {
                (child as Female).PregnancyAge = ((Normal)distributions[Event.GetPregnant]).Sample();
                (child as Female).ChildrenCount = ((Normal)distributions[Event.ChildrenCount]).Sample();
            }
            if (Engaged && ChildrenCount > 0)
            {
                TimeChildren =
                  currentTime + ((Exponential)distributions[Event.TimeChildren]).Sample();
                Couple.TimeChildren = TimeChildren;
            }
            else
                TimeChildren = 0;
            IsPregnant = false;
            return child;
        }
    }
}
