using System.Collections.Generic;
using System.Collections.ObjectModel;
using VikingsGame.Other;

namespace VikingsGame.Model.Base
{
    internal class Upgrade
    {
        public Dictionary<string, double> Stats { get; set; }

        public string Name { get; set; }

        public int Id { get; private set; }
        public double Price { get; set; }

        public Collection<StatsWorkaround> StatsAsWorkaround()
        {
            var temp = new Collection<StatsWorkaround>();
            foreach (var pair in Stats)
            {
                temp.Add(new StatsWorkaround() { Key = pair.Key, Value = pair.Value });
            }
            return temp;
        }


        public Upgrade(int id)
        {
            Id = id;
            Stats = new Dictionary<string, double> { { ShipNames.GetRandomName(), 10 }, { ShipNames.GetRandomName(), 100 } };
        }
    }
}
