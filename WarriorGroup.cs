using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;

namespace VikingsGame.Model
{
    class WarriorGroup
    {
        private double CostPerUnit = 0.3;
        public double CostPerTick;
        
        private int _unitCount;

        public int UnitCount
        {
            get { return _unitCount; }
            set {
                if (value < 0 )
                {
                    value = 0;
                }
                _unitCount = value;
                CostPerTick = UnitCount*CostPerUnit;
                OnUpdate(EventArgs.Empty);
            }
        }

        public WarriorGroup(int unitCount)
        {
            _unitCount = unitCount;
            CostPerTick = UnitCount * CostPerUnit;
        }

        public bool FightWith(WarriorGroup enemyGroup)
        {
            int u1, u2;
            u2 = enemyGroup.UnitCount;
            u1 = _unitCount;

            enemyGroup.UnitCount = u2-u1;
            UnitCount = u1 - u2;
            OnUpdate(EventArgs.Empty);

            return UnitCount > 0;
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

    }
}
