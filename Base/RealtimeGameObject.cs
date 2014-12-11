using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace VikingsGame.Model.Base
{
    abstract class RealtimeGameObject : GameObject
    {
        readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        protected RealtimeGameObject()
        {
            _dispatcherTimer.Tick += _tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            _dispatcherTimer.Start();
        }

        public abstract void _tick(object sender, object e);
    }
}
