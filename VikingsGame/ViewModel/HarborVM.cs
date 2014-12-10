using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using VikingsGame.Helpers;
using VikingsGame.Model;
using VikingsGame.Model.Base;
using VikingsGame.Observable;
using VikingsGame.Other;

namespace VikingsGame.ViewModel
{
    // ReSharper disable once InconsistentNaming
    class HarborVM : ObservableObject
    {
        #region Ships

        private ObservableCollection<Ship> _ships;

        public ObservableCollection<Ship> Ships
        {
            get { return _ships; }
            set { _ships = value; }
        }


        private Ship _selectedShip;

        public Ship SelectedShip
        {
            get { return _selectedShip; }
            set
            {
                _selectedShip = value;
                RaisePropertyChangedEvent("SelectedShip");

                AvailableUpgrades = new ObservableCollection<Upgrade>(_selectedShip.AvailableUpgrades);
                BuiltUpgrades = new ObservableCollection<Upgrade>(_selectedShip.BuiltUpgrades);

                RaisePropertyChangedEvent("AvailableUpgrades");
                RaisePropertyChangedEvent("BuiltUpgrades");


                RaisePropertyChangedEvent("Stats");
            }
        }

        private void Blah(object sender, object e)
        {
            
        }

        #endregion

        #region Upgrades

        public ObservableCollection<Upgrade> AvailableUpgrades { get; set; }
        public ObservableCollection<Upgrade> BuiltUpgrades { get; set; }

        public ICommand BuildUpgradeClick
        {
            get { return new DelegateCommand(_buildUpgradeClick); }
        }

        private void _buildUpgradeClick()
        {
            if ((SelectedShip != null) && (SAvailableUpgrade != null))
            {
                if (_currentCity.Cash >= SAvailableUpgrade.Price)
                {
                    _currentCity.Cash -= SAvailableUpgrade.Price;
                    SelectedShip.Upgrade(SAvailableUpgrade.Id);
                    BuiltUpgrades.Add(SAvailableUpgrade);
                    AvailableUpgrades.Remove(SAvailableUpgrade);
                }
                else
                {
                    new MessageDialog("Not enough cash.").ShowAsync();
                }
            }
            else
            {
                //todo: make this text more viking
                new MessageDialog("You have to select upgrade first.").ShowAsync();
            }
            Stats = new ObservableCollection<StatsWorkaround>(_selectedShip.StatsAsWorkaround());
            RaisePropertyChangedEvent("Stats");

            RaisePropertyChangedEvent("BuiltUpgrades");
            RaisePropertyChangedEvent("AvailableUpgrades");
        }


        private Upgrade _sAvailableUpgrade;

        public Upgrade SAvailableUpgrade
        {
            get { return _sAvailableUpgrade; }
            set
            {
                _sAvailableUpgrade = value;
                if (_sAvailableUpgrade != null)
                {
                    _upgradeStats = new ObservableCollection<StatsWorkaround>(_sAvailableUpgrade.StatsAsWorkaround());
                }
                else
                {
                    _upgradeStats.Clear();
                }
                RaisePropertyChangedEvent("UpgradeStats");
            }
        }

        private Upgrade _sBuiltUpgrade;

        public Upgrade SBuiltUpgrade
        {
            get { return _sBuiltUpgrade; }
            set
            {
                _sBuiltUpgrade = value;
                if (_sBuiltUpgrade != null)
                {
                    _upgradeStats = new ObservableCollection<StatsWorkaround>(_sBuiltUpgrade.StatsAsWorkaround());
                }
                else
                {
                    _upgradeStats.Clear();
                }
                RaisePropertyChangedEvent("UpgradeStats");
            }
        }

        private ObservableCollection<StatsWorkaround> _upgradeStats;

        public ObservableCollection<StatsWorkaround> UpgradeStats
        {
            get { return _upgradeStats; }
            set { _upgradeStats = value; }
        }

        #endregion

        #region Ship detail

        private ObservableCollection<StatsWorkaround> _stats;

        public ObservableCollection<StatsWorkaround> Stats
        {
            get
            {
                if (_selectedShip != null)
                    _stats = new ObservableCollection<StatsWorkaround>(_selectedShip.StatsAsWorkaround());
                return _stats;
            }
            set { _stats = value; }
        }

        #endregion

        #region Cities

        private ObservableCollection<City> _cities;

        public ObservableCollection<City> Cities
        {
            get { return _cities; }
            set { _cities = value; }
        }

        public City SelectedCity { get; set; }

        public ICommand SailToClick
        {
            get { return new DelegateCommand(_sailToClick); }
        }

        private void _sailToClick()
        {
            if ((SelectedCity != null) & (SelectedShip != null))
            {
                //todo: subtract from city, that owns this harbor (it doesnt always have to be players city)
                //todo: sail should cost money
                SelectedShip.SailTo(SelectedCity);
            }
            else
            {
                //todo: make this text more viking
                new MessageDialog("You have to select ship and city first.").ShowAsync();
            }
            Stats = new ObservableCollection<StatsWorkaround>(_selectedShip.StatsAsWorkaround());
            RaisePropertyChangedEvent("Stats");

            RaisePropertyChangedEvent("BuiltUpgrades");
            RaisePropertyChangedEvent("AvailableUpgrades");
        }


        #endregion

        private City _currentCity;

        public City CurrentCity
        {
            get { return _currentCity; }
            set { _currentCity = value; }
        }
        
        public HarborVM()
        {
            _currentCity = Core.Instance.PlayerCity;
            _ships = new ObservableCollection<Ship>(_currentCity.Ships);
            _stats = new ObservableCollection<StatsWorkaround>();
            //todo: Harbor owner doesnt have to be always player, so not always EnemyCities
            _cities = new ObservableCollection<City>(Core.Instance.EnemyCities);
        }
    }
}
