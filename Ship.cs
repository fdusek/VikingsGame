using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using VikingsGame.Model.Base;
using VikingsGame.Other;

namespace VikingsGame.Model
{
    class Ship : RealtimeGameObject
    {
        #region Fields
        public string Name { get; set; }
        public City DestinationCity { get; set; }
        public int Id { get; set; }
        public double Price { get; set; }
        public int FactionId { get; set; }
        public Double TravelRemain { get; set; }
        public WarriorGroup Units { get; set; }
        public string TravelDescription
        {
            get
            {
                if (TravelRemain > 0)
                {
                    return "This ship is now sailing to " + DestinationCity.Name + " and will reach its destination in " + TravelRemain + " seconds";
                }
                else
                {
                    return "This ship is staying safely in waters of "+DestinationCity.Name;
                }
            }
        }

        #endregion

        public Ship(City city, int shipId)
        {
            DestinationCity = city;
            FactionId = city.FactionId;
            Id = shipId;
            TravelRemain = 0;

            AvailableUpgrades.Add(new Upgrade(0) { Name = "Unsinkable" });
            AvailableUpgrades.Add(new Upgrade(0) { Name = "Undefeatable" });

            BuiltUpgrades.Add(new Upgrade(0) { Name = "Bad upgrade 1" });
            BuiltUpgrades.Add(new Upgrade(0) { Name = "Bad upgrade 2" });

            Name = ShipNames.GetRandomName();

            Stats["Speed"] = 0;
            Price = 5;

            Units = new WarriorGroup(0);
            Units.Update += Refresh;
        }

        public void SailTo(City destination)
        {
            //TODO: Count travel distance
            TravelRemain = 10;
            DestinationCity = destination;
        }

        public override string ToString()
        {
            return "This ship has ID " + Id + " its target city ID is " + DestinationCity.Id + " and has " + Units.UnitCount +
                " units, time remaining " + TravelRemain;
        }

        #region Event Handlers
        private void Refresh(object sender, object e)
        {
            Stats["Speed"] = Units.UnitCount;
        }

        public override void _tick(object sender, object e)
        {
            if (TravelRemain > 0)
            {
                TravelRemain -= Stats["Speed"];
                RaisePropertyChangedEvent("TravelDescription");
                RaisePropertyChangedEvent("TravelRemain");
            }
            else
            {
                OnReachedDestination(EventArgs.Empty);
                if ((DestinationCity.FactionId != FactionId) && (DestinationCity.FactionId != 0))
                {
                    if (!Units.FightWith(DestinationCity.Units))
                    {
                        OnLostBattle(EventArgs.Empty);
                    }
                    RaisePropertyChangedEvent("Units");

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


        public event EventHandler LostBattle;
        public event EventHandler ReachedDestination;
        #endregion
    }
}
