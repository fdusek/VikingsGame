using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace VikingsGame.Model
{
    class Ship
    {
        public City TargetCity;
        public int Id;
        private Double _speed;
        public int FactionId;
        public Double TravelRemain;
        public WarriorGroup Units;
        public int Price = 5;

        public Ship(City city, int shipId)
        {
            TargetCity = city;
            Id = shipId;
            TravelRemain = 0;
            _speed = 0;

            Units = new WarriorGroup(0);
            Units.Update += Refresh;
            

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += _tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimer.Start();
        }



        public void SailTo(City to)
        {
            TravelRemain = 10;
            TargetCity = to;
        }

        public override string ToString()
        {
            return "This ship has ID " + Id + " its target city ID is " + TargetCity.Id + " and has " + Units.UnitCount +
                " units, time remaining " + TravelRemain;
        }

        #region Event Handlers
        private void Refresh(object sender, object e)
        {
            _speed = Units.UnitCount;
        }

        private void _tick(object sender, object e)
        {
            if (TravelRemain > 0)
            {
                TravelRemain -= _speed;
            }
            else
            {
                OnReachedDestination(EventArgs.Empty);
                if ((TargetCity.FactionId != FactionId) && (TargetCity.FactionId != 0))
                {
                    if (!Units.FightWith(TargetCity.Units))
                    {
                        OnLostBattle(EventArgs.Empty);
                    }
                }
            }
            OnUpdate(EventArgs.Empty);
        }
        #endregion

        #region Events
        protected virtual void OnLostBattle(EventArgs e)
        {
            EventHandler handler = LostBattle;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnReachedDestination(EventArgs e)
        {
            EventHandler handler = ReachedDestination;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnUpdate(EventArgs e)
        {
            EventHandler handler = Update;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler LostBattle;
        public event EventHandler ReachedDestination;
        public event EventHandler Update;

        #endregion
    }
}
