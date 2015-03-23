using System;

namespace Transit.Core.Events
{
    
    public class PropertyRouteRegistrationEventArgs : EventArgs
    {

        private PropertyRoute _propertyRoute;


        private PropertyRouteRegistrationEventArgs() : base()
        {
        }

        public PropertyRouteRegistrationEventArgs(PropertyRoute propertyRoute) : base()
        {

            if (propertyRoute == null)
            {
                throw new ArgumentNullException("propertyRoute");
            }

            this._propertyRoute = propertyRoute;

        }


        #region public

        public PropertyRoute PropertyRoute
        {

            get
            {
                return this._propertyRoute;
            }

        }

        #endregion

    }

}
