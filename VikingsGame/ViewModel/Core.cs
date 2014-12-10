using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using VikingsGame.Helpers;
using VikingsGame.Model;
using VikingsGame.Observable;

namespace VikingsGame.ViewModel
{
    class Core : ObservableObject
    {

        private static Core _instance;

        public static Core Instance
        {
            get
            {
                if (_instance != null) return _instance;
                else return _instance = new Core();
            }
        }

        public string DebugStillMsg { get; set; }
        public ICommand BuildShip
        {
            get { return new DelegateCommand(_buildShip); }
        }

        public void _buildShip()
        {
            PlayerCity.BuildShip();

            //PlayerCity.RecruitWarriors(100);}}}

            //PlayerCity.Units.UnitCount += 1;
            //PlayerCity.Units.UnitCount += (int)Math.Round(PlayerCity.WarriorsAvailable);
            //PlayerCity.WarriorsAvailable -= (int)Math.Round(PlayerCity.WarriorsAvailable);
            //var army1 = new WarriorGroup(Convert.ToInt32(Val1));
            //var army2 = new WarriorGroup(Convert.ToInt32(Val2));
            //army1.FightWith(army2);
            //DebugMsg = "Army 1 is left with " + army1.UnitCount;
            //RaisePropertyChangedEvent("DebugMsg");
        }

        public ICommand Save
        {
            get { return new DelegateCommand(_save); }
        }

        public void _save()
        {
            using (var db = new SQLite.SQLiteConnection(App.DBPath))
            {
                try
                {
                    db.Insert(PlayerCity);
                }
                catch (Exception e)
                {
                    new Windows.UI.Popups.MessageDialog("Couldn't save city to database :(" + Environment.NewLine +
                                                        e.Message).ShowAsync();
                }
            }
            new Windows.UI.Popups.MessageDialog(App.DBPath).ShowAsync();
        }

        public ICommand Sail
        {
            get { return new DelegateCommand(_sail); }
        }

        public void _sail()
        {
            var result = PlayerCity.Ships.FirstOrDefault(ship => ship.Id == Convert.ToInt32(Val1));
            var result2 = _enemyCities.FirstOrDefault(city => city.Id == Convert.ToInt32(Val2));
            if ((result != null) && (result2 != null))
            {
                result.SailTo(result2);
                DebugMsg = result.ToString();
            }
        }

        public ICommand Upgrade
        {
            get { return new DelegateCommand(_upgrade); }
        }

        public void _upgrade()
        {
            PlayerCity.Upgrade(0);
        }



        public ICommand Transfer
        {
            get { return new DelegateCommand(_transfer); }
        }

        public void _transfer()
        {
            Ship result = PlayerCity.Ships.FirstOrDefault(ship => ship.Id == Convert.ToInt32(Val1));
            if (result != null)
            {
                if (PlayerCity.Units.UnitCount >= 1)
                {
                    result.Units.UnitCount += 1;
                    PlayerCity.Units.UnitCount -= 1;
                }

                DebugMsg = result.ToString();
            }
        }

        private string _debugMsg;



        public string DebugMsg
        {
            get { return _debugMsg; }
            set
            {
                DebugMsgs.Add(value);
                _debugMsg = value;

            }
        }

        public string Val1 { get; set; }
        public string Val2 { get; set; }
        public string Val3 { get; set; }


        private City _playerCity = new City(1, 1, 0);

        public City PlayerCity
        {
            get { return _playerCity; }
            set { _playerCity = value; }
        }

        private Collection<City> _enemyCities = new Collection<City>();

        public ObservableCollection<String> DebugMsgs { get; set; }


        public Collection<City> EnemyCities
        {
            get { return _enemyCities; }
            set { _enemyCities = value; }
        }

        public Core()
        {
            DebugMsgs = new ObservableCollection<string>();

            Val1 = "0";

            PlayerCity.Update += OnUpdate;
            PlayerCity.Ships.Add(new Ship(PlayerCity, 0));
            PlayerCity.Ships.Add(new Ship(PlayerCity, 1));
            PlayerCity.Ships.Add(new Ship(PlayerCity, 2));
            _enemyCities.Add(new City(1, 2, 1) { Cash = 10, Units = new WarriorGroup(1) });
            DebugMsg = "enemy city added with ID 1, faction id 2, cash 10 and 1 unit";
        }

        void OnUpdate(object sender, EventArgs e)
        {
            DebugStillMsg = "Ship count: " + PlayerCity.Ships.Count + " Gold: " + PlayerCity.Cash + " PlayerCity avableble units: " + Math.Round(PlayerCity.WarriorsAvailable, 2) + " Current wariors: " +
                            PlayerCity.Units.UnitCount;
            RaisePropertyChangedEvent("DebugStillMsg");

            var temp = PlayerCity.Ships.FirstOrDefault(ship => ship.Id == Convert.ToInt32(Val1));

            if (temp != null)
            {
                DebugMsg = temp.ToString();
            }
        }
    }
}
