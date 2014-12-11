using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using VikingsGame.Model.Base;

namespace VikingsGame.Model
{
    class WarriorGroup : GameObject
    {
        public double CostPerTick;

        private int _unitCount;

        public int UnitCount
        {
            get { return _unitCount; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _unitCount = value;
                CostPerTick = UnitCount * GetStatMultiplier("CostPerUnit");
                RaisePropertyChangedEvent("UnitCount");
                OnUpdate(EventArgs.Empty);
            }
        }

        public WarriorGroup(int unitCount)
        {
            _unitCount = unitCount;
            Stats = new Dictionary<string, double>
            {
                {"CostPerUnit", 0.3}
            };
        }

        public bool FightWith(WarriorGroup enemyGroup)
        {
            int u1, u2;
            u2 = enemyGroup.UnitCount;
            u1 = _unitCount;

            enemyGroup.UnitCount = u2 - u1;
            UnitCount = u1 - u2;
            OnUpdate(EventArgs.Empty);

            return UnitCount > 0;
        }
    }
}
