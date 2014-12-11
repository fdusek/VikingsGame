using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using VikingsGame.Model.Base;
using VikingsGame.Observable;

namespace VikingsGame.Model
{
    class City : RealtimeGameObject
    {
        #region Fields
        private double _cash;
        public double Cash
        {
            get { return _cash; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Cash minimum is 0");
                }
                _cash = value;
            }
        }

        public int Id { get; private set; }
        public int FactionId { get; private set; }
        public int Level { get; private set; }

        public string Name
        {
            get;
            private set;
        }

        public double WarriorsAvailable { get; private set; }
        public WarriorGroup Units { get; set; }
        public ObservableCollection<Ship> Ships { get; set; }

        #endregion

        public City(int level, int factionId, int id, string name)
        {
            Name = name;
            Id = id;
            FactionId = factionId;
            Level = level;
            Cash = 10;
            Units = new WarriorGroup(2);
            Ships = new ObservableCollection<Ship>();

            Stats["CashPerTick"] = 10;
            Stats["WarriorsPerTick"] = 3;

            AvailableUpgrades.Add(new Upgrade(1));
            AvailableUpgrades.Add(new Upgrade(0));

            //var dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += _tick;
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            //dispatcherTimer.Start();
        }

        private int GetNewShipId()
        {
            var i = -1;

            for (var index = 0; index < Ships.Count; index++)
            {
                var ship = Ships[index];
                if (ship.Id > i)
                {
                    i = ship.Id;
                }
            }

            return i + 1;
        }

        public void BuildShip()
        {
            var newShip = new Ship(this, GetNewShipId());
            if (newShip.Price < Cash)
            {
                Cash -= newShip.GetStat("Price");

                newShip.LostBattle += ShipLost;
                Ships.Add(newShip);
            }
        }

        public void RecruitWarriors(int count)
        {
            if (count < WarriorsAvailable)
            {
                Units.UnitCount += count;
                WarriorsAvailable -= count;
            }

            //Units.UnitCount += (int)Math.Round(WarriorsAvailable);
            //WarriorsAvailable -= (int)Math.Round(WarriorsAvailable);
        }


        #region Event Handlers
        public override void _tick(object sender, object e)
        {
            WarriorsAvailable += GetStat("WarriorsPerTick");

            Cash += GetStat("WarriorsPerTick") - Units.GetStat("CostPerTick");
            //RaisePropertyChangedEvent("Cash");}}
            OnUpdate(EventArgs.Empty);
        }

        private void ShipLost(object sender, object e)
        {
            Ships.Remove((Ship)sender);
        }

        #endregion
    }
}
