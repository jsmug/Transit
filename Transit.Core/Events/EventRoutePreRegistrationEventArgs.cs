namespace Transit.Core.Events
{

    public class EventRoutePreRegistrationEventArgs : EventRouteRegistrationEventArgs
    {

        private EventRoutePreRegistrationEventArgs() : base(null)
        {
        }

        public EventRoutePreRegistrationEventArgs(EventRoute eventRoute) : base(eventRoute)
        {
        }


        #region public

        public bool Cancel { get; set; }

        #endregion

    }

}
