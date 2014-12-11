using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VikingsGame.Observable;
using VikingsGame.Other;

namespace VikingsGame.Model.Base
{
    class GameObject:ObservableObject
    {
        public Dictionary<string, double> Stats { get; protected set; }
        public event EventHandler Update;
        public Collection<Upgrade> AvailableUpgrades { get; set; }
        public Collection<Upgrade> BuiltUpgrades { get; protected set; }

        public GameObject()
        {
            Stats = new Dictionary<string, double>();
            AvailableUpgrades = new Collection<Upgrade>();
            BuiltUpgrades = new Collection<Upgrade>();
        }

      
        public Collection<StatsWorkaround> StatsAsWorkaround()
        {
            var temp = new Collection<StatsWorkaround>();
            foreach (var pair in Stats)
            {
                temp.Add(new StatsWorkaround() { Key = pair.Key, Value = pair.Value });
            }
            return temp;
        }

        public void Upgrade(int id)
        {
            var toGet = AvailableUpgrades.FirstOrDefault(upgrade => upgrade.Id == id);

            foreach (var newStat in toGet.Stats)
            {
                if (Stats.ContainsKey(newStat.Key))
                {
                    Stats[newStat.Key] += newStat.Value;
                }
                else
                {
                    Stats[newStat.Key] = newStat.Value;
                }
            }

            BuiltUpgrades.Add(toGet);
            AvailableUpgrades.Remove(toGet);
        }

        public double GetStat(string key)
        {
            double result;
            return ((Stats != null) && Stats.TryGetValue(key, out result)) ? result : 0;
        }

        public double GetStatMultiplier(string key)
        {
            double result;
            return ((Stats != null) && Stats.TryGetValue(key, out result)) ? result : 1;
        }

        protected virtual void OnUpdate(EventArgs e)
        {
            EventHandler handler = Update;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
