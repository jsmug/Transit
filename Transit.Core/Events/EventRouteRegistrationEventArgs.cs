using System;

namespace Transit.Core.Events
{

    public class EventRouteRegistrationEventArgs : EventArgs
    {

        private EventRoute _eventRoute;


        private EventRouteRegistrationEventArgs() : base()
        {
        }

        public EventRouteRegistrationEventArgs(EventRoute eventRoute) : base()
        {

            if (eventRoute == null)
            {
                throw new ArgumentNullException("eventRoute");
            }

            this._eventRoute = eventRoute;

        }


        #region public

        public EventRoute EventRoute
        {

            get
            {
                return this._eventRoute;
            }

        }

        #endregion

    }

}
