using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tfo.Utils.Io.BaseClasses
{
    public delegate void StartableStartedEventHandler(IStartable startable);
    public delegate void StartableStoppedEventHandler(IStartable startable);

    public interface IStartable
    {
        event StartableStartedEventHandler StartableStarted;
        event StartableStoppedEventHandler StartableStopped;

        void Start();
        void Stop();
    }

    public abstract class Startable : IStartable
    {
        public event StartableStartedEventHandler StartableStarted;
        public event StartableStoppedEventHandler StartableStopped;

        public abstract void OnStart();

        public abstract void OnStop();

        public void Start()
        {
            OnStart();

            if (StartableStarted != null)
                StartableStarted(this);
        }

        public void Stop()
        {
            OnStop();

            if (StartableStopped != null)
                StartableStopped(this);
        }
    }

}
