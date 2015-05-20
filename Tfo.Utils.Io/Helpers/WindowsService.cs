using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Tfo.Utils.Io.BaseClasses;

namespace Tfo.Utils.Io.Helpers
{
    /// <summary>
    /// Usage: 
    ///     static main method is meant to live in your Program.cs file.
    ///     Set up the bootstrapper there, with all of your IStartable 
    ///     Implementations.
    /// </summary>
    public class WindowsService : ServiceBase
    {
        public Bootstrapper _bootstrapper;

        public WindowsService(Bootstrapper bootstrapper, string serviceName)
        {
            ServiceName = serviceName;
            CanStop = true;
            CanPauseAndContinue = false;
            AutoLog = true;

            _bootstrapper = bootstrapper;
        }

        protected override void OnStart(string[] args)
        {
            _bootstrapper.Start();
        }

        protected override void OnStop()
        {
            _bootstrapper.Stop();
        }
    }

    public class Bootstrapper : Startable
    {
        private readonly bool _startAsynch;
        private readonly List<Startable> _startables;

        private readonly List<Task> _tasksStarting;
        public List<Task> TasksStarting
        {
            get { return _tasksStarting; }
        }

        public Bootstrapper(bool startAsynch)
        {
            _startAsynch = startAsynch;
            _startables = new List<Startable>();

            _tasksStarting = new List<Task>();
        }

        public void AddStartable(Startable startable)
        {
            _startables.Add(startable);
        }

        #region Startable Implementation

        public override sealed void OnStart()
        {
            if (_startAsynch)
            {
                _startables.ForEach(e =>
                    {
                        var t = Task.Factory.StartNew(() =>
                            {
                                e.Start();
                            });

                        _tasksStarting.Add(t);
                    });
            }
            else
            {
                _startables.ForEach(e =>
                {
                    e.Start();
                });
            }
        }

        public override sealed void OnStop()
        {
            var reversed = Enumerable.Reverse(_startables)
                .ToList();

            reversed.ForEach(e =>
            {
                e.Stop();
            });
        }

        #endregion
    }
}
