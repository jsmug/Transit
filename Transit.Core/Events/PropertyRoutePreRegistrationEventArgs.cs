namespace Transit.Core.Events
{

    public class PropertyRoutePreRegistrationEventArgs : PropertyRouteRegistrationEventArgs
    {

        private PropertyRoutePreRegistrationEventArgs() : base(null)
        {
        }

        public PropertyRoutePreRegistrationEventArgs(PropertyRoute propertyRoute) : base(propertyRoute)
        {
        }


        #region public

        public bool Cancel { get; set; }

        #endregion

    }

}
