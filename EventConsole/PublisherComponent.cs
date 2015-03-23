using System;
using System.Threading;
using Transit.Core;

namespace EventConsole
{
    
    public class PublisherComponent : Component, IDisposable
    {

        #region "constants"

        private const int TimerInterval = 5000;

        #endregion


        private readonly SynchronizationContext _context;
        private bool _isDisposed;
        private readonly Timer _timer;


        [RouteOut]
        public event EventHandler IntervalEvent = delegate { };


        public PublisherComponent()
            : base("Publisher")
        {

            this._context = SynchronizationContext.Current;
            this._timer = new Timer(TimerFire, this._context, TimerInterval, TimerInterval);

        }


        #region public

        public void Dispose()
        {

            GC.SuppressFinalize(this);
            OnDispose(true);

        }

        #endregion

        #region protected

        protected virtual void OnDispose(bool disposing)
        {

            if (disposing && !this._isDisposed)
            {

                if (this._timer != null)
                {

                    this._timer.Change(Timeout.Infinite, Timeout.Infinite);
                    this._timer.Dispose();

                }

            }

            this._isDisposed = true;

        }

        protected virtual void OnIntervalEvent()
        {
            IntervalEvent(this, EventArgs.Empty);
        }

        #endregion

        #region private

        private void MarshalCallback(object state)
        {
            OnIntervalEvent();
        }

        private void TimerFire(object state)
        {

            SynchronizationContext context = state as SynchronizationContext;

            if (context != null)
            {
                context.Post(new SendOrPostCallback(MarshalCallback), null);
            }

        }

        #endregion

    }

}
