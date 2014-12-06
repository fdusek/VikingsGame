using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using VikingsGame.Observable;

namespace VikingsGame.Model
{
    class City : ObservableObject
    {
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
                else
                {
                    _cash = value;
                }
            }
        }

        public int Id;
        public int FactionId;
        public int Level;
        private double CashPerTick = 1;
        private double WarriorsPerTick = 0.05;
        public double WarriorsAvailable { get; private set; }
        public WarriorGroup Units;
        public Collection<Ship> Ships;

        public City()
        {
            Cash = 10;
            Units = new WarriorGroup(2);
            Ships = new ObservableCollection<Ship>();

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += _tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimer.Start();
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
                Cash -= newShip.Price;
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

            Units.UnitCount += (int)Math.Round(WarriorsAvailable);
            WarriorsAvailable -= (int)Math.Round(WarriorsAvailable);
        }
        #region Event Handlers

        private void ShipLost(object sender, object e)
        {
            Ships.Remove((Ship)sender);
        }

        #endregion

        #region Events
        public void _tick(object sender, object e)
        {
            WarriorsAvailable += WarriorsPerTick;
            Cash += CashPerTick - Units.CostPerTick;
            RaisePropertyChangedEvent("Cash");
            OnUpdate(EventArgs.Empty);
        }

        protected virtual void OnUpdate(EventArgs e)
        {
            EventHandler handler = Update;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler Update;
        #endregion
    }
}
